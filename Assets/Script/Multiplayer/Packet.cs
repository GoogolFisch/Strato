using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.Unicode;
using System.Threading.Channels;


public class Packet
{
    private static List<Type> SetupPackets()
    {
        List<Type> possible = new List<Type>(new Type[(int)PacketTypes.PackUp]);
        possible[(int)PacketTypes.PingPack] = PingPacket;
        return possible;
    }
    public static List<Type> possible = SetupPackets();
    
    public PacketTypes id;
    public Packet()
    {
        this.id = PacketTypes.PackNone;
    }
    public Packet(int id)
    {
        this.id = (PacketTypes)id;
    }

    public static Packet ParsePacket(byte[] message,ref int index)
    {
        int id = BitConverter.ToInt32(message,index);
        int length = BitConverter.ToInt32(message,index + 4);
        index += 8;
        if(possible.Count < id)return new Packet(id);
        if(id < 0)return new Packet(id);
        //Packet pout = (Packet)Activator.CreateInstance(possible[id],id,message,length);
        MethodInfo meth = possible[id].GetMethod("CreatePacket");
        Packet pout = (Packet)meth.Invoke(null,new object[]{id,index,message,length});
        index += length;
        return pout;
    }

    public byte[] PackPacket()
    {
        List<byte> messOut = PacketData();
        List<byte> bout = new List<byte>();
        bout.AddRange(BitConverter.GetBytes((int)id));
        bout.AddRange(BitConverter.GetBytes(messOut.Count));
        bout.AddRange(messOut);
        return bout.ToArray();
    }
    public static string ConvertToString(byte[] message,ref int index)
    {
        int length = BitConverter.GetBytes(message,index);
        string outp = new string(Encoding.UTF8.GetChars(message,index + 4,length));
        index += 4 + length;
        return outp;
    }
    public static List<byte> ConvertFromString(string str)
    {
        List<byte> bout = new List<byte>();
        byte[] utf8Out = Encoding.UTF8.GetBytes(str);
        bout.AddRange(BitConverter.GetBytes(utf8Out.Length));
        bout.Add(utf8Out);
        return bout;
    }
    // override these
    public List<byte> PacketData()
    {
        return new List<byte>();
    }

    public static Packet CreatePacket(int id,int index,byte[] message,int length)
    {
        return new Packet();
    }
}