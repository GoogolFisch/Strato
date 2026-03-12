using System;
using UnityEngine;
using System.Collections.Generic;

public class AttackingPacket : Packet
{
    
    public ulong targetEnt;
    public ulong actingEnt;
    public float damage;
    public const float KILL_AMMOUNT = 99999;
    public AttackingPacket() : base()
    {
        this.id = PacketTypes.AttackingPacket;
        targetEnt = 0;
        actingEnt = 0;
        damage = 0;
    }
    public AttackingPacket(BaseEntity beAgressor,BaseEntity beVictim) : base()
    {
        this.id = PacketTypes.AttackingPacket;
        targetEnt = beVictim.id;
        actingEnt = beAgressor.id;
        damage = 1;
    }
    public AttackingPacket(BaseEntity beAgressor,BaseEntity beVictim,float damage) : base()
    {
        this.id = PacketTypes.AttackingPacket;
        targetEnt = beVictim.id;
        actingEnt = beAgressor.id;
        this.damage = damage;
    }
    override public List<byte> PacketData()
    {
        List<byte> outp = new List<byte>();
        outp.AddRange(base.PacketData());
        outp.AddRange(BitConverter.GetBytes(targetEnt));
        outp.AddRange(BitConverter.GetBytes(actingEnt));
        outp.AddRange(BitConverter.GetBytes(damage));

        return outp;
    }

    new public static Packet CreatePacket(int id,int index,byte[] message,int maxIdx)
    {
        AttackingPacket pack = new AttackingPacket();
        if(index > maxIdx)return null;
        pack.PopulatePacket(ref index,message,maxIdx);
        return pack;
    }

    override public void PopulatePacket(ref int index,byte[] message,int maxIdx) {
        base.PopulatePacket(ref index,message,maxIdx);
        targetEnt = BitConverter.ToUInt64(message,index);
        actingEnt = BitConverter.ToUInt64(message,index + 8);
        index += 16;
        damage = BitConverter.ToSingle(message,index);
        index += 4;

    }

    override public void ActUppon(){
        BaseEntity acEnt;
    BaseEntity trEnt;
        if(!EntityManager.em.enityList.ContainsKey(actingEnt)){
            Debug.Log("Argh! Attacker doesn't exist");
            acEnt = null;
        }else{
            acEnt = EntityManager.em.enityList[actingEnt];
        }
        if(!EntityManager.em.enityList.ContainsKey(targetEnt)){
            Debug.Log("Argh! Attacker is attacking nothing?");
            return;
        }
        trEnt = EntityManager.em.enityList[targetEnt];
        ActUppon(acEnt,trEnt);
    }
    public void ActUppon(BaseEntity acEnt,BaseEntity trEnt){
        trEnt.OnDamage(damage,acEnt);
        if(damage >= KILL_AMMOUNT){
            trEnt.OnKill(acEnt);
        }
    }
}
