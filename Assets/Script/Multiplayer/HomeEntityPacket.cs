using UnityEngine;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System;

public class HomeEntityPacket : BaseEntityPacket
{
    public float bHealth;
    public int tickDelta;
    public HomeEntityPacket() : base()
    {
        this.id = PacketTypes.HomeEntityPacket;
        tickDelta = 501;
        bHealth = 0;
    }
    public HomeEntityPacket(HomeEntity he) : base(he)
    {
        this.id = PacketTypes.HomeEntityPacket;
        tickDelta = he.tickDelta;
        bHealth = he.baseHealth;
    }
    
    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(base.PacketData());
        outp.AddRange(BitConverter.GetBytes(tickDelta));
        outp.AddRange(BitConverter.GetBytes(bHealth));

        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int maxIdx)
    {
        HomeEntityPacket pack = new HomeEntityPacket();
        if(index > maxIdx)return null;
        pack.PopulatePacket(ref index,message,maxIdx);
        return pack;
    }

    override public void PopulatePacket(ref int index,byte[] message,int maxIdx) {
        base.PopulatePacket(ref index,message,maxIdx);
        tickDelta = BitConverter.ToInt32(message,index);
        bHealth = BitConverter.ToSingle(message,index + 4);
        index += 8;

    }

    override public void ActUppon(){
        if(!EntityManager.em.enityList.ContainsKey(entId)){
            return;
        }
        BaseEntity beI = EntityManager.em.enityList[entId];
        HomeEntity he = beI as HomeEntity;
        if(he == null)return;
        he.tickDelta = tickDelta;
        he.baseHealth = bHealth;
    }
}
