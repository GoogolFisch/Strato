using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public int playerCount = 2;
    public float baseRadius = 2;
    public BaseEntity homeBase;
    public Dictionary<long, BaseEntity> enityList = new Dictionary<long, BaseEntity>();
    public static EntityManager em;
    void Awake()
    {
        em = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float twoPi = Mathf.PI * 2;
        for(int i = 0;i < playerCount; i++)
        {
            Vector3 vpos = new Vector3(Mathf.Sin(twoPi * i / playerCount),0,Mathf.Cos(twoPi * i / playerCount));
            vpos *= baseRadius;
            BaseEntity be = Instantiate(homeBase,vpos,Quaternion.identity,transform);
            be.playerOwner = i;
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BaseEntity selectedEnt;
    public SelectedObj insertWithSelected;
    public void DoSelectObject(BaseEntity gm)
    {
        if(selectedEnt == gm)return;
        if(selectedEnt != null)
        {
            //DestroyImmediate(SelectedObj.selObj);
            Destroy(SelectedObj.selObj.gameObject);
            selectedEnt = null;
        }
        if(gm != null)
        {
            selectedEnt = gm;
            Vector3 vpos = gm.transform.position + Vector3.up * 3;
            Instantiate(insertWithSelected,vpos,Quaternion.identity,gm.transform);
        }
        
    }
}
