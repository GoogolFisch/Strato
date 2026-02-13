using UnityEngine;

public class BaseEntity : MonoBehaviour
{

    public long id = 0;
    public float health;
    public int playerOwner;
    public Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(id == 0)
        {
            id = Random.Range(0,0x10000) + (Random.Range(0,0x10000) <<16);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
