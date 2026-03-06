using UnityEngine;

public class HomeEntity : TowerEntity
{
    public BaseEntity lastCreate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    void FixedUpdate(){
        base.FixedUpdate();
    }
}
