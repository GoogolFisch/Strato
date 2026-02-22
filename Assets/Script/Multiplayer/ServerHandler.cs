
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
    public bool isServer;
    //public List<byte[]> fetchedInternet;
    public Socket serverSocket;
    public List<DirConnection> clCons;
    public DirConnection dirServerCon;

    public ServerHandler(Socket listener,bool isServer)
    {
        serverSocket = listener;
        if (isServer)
        {
            clCons = new List<DirConnection>();
            listener.Bind(new IPEndPoint(IPAddress.Any,LanConnector.PORT));
            listener.Listen(2);
        }
        else
        {
            dirServerCon = new DirConnection(listener);
        }
    }

    public void Handel()
    {
        
    }
    public void AddPacket(Packet p)
    {
        if (!isServer)
            dirServerCon.AddOutgoingPacket(p);
        else
            foreach(DirConnection dc in clCons)
                dc.AddOutgoingPacket(p);
    }
}
