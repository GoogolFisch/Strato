using UnityEngine;
using System.Collections.Generic;

public class HomeEntity : BaseEntity
{
    public int tickDelta = 501;
    public MovingEntity lastCreate;
    public MovingEntity spawnEntity;
    public CarEntity carEnt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {

        bool doSkip = EntityManager.em.enityList.ContainsKey(id);
        transform.LookAt(Vector3.zero);
        base.Start();
        Debug.Log(id);
        if(doSkip)return;
        /*if(playerOwner == GameManager.gm.currentTeam)return;
        SummonEntityPacket mep = new SummonEntityPacket(this);
        MemoryHandler.mh.shan.AddPacket(mep);*/
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    bool SummonCar(){

        if(carEnt != null){
            /* if (carEnt is not UnityEngine.Object){
                Debug.Log("Argh!");
                return false;
            } */
            if(!carEnt.isDead)return false;
        }
        
        Debug.Log("Hello trying to spawn stuff here?");
        if(Random.Range(0,tickDelta) > 250)
            return false;
        carEnt = Instantiate(EntityManager.em.carEntity,transform.position,
                transform.rotation,transform.parent);
        carEnt.playerOwner = playerOwner;
        if(playerOwner == GameManager.gm.currentTeam)return true;
        SummonEntityPacket mep = new SummonEntityPacket(carEnt);
        MemoryHandler.mh.shan.AddPacket(mep);
        return true;
    }
    new void FixedUpdate(){
        if(health < -20){
            EntityManager.em.enityList.Remove(id);
            Debug.Log("Destroy cause of health");
            Destroy(gameObject);
            return;
        } // */
        base.FixedUpdate();
        if(tick < tickDelta)return;
        tick = 0;
        if(MemoryHandler.mh.shan.clCons == null)return;
        if(SummonCar())return;
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
        int oldOwner = playerOwner;
        if(GameManager.gm.currentTeam == be.playerOwner){
            BaseEntity nBe = Instantiate(EntityManager.em.homeBase,transform.position,
                        transform.rotation,transform.parent);
            HomeEntity nHe = nBe as HomeEntity;
            if(nHe == null)return; // should never run!
            nHe.baseHealth = baseHealth / 2;
            nHe.playerOwner = be.playerOwner;
            nHe.tickDelta = (int)(nHe.tickDelta * 1.5f);
            Debug.Log(nHe.playerOwner);
        }
        //SetColor(GameManager.gm.GetMaterialFor(playerOwner));
        base.OnKill(be);
        TestSetLayer();
        Debug.Log("HomeEntity change player");
        EntityManager.em.HaveLost(oldOwner);
        EntityManager.em.HaveWon(be.playerOwner);

        EntityManager.em.enityList.Remove(id);
        Destroy(gameObject);
    }
    new public Packet SendStatus(){
        HomeEntityPacket mep = new HomeEntityPacket(this);
        return mep;
    }
}
