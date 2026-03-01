using UnityEngine;
using System.Collections.Generic;


public class PlayerJoinPacket : Packet
{
    public string playerName;
    public PlayerJoinPacket()
    {
        this.id = PacketTypes.ChatPacket;
    }

    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(Packet.ConvertFromString(playerName));
        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int maxIdx)
    {
        PlayerJoinPacket pingp = new PlayerJoinPacket();
        pingp.PopulatePacket(ref index,message,maxIdx);
        return pingp;
    }
    override public void PopulatePacket(ref int index,byte[] message,int maxIdx) {
        playerName = Packet.ConvertToString(message,ref index);

    }

    override public void ActUppon(){
        DeLogger.dl.Log(playerName);
    }
    
}
