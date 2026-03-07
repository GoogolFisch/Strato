using UnityEngine;

public class HomeEntity : TowerEntity
{
    public BaseEntity lastCreate;
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
    }
}
