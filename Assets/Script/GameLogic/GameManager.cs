using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int currentTeam;
    public LanConnector localConnector;
    public ServerHandler servHandler;
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
        localConnector = new LanConnector();
        //servHandler = new ServerHandler();
        localConnector.StartBroadcasting(
                LanConnector.MakeSessionInfo(roomName),15);
    }
    
    public void RegisterGroundService(GroundState gs){
        this.gs = gs;
    }

    public void Update()
    {
        gm = this;
    }
    public void FixedUpdate()
    {
    }
}
