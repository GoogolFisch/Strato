using UnityEngine;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MemoryHandler : MonoBehaviour
{
    public static MemoryHandler mh;
    bool created = false;
    public bool hasListeners = false;
    public bool hasSenders = false;
    public LanConnector udpLan;
    public ServerHandler shan;

    public const string scMenu = "MainMenu";
    public const string scGame = "GameScene";


    Task<(IPEndPoint,byte[])> udpListenerTask;
    Task broadcastSender;
    private void Awake()
    {
        if(mh != null){
            Destroy(this.gameObject);
            return;
        }
        mh = this;
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate(){
        mh = this;
        if(shan == null)
            return;
        shan.Handel();
    }

    // Don't call this often
    public (IPEndPoint,byte[]) GetLocalGames()
    {
        hasListeners = true;
        if(udpLan == null)
            udpLan = new LanConnector();
        if(udpListenerTask == null)
            udpListenerTask = udpLan.ListenForConnection();
        if(udpListenerTask.Status == TaskStatus.RanToCompletion){
            var res = udpListenerTask.Result;
            udpListenerTask = null;
            hasListeners = false;
            return res;
        }
        return (null,null);
    }

    public void StartBoradCasting(string roomName){
        const float BOARDCAST_DELTA = 15f;
        if(hasSenders)return;
        if(broadcastSender != null)return;
            //broadcastSender = 
            udpLan.StartBroadcasting(
                LanConnector.MakeSessionInfo(roomName),BOARDCAST_DELTA);
        hasSenders = true;
    }
    public void StopBoradCasting(){
        if(broadcastSender == null)return;
        udpLan.StopBroadcasting();
        broadcastSender = null;
        hasSenders = false;
    }

    void OnDestroy()
    {
        /*
        if(udpListenerTask != null)
            udpListenerTask.Dispose(); // */
        if(broadcastSender != null)
            broadcastSender.Dispose();
        if(udpLan != null)
            udpLan.Dispose();
        if(!created)return;
    }

    // true on success
    public bool ConnectToServer(IPEndPoint ipe){
        TcpClient tcl = new TcpClient();
        try{
            tcl.Connect(ipe);
        }catch(SocketException e){
            Debug.Log(e.ErrorCode);
            tcl.Dispose();
            return false;
        }
        shan = new ServerHandler(tcl);
        return true;
    }
    // true on success
    public bool HostGame(IPEndPoint ipe){
        TcpListener tcl = new TcpListener(ipe);
        try{
            tcl.Start();
        }catch(SocketException e){
            Debug.Log(e.ErrorCode);
            //tcl.Dispose();
            return false;
        }
        shan = new ServerHandler(tcl);
        return true;
    }

    public void SetActiveScene(string sc){
        //Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sc);
        //var newScene = SceneManager.CreateScene("GameScene",LoadSceneMode.Single);
        /*SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.OpenScene("GameScene", LoadSceneMode.Additive); //  */
        //SceneManager.OpenScene("GameScene", LoadSceneMode.Single);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sc));
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sc));
        //SceneManager.UnloadSceneAsync(currentScene);

        //Debug.Log(sc);
    }
}
