using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Data.Common;


public class Packet
{
    private static List<Func<int,int,byte[],int,Packet>> SetupPackets()
    {
        List<Func<int,int,byte[],int,Packet>> possible = new List<Func<int,int,byte[],int,Packet>>();
        for(int idx = 0;idx < (int)PacketTypes.PackUp;idx++)
            possible.Add(null);
        possible[(int)PacketTypes.PingPack] = PingPacket.CreatePacket;
        possible[(int)PacketTypes.ChatPacket] = ChatPacket.CreatePacket;
        return possible;
    }
    public static List<Func<int,int,byte[],int,Packet>> possible = SetupPackets();
    //public static List<Action> assdd = SetupPackets();
    
    public PacketTypes id;
    public Packet()
    {
        this.id = PacketTypes.PackNone;
    }
    public Packet(int id)
    {
        this.id = (PacketTypes)id;
    }
    public static string ConvertToString(byte[] message,ref int index)
    {
        if(message.Length - index < 4)return null;
        int length = BitConverter.ToInt32(message,index);
        if(message.Length - index - 4 < length)return null;
        string outp = new string(Encoding.UTF8.GetChars(message,index + 4,length));
        index += 4 + length;
        return outp;
    }
    public static List<byte> ConvertFromString(string str)
    {
        List<byte> bout = new List<byte>();
        byte[] utf8Out = Encoding.UTF8.GetBytes(str);
        bout.AddRange(BitConverter.GetBytes(utf8Out.Length));
        bout.AddRange(utf8Out);
        return bout;
    }
    public bool keepIfOrderMism(){return false;}

    public static Packet ParsePacket(byte[] message,ref int index)
    {
        if(index + 8 < message.Length)return null;
        int length = BitConverter.ToInt32(message,index);
        int id = BitConverter.ToInt32(message,index + 4);
        if(length + index >= message.Length)return null;
        index += 8;
        if(possible.Count < id)return new Packet(id);
        if(id < 0)return new Packet(id);
        //
        if(id == 0 && length == 0)return null;
        //Packet pout = (Packet)Activator.CreateInstance(possible[id],id,message,length);
        Packet pout = (Packet)possible[id](id,index,message,length);
        index += length;
        return pout;
    }

    public byte[] PackPacket()
    {
        List<byte> messOut = PacketData();
        List<byte> bout = new List<byte>();
        bout.AddRange(BitConverter.GetBytes(messOut.Count));
        bout.AddRange(BitConverter.GetBytes((int)id));
        bout.AddRange(messOut);
        return bout.ToArray();
    }
    public static Packet CreatePacket(int id,int index,byte[] message,int length)
    {
        return new Packet();
    }
    // override these
    virtual public List<byte> PacketData()
    {
        return new List<byte>();
    }

    virtual public void PopulatePacket(ref int index,byte[] message,int length) {

    }
}