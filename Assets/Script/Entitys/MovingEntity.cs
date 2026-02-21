using UnityEngine;

public class MovingEntity : BaseEntity
{
    public Vector3 targetPos;
    public BaseEntity followEnt;
    public BaseEntity targetEnt;

    public float speed = 1;
    public Vector3 moveVector;
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
        if(targetPos != null)
            moveVector += targetPos - transform.position;
        else if(followEnt != null)
            moveVector += followEnt.transform.position - transform.position;
        else if(targetEnt != null)
            moveVector += targetEnt.transform.position - transform.position;
        float mag = moveVector.magnitude;
        if(mag > 2)
            moveVector *= 1 / mag;
        else if(mag > 1)
            moveVector *= 1 / (mag - 1);
        else
            moveVector = Vector3.zero;

        if(mag > 1){
            transform.LookAt(moveVector + transform.position);
            transform.position += moveVector * speed * Time.deltaTime;
        }
        moveVector = Vector3.zero;
    }
    new internal void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
