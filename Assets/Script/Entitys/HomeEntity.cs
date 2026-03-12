using UnityEngine;

public class HomeEntity : BaseEntity
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
        if(tick < tickDelta)return;
        tick = 0;
        if(MemoryHandler.mh.shan.clCons == null)return;
        MovingEntity me = Instantiate(spawnEntity,transform.position,
                Quaternion.identity,EntityManager.em.transform);
        me.followEnt = null; // server side advantage? //lastCreate;
        me.playerOwner = playerOwner;
        //if(lastCreate != null)
        //    me.followEnt = lastCreate;
        lastCreate = me;
        if(playerOwner == GameManager.gm.currentTeam)return;
        SummonEntityPacket mep = new SummonEntityPacket(me);
        MemoryHandler.mh.shan.AddPacket(mep);
    }
    override public void OnKill(BaseEntity be){
        health = baseHealth / 2;
        lastCreate = null;
        int oldOwner = playerOwner;
        playerOwner = be.playerOwner;
        SetColor(GameManager.gm.GetMaterialFor(playerOwner));
        TestSetLayer();
        Debug.Log("HomeEntity change player");
        EntityManager.em.HaveLost(oldOwner);
        EntityManager.em.HaveWon(be.playerOwner);
    }
}
