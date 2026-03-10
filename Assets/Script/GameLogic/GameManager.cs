using UnityEngine;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int currentTeam;
    public MemoryHandler memHand;
    public UIManager uiMan;
    public string playerName = "name";
    public Dictionary<int,Material> mats = new Dictionary<int,Material>();
    public Material onesMaterial;
    public Shader bluePrintShader;
    GroundState gs;
    void Awake(){
        gm = this;
        // ???
        //localConnector.StartBroadcasting(new byte[]{0x1f,0x1f},15);
    }
    void Start()
    {
        memHand = MemoryHandler.mh;
        playerName = memHand.plName;
        memHand.shan.AddPacket(new ChatPacket(""));
        //mats.Add(0,onesMaterial);
    }
    
    public void RegisterGroundService(GroundState gs){
        this.gs = gs;
    }

    public void Update()
    {
    }
    public void FixedUpdate()
    {
        gm = this;
        memHand = MemoryHandler.mh;
        //mh.Handel();
    }
    public Material GetMaterialFor(int playerTeam){
        if(playerTeam == currentTeam)return onesMaterial;
        if(mats.ContainsKey(playerTeam)){
            return mats[playerTeam];
        }
        Debug.Log($"Create Material for :{playerTeam}");
        Material m = new Material(bluePrintShader);
        m.SetColor("Base Color", new Color(0,1,0));
        m.SetColor("_BaseColor", new Color(0,1,0));
        m.SetColor("Color", new Color(0,1,0));
        mats.Add(playerTeam,m);
        return m;
    }
}
