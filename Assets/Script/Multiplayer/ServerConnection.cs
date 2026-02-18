
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Data.Common;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Reflection;
using System.Reflection.Emit;

public class ServerConnection
{
    public bool isServer;
    byte[] symkey;
    int lastTickRecv = 0;
    public List<byte[]> fetchedInternet;

    public List<Packet> getPackets(byte[] message,int currentTick)
    {
        byte[] pdata = message;
        if(symkey != null && symkey.Length != 0)
        {
            pdata = Decrypt(message);
        }
        List<Packet> pack = new List<Packet>();
        int fetchedTick = BitConverter.ToInt32(message,4);
        int index = 8;


        int delta = lastTickRecv - fetchedTick;
        if(delta > 0)lastTickRecv = fetchedTick;
        return pack;
    }
    public byte[] Encrypt(byte[] plaintext)
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
    public byte[] Decrypt(byte[] plaintext)
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
}
