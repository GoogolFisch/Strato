
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Data.Common;
using System.IO;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Reflection;
using System.Reflection.Emit;
using System.Net;

public class ServerHandler : IDisposable
{
    //public List<byte[]> fetchedInternet;
    public TcpListener serverSocket;
    public List<DirConnection> clCons;
    public DirConnection dirServerCon;

    public ServerHandler(TcpListener listener)
    {
        clCons = new List<DirConnection>();
        //listener.Bind(new IPEndPoint(IPAddress.Any,LanConnector.PORT));
        //listener.Listen(2);
        serverSocket = listener;
    }
    public ServerHandler(TcpClient socket)
    {
        //serverSocket = socket.Client;
        dirServerCon = new DirConnection(socket);
    }

    public void Handel()
    {
        int count = 32;
        while(HandelPacket())
            count--;
    }
    public bool HandelPacket(){
        bool succ = false;
        if (clCons == null){
            dirServerCon.Handel();
            if(dirServerCon.HasIncomming()){
                Packet pck = dirServerCon.PopIncommingPacket();
                pck.ActUppon();
                succ = true;
            }
            return succ;
        }
        // server side====
        foreach(DirConnection dc in clCons)
            dc.Handel();
        //serverSocket.Start(5);
        if(serverSocket.Pending()){
            TcpClient tcpClient = serverSocket.AcceptTcpClient();
            DeLogger.dl.Log($"pend {tcpClient.Client.RemoteEndPoint}");
            clCons.Add(new DirConnection(tcpClient));
        }
        foreach(DirConnection dc in clCons){
            if(dc.HasIncomming()){
                succ = true;

                Packet pck = dc.PopIncommingPacket();
                pck.ActUppon();
                if(pck.GetType() != typeof(PingPacket))continue;
                foreach(DirConnection dc2 in clCons){
                    if(dc2 == dc)continue;
                    dc2.AddOutgoingPacket(pck);
                }
            }
        }
        for(int i = 0;i < clCons.Count;i++){
            if(clCons[i].alive)continue;
            clCons[i].Dispose();
            clCons.RemoveAt(i);
            i--;
        }
        return succ;
    }

    public void AddPacket(Packet p)
    {
        if (clCons == null)
            dirServerCon.AddOutgoingPacket(p);
        else
            foreach(DirConnection dc in clCons)
                dc.AddOutgoingPacket(p);
    }
    public void Dispose(){
        if(dirServerCon != null){
            dirServerCon.Dispose();
        }
        if(clCons != null){
            foreach(DirConnection dc in clCons)
                dc.Dispose();
        }
        if(serverSocket != null){
            serverSocket.Stop();
            //serverSocket.Dispose();
        }
    }
}
