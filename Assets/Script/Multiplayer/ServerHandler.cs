
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

public class ServerHandler
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
        if (clCons == null)
            dirServerCon.Handel();
        else
        {
            foreach(DirConnection dc in clCons)
                dc.Handel();
            if(serverSocket.Pending()){
                TcpClient tcpClient = serverSocket.AcceptTcpClient();
                DeLogger.dl.Log($"pend {tcpClient.Client.RemoteEndPoint}");
                clCons.Add(new DirConnection(tcpClient));
            }
        }
    }

    public void AddPacket(Packet p)
    {
        DeLogger.dl.Log($"{p}");
        if (clCons == null)
            dirServerCon.AddOutgoingPacket(p);
        else
            foreach(DirConnection dc in clCons)
                dc.AddOutgoingPacket(p);
    }
}
