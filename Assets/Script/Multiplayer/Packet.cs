using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;


public class Packet
{
    public static List<Type> possible = new List<Type>();
    public int id;
    public Packet()
    {
        id = 0;
    }
    public Packet(int id)
    {
        this.id = id;
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
        bout.AddRange(BitConverter.GetBytes(id));
        bout.AddRange(BitConverter.GetBytes(messOut.Count));
        bout.AddRange(messOut);
        return bout.ToArray();
    }
    public List<byte> PacketData()
    {
        return new List<byte>();
    }

    public static Packet CreatePacket(int id,int index,byte[] message,int length)
    {
        return new Packet();
    }
}