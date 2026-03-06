using System.Net.NetworkInformation;
using System;
using UnityEngine;
using System.Collections.Generic;

public class SetPlayerInfo : Packet
{
    public int team;
    public int other;
    public SetPlayerInfo() : base()
    {
        this.id = PacketTypes.SetPlayerInfo;
        team = 0;
        other = 0;
    }
    public SetPlayerInfo(PlayerController pc) : base()
    {
        this.id = PacketTypes.SetPlayerInfo;
        team = MemoryHandler.mh.shan.clCons.Count;
        other = 0;
    }
    
    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(base.PacketData());
        outp.AddRange(BitConverter.GetBytes(team));
        outp.AddRange(BitConverter.GetBytes(other));

        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int maxIdx)
    {
        SetPlayerInfo pack = new SetPlayerInfo();
        if(index > maxIdx)return null;
        pack.PopulatePacket(ref index,message,maxIdx);
        return pack;
    }

    override public void PopulatePacket(ref int index,byte[] message,int maxIdx) {
        base.PopulatePacket(ref index,message,maxIdx);
        team = BitConverter.ToInt32(message,index);
        other = BitConverter.ToInt32(message,index + 4);
        index += 8;
    }

    override public void ActUppon(){
        GameManager.gm.currentTeam = team;
    }
}
