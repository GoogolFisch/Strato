
using System.Collections.Generic;
using System;

using UnityEngine;

public class ChatPacket : Packet
{
    public string playerName;
    public string playerMessage;
    public ChatPacket()
    {
        this.id = PacketTypes.ChatPacket;
    }

    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(Packet.ConvertFromString(playerName));
        outp.AddRange(Packet.ConvertFromString(playerMessage));
        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int maxIdx)
    {
        ChatPacket pingp = new ChatPacket();
        pingp.PopulatePacket(ref index,message,maxIdx);
        return pingp;
    }
    override public void PopulatePacket(ref int index,byte[] message,int maxIdx) {
        playerName = Packet.ConvertToString(message,ref index);
        playerMessage = Packet.ConvertToString(message,ref index);

    }

    override public void ActUppon(){
    }
}
