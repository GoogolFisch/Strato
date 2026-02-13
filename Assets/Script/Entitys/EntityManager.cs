using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public int playerCount = 2;
    public float baseRadius = 2;
    public GameObject homeBase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float twoPi = Mathf.PI * 2;
        for(int i = 0;i < playerCount; i++)
        {
            Vector3 vpos = new Vector3(Mathf.Sin(twoPi * i / playerCount),0,Mathf.Cos(twoPi * i / playerCount));
            vpos *= baseRadius;
            Instantiate(homeBase,vpos,Quaternion.identity,transform);
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
