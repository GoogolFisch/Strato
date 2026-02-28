using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public int playerCount = 2;
    public float baseRadius = 2;
    public BaseEntity homeBase;
    public Dictionary<long, BaseEntity> enityList = new Dictionary<long, BaseEntity>();
    public static EntityManager em;


    [Header("Entity List")]
    public BaseEntity[] allEntitys;
    void Awake()
    {
        em = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SummonHomes(){
        float twoPi = Mathf.PI * 2;
        for(int i = 0;i < playerCount; i++)
        {
            Vector3 vpos = new Vector3(Mathf.Sin(twoPi * i / playerCount),0,Mathf.Cos(twoPi * i / playerCount));
            vpos *= baseRadius;
            BaseEntity be = Instantiate(homeBase,vpos,Quaternion.identity,transform);
            be.playerOwner = i;
        }
    }
    public void Summon(SummonEntityPacket sep){
        BaseEntity be = Instantiate(allEntitys[sep.entityType],
                    sep.position,Quaternion.identity,transform);
        be.id = sep.entId;
        be.playerOwner = sep.ePlayerOwner;
        be.health = sep.eHealth;
        
        // typeof(Derived).IsSubclassOf(typeof(SomeType))
        // typeof(SomeType).IsAssignableFrom(typeof(Derived))
        if(sep.subPack.GetType().IsSubclassOf(typeof(BaseEntity))){
            HandelPacket(sep);
        }
    }
    public void HandelPacket(BaseEntityPacket bep){
        Debug.Log("{2026-02-28T14:29:00}");
    }
    public void HandelPacket(MovingEntOrder meo){
    }

    // Update is called once per frame
    void Update()
    {
        em = this;
    }

    public BaseEntity selectedEnt;
    public SelectedObj insertWithSelected;

    public bool DoSelectScreenSpace(Vector2 pos)
    {
        Vector3 uiPos = RectTransformUtility.WorldToScreenPoint(Camera.main, Vector3.zero);
        Debug.Log(uiPos);
        Debug.Log(pos);
        return true;
    }
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
    public void DoSelectObject2(BaseEntity gm)
    {
        if(selectedEnt == gm)return;
        if(gm == null)return;
        if(selectedEnt.GetType() == typeof(MovingEntity))
        {
            MovingEntity ment = (MovingEntity)selectedEnt;
            ment.FollowPos(gm.transform.position);
        }
        /*
        if(selectedEnt.GetType() == typeof(TowerEntity))
        {
            TowerEntity ment = (TowerEntity)selectedEnt;
            ment.targetEnt = gm;
        }
        // */
    }
    public void DoSelectObject3(BaseEntity gm)
    {
        if(selectedEnt == gm)return;
        if(gm == null)
            return;
        if(selectedEnt.GetType() == typeof(MovingEntity))
        {
            MovingEntity ment = (MovingEntity)selectedEnt;
            ment.FollowEnt(gm);
        }
    }
    public void DoSelectGround(Vector3 hit)
    {
        if(selectedEnt == null)return;
        if(selectedEnt.GetType() == typeof(MovingEntity))
        {
            MovingEntity ment = (MovingEntity)selectedEnt;
            ment.FollowPos(hit);
        }
    }
    public List<BaseEntity> getCircleEntity(Vector3 pos,float radius)
    {
        // TODO make faster
        List<BaseEntity> bent = new List<BaseEntity>();
        foreach(long i in enityList.Keys)
        {
            BaseEntity be = enityList[i];
            float dist = Vector3.Distance(pos,be.transform.position);
            if(dist / 2 > radius)continue;
            bent.Add(be);
        }
        return bent;
    }
}
