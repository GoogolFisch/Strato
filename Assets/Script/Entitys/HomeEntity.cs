using UnityEngine;

public class HomeEntity : TowerEntity
{
    public MovingEntity lastCreate;
    public MovingEntity spawnEntity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    new void FixedUpdate(){
        base.FixedUpdate();
        if(tick < 51)return;
        tick = 0;
        MovingEntity me = Instantiate(spawnEntity,transform.position,
                Quaternion.identity,EntityManager.em.transform);
        me.followEnt = lastCreate;
        if(lastCreate != null && lastCreate.followEnt != null)
            me.followEnt = lastCreate.followEnt;
        lastCreate = me;
    }
}
