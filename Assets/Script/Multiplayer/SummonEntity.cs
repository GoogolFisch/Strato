using System.Collections.Generic;
using System;
using UnityEngine;

public class SummonEntity : BaseEntityPacket
{
    public int ePlayerOwner;
    public SummonEntity() : base()
    {
        this.id = PacketTypes.BaseEntityPacket;
        entId = 0;
        eHealth = 0;
        ePlayerOwner = -1;
        eTick = 0;
    }
    public SummonEntity(BaseEntity me) : base(me)
    {
        this.id = PacketTypes.SummonEntityPacket;
        ePlayerOwner = me.playerOwner;
    }

    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(base.PacketData());
        outp.AddRange(BitConverter.GetBytes(ePlayerOwner));
        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int length)
    {
        SummonEntity pingp = new SummonEntity();
        if(length + index > message.Length)return null;
        pingp.PopulatePacket(ref index,message,length);
        return pingp;
    }
    override public void PopulatePacket(ref int index,byte[] message,int length) {
        base.PopulatePacket(ref index,message,length);
        ePlayerOwner = BitConverter.ToInt32(message,index);
        index += 4;
    }

    
}
