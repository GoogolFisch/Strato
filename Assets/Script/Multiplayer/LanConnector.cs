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
    public const int PORT_UDP = 25350;
    private readonly int _port;
    public int portService;
    // volitile?
    private bool _isBroadcasting;
    private UdpClient _udpClient;
    private TaskCompletionSource<(IPEndPoint, byte[])> _onListenTcs;

    public LanConnector(int port = PORT)
    {
        _port = PORT_UDP;
        portService = port;
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
            barr.AddRange(BitConverter.GetBytes(1 + ldc.Count));
            barr.AddRange(BitConverter.GetBytes(1 + MemoryHandler.mh.maxPlCnt));
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
        Debug.Log($"{nameof(LanConnector)}: Listening for local sessions on port {_port}.");

        IPEndPoint bindEP = new IPEndPoint(IPAddress.Any, _port);
        _udpClient = new UdpClient(bindEP);
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
        IPEndPoint listEP = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = Array.Empty<byte>();

        try
        {
            data = udpClient.EndReceive(asyncResult, ref listEP);
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

        _onListenTcs.TrySetResult((listEP, data));
        StopListening();
    }

    internal void StopListening()
    {
        _udpClient?.Dispose();
        _udpClient = null;
    }

    public async void StartBroadcasting(byte[] sessionInformation, float broadcastInterval = 0.5f)
    {
        Debug.Log($"{nameof(LanConnector)}: Broadcasting session on port {_port} from {portService}.");

        _isBroadcasting = true;

        try
        {
            //var ipEndPoint = new IPEndPoint(IPAddress.Broadcast, _port);
            var ipEndPoint = new IPEndPoint(IPAddress.Loopback, _port);
            var ipEndPointBrd = new IPEndPoint(IPAddress.Broadcast, _port);
            using (var senderSocket = new UdpClient(portService))
            {
                senderSocket.EnableBroadcast = true;
                senderSocket.MulticastLoopback = true;

                while (_isBroadcasting)
                {
                    byte[] sesInfo;
                    if(GameManager.gm != null)sesInfo = MakeSessionInfo(GameManager.gm.playerName);
                    else sesInfo = sessionInformation;
                    if(DeLogger.dl != null)
                        DeLogger.dl.Log($"BRD {ipEndPoint}");
                    senderSocket.Send(sesInfo,sesInfo.Length, ipEndPoint);
                    senderSocket.Send(sesInfo,sesInfo.Length, ipEndPointBrd);
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