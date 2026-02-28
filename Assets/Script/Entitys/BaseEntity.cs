using UnityEngine;

public class BaseEntity : MonoBehaviour
{

    public long id = 0;
    public float health;
    public float baseHealth;
    public int playerOwner;
    public int tick;
    //public Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(id == 0)
        {
            id = Random.Range(0,0x10000) + (Random.Range(0,0x10000) <<16);
        }
    }
    internal void Start()
    {
        EntityManager.em.enityList.Add(id,this);
        int collideLayer;
        if(playerOwner == GameManager.gm.currentTeam)
        {
            collideLayer = LayerMask.NameToLayer("Selectable");
            moveToLayer(collideLayer);


            SummonEntityPacket mep = new SummonEntityPacket(this);
            MemoryHandler.mh.shan.AddPacket(mep);
        }
        else if(playerOwner > 0)
        {
            collideLayer = LayerMask.NameToLayer("Attackable");
            moveToLayer(collideLayer);
        }
    }
    void moveToLayer(int collideLayer)
    {
        if(collideLayer < 0 || collideLayer > 31){
            Debug.Log("layer errror!");
            return;
        }
        gameObject.layer = collideLayer;
        // turn all of the children to be raycastable (2 layers)
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.layer = collideLayer;
            foreach (Transform child2 in child)
            {
                child2.gameObject.layer = collideLayer;
            }
        }
    }
    public bool Damage(float i)
    {
        baseHealth -= i;
        return baseHealth < 0;
    }

    // Update is called once per frame
    internal void Update()
    {
        
    }

    internal void FixedUpdate()
    {
        tick++;
        if(tick < 128)
            return;
        tick = 0;
        MemoryHandler.mh.shan.AddPacket(SendStatus());
    }
    public Packet SendStatus(){
        BaseEntityPacket mep = new BaseEntityPacket(this);
        return mep;
    }
}
