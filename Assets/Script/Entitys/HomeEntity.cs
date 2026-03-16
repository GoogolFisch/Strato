using UnityEngine;

public class HomeEntity : BaseEntity
{
    public int tickDelta = 501;
    public MovingEntity lastCreate;
    public MovingEntity spawnEntity;
    public CarEntity carEnt;
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
        if(GameManager.gm.currentTeam == be.playerOwner){
            HomeEntity nHe = Instantiate(this,transform.position,
                        transform.rotation,transform.parent);
            nHe.baseHealth = baseHealth / 2;
            nHe.playerOwner = be.playerOwner;
            nHe.tickDelta = (int)(nHe.tickDelta * 1.5f);
            Debug.Log(nHe.playerOwner);
        }
        int oldOwner = playerOwner;
        //SetColor(GameManager.gm.GetMaterialFor(playerOwner));
        base.OnKill(be);
        TestSetLayer();
        Debug.Log("HomeEntity change player");
        EntityManager.em.HaveLost(oldOwner);
        EntityManager.em.HaveWon(be.playerOwner);
    }
    new public Packet SendStatus(){
        HomeEntityPacket mep = new HomeEntityPacket(this);
        return mep;
    }
}
