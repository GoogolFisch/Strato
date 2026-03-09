using UnityEngine;

public class HomeEntity : TowerEntity
{
    public int tickDelta = 501;
    public MovingEntity lastCreate;
    public MovingEntity spawnEntity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
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
    new void FixedUpdate(){
        base.FixedUpdate();
        if(tick < 501)return;
        tick = 0;
        if(MemoryHandler.mh.shan.clCons == null)return;
        MovingEntity me = Instantiate(spawnEntity,transform.position,
                Quaternion.identity,EntityManager.em.transform);
        me.followEnt = lastCreate;
        me.playerOwner = playerOwner;
        if(lastCreate != null && lastCreate.followEnt != null)
            me.followEnt = lastCreate.followEnt;
        lastCreate = me;
        if(playerOwner == GameManager.gm.currentTeam)return;
        SummonEntityPacket mep = new SummonEntityPacket(me);
        MemoryHandler.mh.shan.AddPacket(mep);
    }
}
