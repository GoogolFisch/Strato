using UnityEngine;

public class Minable : BaseEntity
{
    new void Start()
    {
        playerOwner = -2;
        base.Start();
        SetColor(GameManager.gm.oreMaterial);
        if(0 != GameManager.gm.currentTeam)return;
        SummonEntityPacket mep = new SummonEntityPacket(this);
        MemoryHandler.mh.shan.AddPacket(mep);
    }

    override public void OnKill(BaseEntity be){
        if(GameManager.gm.currentTeam == be.playerOwner){
            BaseEntity nbe = Instantiate(EntityManager.em.homeBase,transform.position,
                        transform.rotation,transform.parent);
            nbe.playerOwner = be.playerOwner;
            nbe.baseHealth = EntityManager.em.homeBase.baseHealth * 0.9f;
            HomeEntity he = nbe as HomeEntity;
            if(he != null)
                he.tickDelta *= 2;
        }
        base.OnKill(be);
    }
}
