using System.Net.NetworkInformation;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections.Generic;

public class MovingEntOrder : BaseEntityPacket
{
    public long followEnt;
    public long attackingEnt;
    public Vector3 currentPos;
    public Vector3 targetPos;
    public Vector3 moveVector;

    public MovingEntOrder() : base()
    {
        this.id = PacketTypes.MovingEntityPacket;
        followEnt = 0;
        attackingEnt = 0;
        currentPos = Vector3.zero;
        targetPos = Vector3.zero;
        moveVector = Vector3.zero;
    }
    public MovingEntOrder(MovingEntity me) : base(me)
    {
        this.id = PacketTypes.MovingEntityPacket;
        followEnt = 0;
        attackingEnt = 0;
        currentPos = me.transform.position;
        targetPos = me.targetPos;
        moveVector = me.moveVector;
    }

    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(base.PacketData());
        outp.AddRange(BitConverter.GetBytes(followEnt));
        outp.AddRange(BitConverter.GetBytes(attackingEnt));

        outp.AddRange(BitConverter.GetBytes(currentPos.x));
        outp.AddRange(BitConverter.GetBytes(currentPos.y));
        outp.AddRange(BitConverter.GetBytes(currentPos.z));
        outp.AddRange(BitConverter.GetBytes(targetPos.x));
        outp.AddRange(BitConverter.GetBytes(targetPos.y));
        outp.AddRange(BitConverter.GetBytes(targetPos.z));
        outp.AddRange(BitConverter.GetBytes(moveVector.x));
        outp.AddRange(BitConverter.GetBytes(moveVector.y));
        outp.AddRange(BitConverter.GetBytes(moveVector.z));

        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int length)
    {
        MovingEntOrder pack = new MovingEntOrder();
        if(length + index > message.Length)return null;
        pack.PopulatePacket(ref index,message,length);
        return pack;
    }

    override public void PopulatePacket(ref int index,byte[] message,int length) {
        base.PopulatePacket(ref index,message,length);
        followEnt = BitConverter.ToInt64(message,index);
        attackingEnt = BitConverter.ToInt64(message,index + 8);
        index += 16;
        currentPos = new Vector3(BitConverter.ToSingle(message,index),
                        BitConverter.ToSingle(message,index + 4),
                        BitConverter.ToSingle(message,index + 8));
        index += 12;
        targetPos = new Vector3(BitConverter.ToSingle(message,index),
                        BitConverter.ToSingle(message,index + 4),
                        BitConverter.ToSingle(message,index + 8));
        index += 12;
        moveVector = new Vector3(BitConverter.ToSingle(message,index),
                        BitConverter.ToSingle(message,index + 4),
                        BitConverter.ToSingle(message,index + 8));
        index += 12;

    }

}
