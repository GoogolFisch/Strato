using System.Collections.Generic;
using System;
using UnityEngine;

public class SummonEntityPacket : BaseEntityPacket
{
    public int ePlayerOwner;
    public int entityType;
    public Packet subPack;
    public SummonEntityPacket() : base()
    {
        this.id = PacketTypes.BaseEntityPacket;
        ePlayerOwner = -1;
        entityType = -1;
        subPack = null;
    }
    public SummonEntityPacket(BaseEntity me) : base(me)
    {
        BaseEntity[] arr = EntityManager.em.allEntitys;
        entityType = -1;
        for(int idx = 0;idx < arr.Length;idx++){
            if(arr[idx].GetType() != me.GetType())continue;
            entityType = idx;
            break;
        }
        this.id = PacketTypes.SummonEntityPacket;
        ePlayerOwner = me.playerOwner;
        if(me.GetType() != typeof(BaseEntity))
            subPack = me.SendStatus();
    }

    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(base.PacketData());
        outp.AddRange(BitConverter.GetBytes(ePlayerOwner));
        outp.AddRange(BitConverter.GetBytes(entityType));
        if(subPack != null)
            outp.AddRange(subPack.PackPacket());
        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int maxIdx)
    {
        SummonEntityPacket pingp = new SummonEntityPacket();
        if(maxIdx > index)return null;
        pingp.PopulatePacket(ref index,message,maxIdx);
        return pingp;
    }
    override public void PopulatePacket(ref int index,byte[] message,int maxIdx) {
        base.PopulatePacket(ref index,message,maxIdx);
        ePlayerOwner = BitConverter.ToInt32(message,index);
        entityType = BitConverter.ToInt32(message,index + 4);
        index += 8;
        if(maxIdx <= index)return;
        subPack = Packet.ParsePacket(message,ref index);
    }

    override public void ActUppon(){
        EntityManager.em.Summon(this);
    }
}
