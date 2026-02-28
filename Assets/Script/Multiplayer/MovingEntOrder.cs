using System.Net.NetworkInformation;
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
        targetPos = Vector3.zero;
        moveVector = Vector3.zero;
    }
    public MovingEntOrder(MovingEntity me) : base(me)
    {
        this.id = PacketTypes.MovingEntityPacket;
        followEnt = 0;
        attackingEnt = 0;
        targetPos = me.targetPos;
        moveVector = me.moveVector;
    }

    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(base.PacketData());
        outp.AddRange(BitConverter.GetBytes(followEnt));
        outp.AddRange(BitConverter.GetBytes(attackingEnt));

        outp.AddRange(BitConverter.GetBytes(targetPos.x));
        outp.AddRange(BitConverter.GetBytes(targetPos.y));
        outp.AddRange(BitConverter.GetBytes(targetPos.z));
        outp.AddRange(BitConverter.GetBytes(moveVector.x));
        outp.AddRange(BitConverter.GetBytes(moveVector.y));
        outp.AddRange(BitConverter.GetBytes(moveVector.z));

        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int maxIdx)
    {
        MovingEntOrder pack = new MovingEntOrder();
        if(index > maxIdx)return null;
        pack.PopulatePacket(ref index,message,maxIdx);
        return pack;
    }

    override public void PopulatePacket(ref int index,byte[] message,int maxIdx) {
        base.PopulatePacket(ref index,message,maxIdx);
        followEnt = BitConverter.ToInt64(message,index);
        attackingEnt = BitConverter.ToInt64(message,index + 8);
        index += 16;
        targetPos = new Vector3(BitConverter.ToSingle(message,index),
                        BitConverter.ToSingle(message,index + 4),
                        BitConverter.ToSingle(message,index + 8));
        index += 12;
        moveVector = new Vector3(BitConverter.ToSingle(message,index),
                        BitConverter.ToSingle(message,index + 4),
                        BitConverter.ToSingle(message,index + 8));
        index += 12;

    }

    override public void ActUppon(){
        //EntityManager.em.Summon(this);
    }
}
