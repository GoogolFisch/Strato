using UnityEngine;

public class BaseEntity : MonoBehaviour
{

    public ulong id = 0;
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
            id = (uint)Random.Range(0,0x10000);
            id <<= 16;
            id |= (uint)Random.Range(0,0x10000);
            id <<= 16;
            id |= (uint)Random.Range(0,0x10000);
            id <<= 16;
            id |= (uint)Random.Range(0,0x10000);
        }
        health = baseHealth;
    }
    internal void Start()
    {
        if(EntityManager.em.enityList.ContainsKey(id)){
            Destroy(gameObject);
            return;
        }
        EntityManager.em.enityList.Add(id,this);
        TestSetLayer();
        SetColor(GameManager.gm.GetMaterialFor(playerOwner));
    }
    internal void TestSetLayer(){
        int collideLayer;
        if(playerOwner == GameManager.gm.currentTeam)
        {
            collideLayer = LayerMask.NameToLayer("Selectable");
            moveToLayer(collideLayer);


            SummonEntityPacket mep = new SummonEntityPacket(this);
            if(MemoryHandler.mh != null)
                MemoryHandler.mh.shan.AddPacket(mep);
        }
        else if(playerOwner > 0)
        {
            collideLayer = LayerMask.NameToLayer("Attackable");
            moveToLayer(collideLayer);
        }
    }
    
    internal void SetColor(Material mat){
        // turn all of the children to be raycastable (2 layers)
        foreach (Transform child in gameObject.transform)
        {
            //child.gameObject.layer = collideLayer;
            foreach (Transform child2 in child)
            {
                child2.TryGetComponent(out MeshRenderer mr);
                if(mr != null){
                    mr.materials[0] = mat;
                    mr.material = mat;
                }
                child2.TryGetComponent(out SkinnedMeshRenderer smr);
                if(smr != null){
                    smr.materials[0] = mat;
                    smr.material = mat;
                }
            }
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
        health -= i;
        return health < 0;
    }

    public void OnDamage(float i,BaseEntity attackee){
        health -= i;
        if(health < 0){
            if(attackee != null){
                if(MemoryHandler.mh.shan.clCons != null && i < 9999){
                    AttackingPacket atp = new AttackingPacket(attackee,this,99999);
                    atp.ActUppon();
                    MemoryHandler.mh.shan.AddPacket(atp);
                }
            }
            else
                health += i;
        }
    }

    virtual public void OnKill(BaseEntity attackee){
        EntityManager.em.enityList.Remove(id);
        Destroy(gameObject);
    }

    // Update is called once per frame
    internal void Update()
    {
        
    }

    internal void FixedUpdate()
    {
        tick++;
        if(tick < 65536)
            return;
        tick = 0;
        MemoryHandler.mh.shan.AddPacket(SendStatus());
    }
    public Packet SendStatus(){
        BaseEntityPacket mep = new BaseEntityPacket(this);
        return mep;
    }
}
