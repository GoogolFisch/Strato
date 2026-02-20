
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

public class ServerConnection
{
    public bool isServer;
    int lastTickRecv = 0;
    //public List<byte[]> fetchedInternet;
    public Socket serverSocket;
    public ConPairs endps;

    public List<Packet> outgoingPackets;
    public List<Packet> incommingPackets;

    public List<Packet> getPackets(byte[] message)
    {
        List<Packet> pack = new List<Packet>();
        int fetchedTick = BitConverter.ToInt32(message,4);
        int index = 8;
        // TODO

        int delta = lastTickRecv - fetchedTick;
        if(delta > 0)lastTickRecv = fetchedTick;
        return pack;
    }
    public byte[] Encrypt(byte[] plaintext,byte[] symkey)
    {
        if(plaintext.Length <= 3)return new byte[0];
        byte[] bout = new byte[((plaintext.Length - 1) | 0xf) + 1];
        bout[0] = (byte)UnityEngine.Random.Range(0,0x100);
        bout[1] = (byte)UnityEngine.Random.Range(0,0x100);
        bout[2] = (byte)UnityEngine.Random.Range(0,0x100);
        bout[3] = (byte)UnityEngine.Random.Range(0,0x100);
        int idx;
        for(idx = 0;idx < plaintext.Length; idx++)
            bout[idx] = plaintext[idx];
        //
        for(int keyOff = 0;keyOff < 16;keyOff += 4){
            for(idx = 4;idx < bout.Length; idx++)
                bout[idx] ^= (byte)((bout[idx - 2] + bout[idx - 3]) >> 1);
            for(idx = 0;idx < bout.Length; idx += 4){
                bout[idx    ] ^= (byte)(bout[idx + 1] ^ symkey[keyOff    ] ^ (bout[idx + 3] >> 1));
                bout[idx + 1] += (byte)(bout[idx + 2] ^ symkey[keyOff + 1] ^ (bout[idx    ] >> 1));
                bout[idx + 2] += (byte)(bout[idx + 3] ^ symkey[keyOff + 2] ^ (bout[idx + 1] >> 1));
                bout[idx + 3] += (byte)(bout[idx    ] ^ symkey[keyOff + 3] ^ (bout[idx + 2] >> 1));
            }
        }
        return bout;
    }
    public byte[] Decrypt(byte[] plaintext,byte[] symkey)
    {
        byte[] bout = new byte[plaintext.Length];
        int idx;
        for(idx = 0;idx < plaintext.Length; idx++)
            bout[idx] = plaintext[idx];
        //
        for(int keyOff = 12;keyOff >= 0;keyOff -= 4){
            for(idx = bout.Length - 4;idx >= 0; idx -= 4){
                bout[idx + 3] -= (byte)(bout[idx    ] ^ symkey[keyOff + 3] ^ (bout[idx + 2] >> 1));
                bout[idx + 2] -= (byte)(bout[idx + 3] ^ symkey[keyOff + 2] ^ (bout[idx + 1] >> 1));
                bout[idx + 1] -= (byte)(bout[idx + 2] ^ symkey[keyOff + 1] ^ (bout[idx    ] >> 1));
                bout[idx    ] ^= (byte)(bout[idx + 1] ^ symkey[keyOff    ] ^ (bout[idx + 3] >> 1));
            }
            for(idx = bout.Length - 1;idx >= 4;idx--)
                bout[idx] ^= (byte)((bout[idx - 2] + bout[idx - 3]) >> 1);
        }
        //
        return bout;
    }
    public void AddOutgoingPacket(Packet outp)
    {
        outgoingPackets.Add(outp);
    }
    public void FlushOutgoingPacket()
    {
        const int BufferSize = 4000;
        List<byte> outgoing = new List<byte>();
        outgoing.Add(0);
        outgoing.Add(0);
        outgoing.Add(0);
        outgoing.Add(0);
        while (true)
        {
            int off = 0;
            for(off = 0;off < 8;off++){
                Packet pack = outgoingPackets[off];
                byte[] buf = pack.PackPacket();
                if(outgoing.Count + buf.Length > BufferSize)
                    break;
                outgoingPackets.RemoveAt(off);
            }
            if(off == 8)
                break;
        }
        if (!isServer)
        {
            byte[] bufOut = outgoing.ToArray();
            byte[] enc = Encrypt(bufOut,endps.symkey);
            serverSocket.SendTo(enc,endps.point);
            return;
        }
    }
    public Packet PopIncommingPacket()
    {
        Packet p = incommingPackets[0];
        incommingPackets.RemoveAt(0);
        return p;
    }

    public void Handel()
    {
        if(isServer)HandelServer();
        else HandelClient();
    }
    internal void HandelServer()
    {
        serverSocket.Blocking = false;
        EndPoint remote = null;
        byte[] buffer = new byte[4096];
        while (serverSocket.Available > 0)
        {
            serverSocket.ReceiveFrom(buffer,ref remote);
            int idx = endps.Has(remote);
            if (idx < 0)
            {
                continue;
            }
            buffer = Decrypt(buffer,endps.pairs[idx].symkey);
            List<Packet> packets = getPackets(buffer);
            incommingPackets.AddRange(packets);
        }
    }
    internal void HandelClient()
    {
        
    }
}
