using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int currentTeam;
    public MemoryHandler memHand;
    public UIManager uiMan;
    public string roomName = "name";
    GroundState gs;
    void Awake(){
        gm = this;
        // ???
        //localConnector.StartBroadcasting(new byte[]{0x1f,0x1f},15);
    }
    void Start()
    {
        memHand = MemoryHandler.mh;
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
}
