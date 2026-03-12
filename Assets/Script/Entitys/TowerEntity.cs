using System.Collections.Generic;
using UnityEngine;

public class TowerEntity : BaseEntity
{
    public BaseEntity targetEnt;

    public float radius = 1;
    public float attackDamage = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        transform.LookAt(Vector3.zero);
        base.Start();
        if(playerOwner == GameManager.gm.currentTeam)return;
        SummonEntityPacket mep = new SummonEntityPacket(this);
        MemoryHandler.mh.shan.AddPacket(mep);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    new internal void FixedUpdate()
    {
        base.FixedUpdate();
        if(tick < 5)
            return;
        tick = 0;
        List<BaseEntity> bes = EntityManager.em.getCircleEntity(
                                transform.position,radius);
        float minDist = 99;
        BaseEntity attacking = null;
        foreach(BaseEntity be in bes)
        {
            if(be == this)continue;
            if(be.playerOwner == playerOwner)continue;
            Vector3 dif = be.transform.position - transform.position;
            float dstSq = dif.x * dif.x + dif.y * dif.y + dif.z * dif.z;
            if(dstSq < minDist)
            {
                minDist = dstSq;
                attacking = be;
            }
        }
        if(attacking == null)return;
        Debug.Log($"attacking {attacking} at {minDist} : {attacking.health} - {attackDamage}");
        AttackingPacket atingP = new AttackingPacket(this,attacking,attackDamage);
        MemoryHandler.mh.shan.AddPacket(SendStatus());
        atingP.ActUppon(this,attacking);
        if(atingP != null)
            MemoryHandler.mh.shan.AddPacket(atingP);
    }
}
