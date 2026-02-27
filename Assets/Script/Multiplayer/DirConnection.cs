using System.Net;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Net.Sockets;

public class DirConnection
{
    public byte[] symkey;
    internal byte[] prevBytes;
    public Socket sock;
    public TcpClient tcpCl;
    public EndPoint point;
    public List<Packet> outgoingPackets;
    public List<Packet> incommingPackets;

    public DirConnection(Socket sock)
    {
        symkey = null;
        this.sock = sock;
        point = null;
        tcpCl = null;
        outgoingPackets = new List<Packet>();
        incommingPackets = new List<Packet>();
        prevBytes = new byte[0];
    }
    public DirConnection(TcpClient sock)
    {
        symkey = null;
        this.sock = sock.Client;
        point = null;
        tcpCl = sock;
        outgoingPackets = new List<Packet>();
        incommingPackets = new List<Packet>();
        prevBytes = new byte[0];
    }
    
    public List<Packet> getPackets(byte[] later)
    {
        List<byte> barr = new List<byte>();
        barr.AddRange(prevBytes);
        barr.AddRange(later);
        byte[] message = barr.ToArray();

        List<Packet> pack = new List<Packet>();
        //int fetchedTick = BitConverter.ToInt32(message,4);
        int index = 0;
        // TODO
        //int delta = lastTickRecv - fetchedTick;
        //if(delta > 0)lastTickRecv = fetchedTick;
        Packet addedPack = null;
        do{
            addedPack = Packet.ParsePacket(message,ref index);
            //if(delta > 0 || addedPack.keepIfOrderMism())
                pack.Add(addedPack);
        }while(addedPack != null);
        prevBytes = barr.GetRange(index,barr.Count - index).ToArray();


        return pack;
    }
    public byte[] Encrypt(byte[] plaintext,byte[] symkey)
    {
        if(plaintext.Length <= 3)return new byte[0];
        byte[] bout = new byte[((plaintext.Length - 1) | 0xf) + 1];
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
        byte[] bufOut = outgoing.ToArray();
        byte[] enc = Encrypt(bufOut,symkey);
        sock.Send(enc);
        return;
    }
    public Packet PopIncommingPacket()
    {
        Packet p = incommingPackets[0];
        incommingPackets.RemoveAt(0);
        return p;
    }
    public Packet GetIncommingPacket()
    {
        return incommingPackets[0];
    }

    public void Handel()
    {
        byte[] buffer = new byte[sock.Available];
        //EndPoint remote = null;
        while (sock.Available > 0)
        {
            sock.Receive(buffer);
            if(symkey != null)
                buffer = Decrypt(buffer,symkey);
            List<Packet> packets = getPackets(buffer);
            incommingPackets.AddRange(packets);
        }
    }
}
