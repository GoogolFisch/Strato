using UnityEngine;
using System.Collections.Generic;

public class MovingEntity : BaseEntity
{
    public Vector3 targetPos;
    public BaseEntity followEnt;
    public BaseEntity targetEnt;

    public float speed = 1;
    public float colRadius = 1;
    public Vector3 moveVector;
    public Vector3 redirectVel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        moveVector = Vector3.zero;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        float mag = moveVector.magnitude;
        Vector3 moving = moveVector + redirectVel;

        if(mag > 1)
            moving /= mag;
        if(mag > 0.9){
            if(mag > 0.8)
                transform.LookAt(moveVector + transform.position);
            transform.position += moving * speed * Time.deltaTime;
        }
    }
    new internal void FixedUpdate()
    {
        moveVector = Vector3.zero;
        base.FixedUpdate();
        float mag;
        float colMag, strength;
        Vector3 away;
        // TODO make it attack when enemys are nearby
        // this would remove the wired ending behaviour
        if(followEnt != null)
        {
            if(followEnt.GetType() == typeof(MovingEntity)){
                Vector3 preMove = ((MovingEntity)followEnt).moveVector;
                preMove.Normalize();
                moveVector += preMove;
            }

            away = transform.position - followEnt.transform.position;
            mag = away.magnitude;
            colMag = mag / colRadius;
            strength = 2 / (colMag * colMag + 1) - 1;
            if(mag > 0.01){
                moveVector += strength * away / mag;
            }
        }
        else if(targetPos != null)
            moveVector += targetPos - transform.position;
        else if(targetEnt != null)
            moveVector += targetEnt.transform.position - transform.position;
        mag = moveVector.magnitude;
        //if(mag > 0.5)
        //    moveVector += redirectVel;
        if(mag < 0.2){
            moveVector = Vector3.zero;
            // TODO add message, that it should stop?
        }

        if(tick < 6)
            return;
        tick = 0;
        List<BaseEntity> bes = EntityManager.em.getCircleEntity(
                                transform.position,colRadius);
        redirectVel = Vector3.zero;
        foreach(BaseEntity be in bes)
        {
            if(be == this)continue;
            away = transform.position - be.transform.position;
            mag = away.magnitude;
            colMag = mag / colRadius;
            strength = 2 / (colMag * colMag + 1) - 1;
            if(mag > 0.01){
                redirectVel += strength * away / mag;
            }
        }
        redirectVel.Normalize();
        transform.position = new Vector3(transform.position.x,0,transform.position.z);
        MemoryHandler.mh.shan.AddPacket(SendStatus());
    }
    public void FollowEnt(BaseEntity be)
    {
        followEnt = be;
        if(be.GetType() == typeof(MovingEntity))
        {
            MovingEntity ment = (MovingEntity)be;
            if(ment.followEnt != null)
                followEnt = ment.followEnt;
        }
    }
    public void FollowPos(Vector3 pos)
    {
        followEnt = null;
        targetPos = pos;
    }
    new public Packet SendStatus(){
        MovingEntOrder mep = new MovingEntOrder(this);
        return mep;
    }
}
