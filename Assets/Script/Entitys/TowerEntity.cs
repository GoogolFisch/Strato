using System.Collections.Generic;
using UnityEngine;

public class TowerEntity : BaseEntity
{
    public BaseEntity targetEnt;
    public MovingEntity spawnEntity;

    public float radius = 1;

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
    new internal void FixedUpdate()
    {
        /*
        if (targetEnt != null && Vector3.Distance(transform.position, targetEnt.transform.position) < radius)
        {
            if (targetEnt.Damage(1))
            {
                targetEnt = null;
            }
        }
        if(tick < 4)
            return;
        tick = 0;
        List<BaseEntity> lbe = EntityManager.em.getCircleEntity(transform.position,radius);
        for(int i = 0;i < lbe.Count; i++)
        {
            if(lbe[i].playerOwner == playerOwner)continue;
            targetEnt = lbe[i];
            break;
        }
        //   */
        base.FixedUpdate();
        if(tick > 51)return;
        tick = 0;
        Instantiate(spawnEntity,transform.position,
                Quaternion.identity,EntityManager.em.transform);
    }
}
