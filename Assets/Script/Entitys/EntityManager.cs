using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public float baseRadius = 2;
    public BaseEntity homeBase;
    public BaseEntity turretEntity;
    public Dictionary<ulong, BaseEntity> enityList = new Dictionary<ulong, BaseEntity>();
    public static EntityManager em;
    public bool HasStarted = false;


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
        if(HasStarted)return;
        HasStarted = true;
        float twoPi = Mathf.PI * 2;
        int playerCount = MemoryHandler.mh.maxPlCnt + 1;
        for(int i = 0;i < playerCount; i++)
        {
            int team;
            if(i == 0)team = 0;
            else team = MemoryHandler.mh.shan.clCons[i - 1].gameTeam;
            Vector3 vpos = new Vector3(Mathf.Sin(twoPi * i / playerCount),
                        0,Mathf.Cos(twoPi * i / playerCount));
            vpos *= baseRadius;
            BaseEntity be = Instantiate(homeBase,vpos,Quaternion.identity,transform);
            be.playerOwner = team;
            be = Instantiate(turretEntity,vpos * 0.9f,Quaternion.identity,transform);
            be.playerOwner = team;
        }
    }
    public void Summon(SummonEntityPacket sep){
        //Debug.Log($"type:{sep.entityType}");
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
    public void HaveLost(int testLoser){
        if(testLoser != GameManager.gm.currentTeam)return;
        // heavy function
        foreach(ulong i in enityList.Keys)
        {
            BaseEntity be = enityList[i];
            HomeEntity he = be as HomeEntity;
            if(he == null)continue;
            if(he.playerOwner == testLoser)return;
        }
        // kill all of them!
        List<BaseEntity> toRemove = new();
        foreach(ulong i in enityList.Keys)
        {
            BaseEntity be = enityList[i];
            if(be.playerOwner != testLoser)continue;
            toRemove.Add(be);
        }
        foreach(BaseEntity be in toRemove){
            AttackingPacket attP = new AttackingPacket(be,be,AttackingPacket.KILL_AMMOUNT);
            attP.ActUppon();
            MemoryHandler.mh.shan.AddPacket(attP);
        }
        //
        FinishMenu.fm.SetWin("You lose!");
    }
    public void HaveWon(int testLoser){
        if(testLoser != GameManager.gm.currentTeam)return;
        // heavy function
        foreach(ulong i in enityList.Keys)
        {
            BaseEntity be = enityList[i];
            HomeEntity he = be as HomeEntity;
            if(he == null)continue;
            if(he.playerOwner != testLoser)return;
        }
        FinishMenu.fm.SetWin("You won!");
        FinishMenu.fm.AllowExit();
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
            if(gm == null)
                selectedEnt = null;
        }
        if(gm != null)
        {
            if((gm as MovingEntity) != null){
                if(selectedEnt == ((MovingEntity)gm).followEnt){
                    selectedEnt = gm;
                }else{
                    selectedEnt = ((MovingEntity)gm).followEnt;
                }
                if(selectedEnt == null){
                    selectedEnt = gm;
                }
            }else{
                selectedEnt = gm;
            }
            Vector3 vpos = selectedEnt.transform.position + Vector3.up * 3;
            Instantiate(insertWithSelected,vpos,Quaternion.identity,selectedEnt.transform);
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
        if(selectedEnt == null){
            Debug.Log("Make Obj?");
            //BaseEntity be = Instantiate(allEntitys[0],hit,Quaternion.identity,transform);
            //be.playerOwner = GameManager.gm.currentTeam;
            return;
        }
        if(hit.magnitude > baseRadius * 1.5f)return;
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
        foreach(ulong i in enityList.Keys)
        {
            BaseEntity be = enityList[i];
            float dist = Vector3.Distance(pos,be.transform.position);
            if(dist / 2 > radius)continue;
            bent.Add(be);
        }
        return bent;
    }
}
