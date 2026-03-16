using UnityEngine;
using System.Collections.Generic;

public class CarEntity : MovingEntity
{
    public bool isDead = true;
    new void Awake(){
        isDead = false;
        base.Awake();
    }
    override public AttackingPacket TryAttack(List<BaseEntity> bes){
        float minDist = 99;
        BaseEntity attacking = null;
        foreach(BaseEntity be in bes)
        {
            if(be == this)continue;
            //if(be.playerOwner == playerOwner)continue;
            if(!(be is Minable))continue;
            Vector3 dif = be.transform.position - transform.position;
            float dstSq = dif.x * dif.x + dif.y * dif.y + dif.z * dif.z;
            if(dstSq < minDist)
            {
                minDist = dstSq;
                attacking = be;
            }
        }
        if(attacking == null)return null;
        //Debug.Log($"attacking {attacking} at {minDist} : {attacking.health} - {attackDamage}");
        AttackingPacket atingP = new AttackingPacket(this,attacking,attackDamage);
        MemoryHandler.mh.shan.AddPacket(SendStatus());
        atingP.ActUppon(this,attacking);
        return atingP;
    }
    public void OnDestroy(){
        isDead = true;
    }
}
