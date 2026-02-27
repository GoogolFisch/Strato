using System.Net.NetworkInformation;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BaseEntityPacket : Packet
{
    public long entId;
    public float eHealth;
    public int ePlayerOwner;
    public int eTick;

    public BaseEntityPacket() : base()
    {
        this.id = PacketTypes.BaseEntityPacket;
        entId = 0;
        eHealth = 0;
        ePlayerOwner = -1;
        eTick = 0;
    }
    public BaseEntityPacket(BaseEntity me) : base()
    {
        this.id = PacketTypes.BaseEntityPacket;
        entId = me.id;
        eHealth = me.health;
        ePlayerOwner = me.playerOwner;
        eTick = me.tick;
    }

    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(BitConverter.GetBytes(entId));
        outp.AddRange(BitConverter.GetBytes(eHealth));
        outp.AddRange(BitConverter.GetBytes(ePlayerOwner));
        outp.AddRange(BitConverter.GetBytes(eTick));
        outp.AddRange(base.PacketData());
        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int length)
    {
        BaseEntityPacket pingp = new BaseEntityPacket();
        if(length + index > message.Length)return null;
        pingp.PopulatePacket(ref index,message,length);
        return pingp;
    }
    override public void PopulatePacket(ref int index,byte[] message,int length) {
        base.PopulatePacket(ref index,message,length);
        entId = BitConverter.ToInt64(message,index);
        eHealth = BitConverter.ToSingle(message,index + 8);
        ePlayerOwner = BitConverter.ToInt32(message,index + 12);
        eTick = BitConverter.ToInt32(message,index + 20);
        index += 24;
    }

}
