using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
//using System.Diagnostics;

// https://github.com/oculus-samples/Unity-LocalMultiplayerMR/blob/main/colocation-sample-ngo/Assets/Scripts/Samples/LocalNetworkDiscovery.cs
public class LanConnector : IDisposable
{
    public const int PORT = 25359;
    private readonly int _port;
    // volitile?
    private bool _isBroadcasting;
    private UdpClient _udpClient;
    private TaskCompletionSource<(IPEndPoint, byte[])> _onListenTcs;

    public LanConnector(int port = PORT)
    {
        _port = port;
    }
    public void Dispose()
    {
        StopListening();
        StopBroadcasting();
    }

    public static byte[] MakeSessionInfo(string str)
    {
        const int VERSION = 1;
        List<byte> barr = new List<byte>();
        barr.AddRange(BitConverter.GetBytes(VERSION));
        barr.AddRange(Packet.ConvertFromString(str));
        List<DirConnection> ldc = MemoryHandler.mh.shan.clCons;
        if(ldc != null){
            barr.AddRange(BitConverter.GetBytes(MemoryHandler.mh.shan.clCons.Count));
            barr.AddRange(BitConverter.GetBytes(MemoryHandler.mh.maxPlCnt));
        }else{
            barr.AddRange(BitConverter.GetBytes(0));
            barr.AddRange(BitConverter.GetBytes(0));
        }
        
        return barr.ToArray();
    }
    public static StructMenuListing GetSessionInfo(byte[] mes){
        StructMenuListing sml = new StructMenuListing();
        int offset = 4;
        sml.version = BitConverter.ToInt32(mes);
        sml.display = Packet.ConvertToString(mes,ref offset);
        sml.plCount = BitConverter.ToInt32(mes,offset);
        sml.maxPlCnt = BitConverter.ToInt32(mes,offset + 4);
        offset += 8;
        return sml;
    }


    public async Task<(IPEndPoint, byte[])> ListenForConnection()
    {
        _onListenTcs = new();
        //Debug.Log($"{nameof(LanConnector)}: Listening for local sessions on port {_port}.");

        _udpClient = new UdpClient(_port);
        _udpClient.ExclusiveAddressUse = false;
        _udpClient.BeginReceive(Receive, _udpClient);
        var resultTask = await Task.WhenAny(_onListenTcs.Task, CreateHostSessionTask());
        return resultTask.Result;
    }

    private async Task<(IPEndPoint, byte[])> CreateHostSessionTask()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return (null, null);
    }

    private void Receive(IAsyncResult asyncResult)
    {
        UdpClient udpClient = (UdpClient)(asyncResult.AsyncState);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Broadcast, _port);
        byte[] data = Array.Empty<byte>();

        try
        {
            data = udpClient.EndReceive(asyncResult, ref ipEndPoint);
        }
        catch (ObjectDisposedException)
        {
            Debug.Log($"{nameof(LanConnector)}: Stopped listening for local sessions.");
            return;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        _onListenTcs.TrySetResult((ipEndPoint, data));
        StopListening();
    }

    private void StopListening()
    {
        _udpClient?.Dispose();
        _udpClient = null;
    }

    public async void StartBroadcasting(byte[] sessionInformation, float broadcastInterval = 0.5f)
    {
        Debug.Log($"{nameof(LanConnector)}: Broadcasting session on port {_port}.");

        _isBroadcasting = true;

        try
        {
            //var ipEndPoint = new IPEndPoint(IPAddress.Broadcast, _port);
            var ipEndPoint = new IPEndPoint(IPAddress.Loopback, _port);
            using (var senderSocket = new UdpClient())
            {
                senderSocket.EnableBroadcast = true;
                senderSocket.MulticastLoopback = true;

                while (_isBroadcasting)
                {
                    if(DeLogger.dl != null)
                        DeLogger.dl.Log($"BRD {ipEndPoint}");
                    senderSocket.Send(sessionInformation,
                        sessionInformation.Length, ipEndPoint);
                    await Task.Delay(TimeSpan.FromSeconds(broadcastInterval));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            _isBroadcasting = false;
        }

        Debug.Log($"{nameof(LanConnector)}: Stopped broadcasting.");
    }

    public void StopBroadcasting()
    {
        _isBroadcasting = false;
    }
}