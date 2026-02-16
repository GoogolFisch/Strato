using UnityEngine;

public class MovingEntity : BaseEntity
{
    public Vector3 targetPos;
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
        Debug.Log(targetPos);
        Debug.Log(targetEnt);
        if(targetPos != null)
            moveVector += targetPos - transform.position;
        if(targetEnt != null)
            moveVector += targetEnt.transform.position - transform.position;
        moveVector.Normalize();

        

        transform.LookAt(moveVector + transform.position);
        transform.position += moveVector * speed * Time.deltaTime;
        moveVector = Vector3.zero;
    }
}
