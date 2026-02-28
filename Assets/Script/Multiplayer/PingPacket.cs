using System.Net.NetworkInformation;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PingPacket : Packet
{
    public int lastTick;
    public List<Tuple<string,float>> pingOf;
    public PingPacket()
    {
        this.id = PacketTypes.PingPack;
        lastTick = 0;
        pingOf = new List<Tuple<string, float>>();
    }

    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();

        outp.AddRange(BitConverter.GetBytes(this.lastTick));
        outp.AddRange(BitConverter.GetBytes(this.pingOf.Count));
        for(int idx = 0;idx < pingOf.Count; idx++)
        {
            outp.AddRange(Packet.ConvertFromString(pingOf[idx].Item1));
            outp.AddRange(BitConverter.GetBytes(pingOf[idx].Item2));
        }
        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int maxIdx)
    {
        PingPacket pingp = new PingPacket();
        return pingp;
    }
    override public void PopulatePacket(ref int index,byte[] message,int maxIdx) {
        lastTick = BitConverter.ToInt32(message,index);
        int counting = 0;
        counting = BitConverter.ToInt32(message,index + 4);
        index += 8;
        for(int idx = 0;idx < counting && maxIdx > idx + index; idx++)
        {
            pingOf.Add(new Tuple<string, float>(
                Packet.ConvertToString(message,ref index),
                BitConverter.ToSingle(message,index)
                ));
            index += 4;
        }
    }

    override public void ActUppon(){
    }
}
