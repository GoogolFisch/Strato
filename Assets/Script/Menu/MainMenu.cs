using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Globalization;

public class MainMenu : MonoBehaviour
{
    public static MainMenu mm;
    public InputField insertName;
    public InputField insertIp;
    public InputField insertPort;
    public InputField insertPlayerCount;
    public Text ipErrorText;
    void Awake(){
        mm = this;
        string[] baseName = {
            "Luminous", "Obsidian", "Ephemeral",
            "Zenith",   "Labyrinth","Serendipity",
            "Euphoria", "Solstice", "Nebula",
            "Cascade",  "Paradox",  "Aurora",
            "Mirage",   "Equinox",  "Resonance",
            "Quasar",   "Ethereal", "Vortex",
            "Glyph",    "Saffron",  "Halcyon",
            "Eclipse",  "Catalyst", "Nocturne",
            "Prism",    "Tundra",   "Mosaic",
            "Horizon",  "Ember",    "Cascade"
        };
        string nameNow = baseName[Random.Range(0,baseName.Length)];
        nameNow += $"{Random.Range(0,100)}";
        insertName.text = nameNow;
        insertPort.text = $"{LanConnector.PORT}";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 60;
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate(){
        mm = this;
    }

    public void SetIp(IPEndPoint ep){
        insertIp.text = $"{ep.Address}";
        insertPort.text = $"{LanConnector.PORT}";
    }

    public void HostGame(int port){
        bool tstCnn = MemoryHandler.mh.HostGame(
                new IPEndPoint(IPAddress.Any,port));
        if(!tstCnn)return;
        if(!int.TryParse(insertPlayerCount.text, NumberStyles.None, NumberFormatInfo.CurrentInfo, out MemoryHandler.mh.maxPlCnt)){
            //MemoryHandler.mh.maxPlCnt
        }
        MemoryHandler.mh.SetActiveScene(MemoryHandler.scGame);
        MemoryHandler.mh.StartBoradCasting("Hello");
    }
    public void JoinGame(){
        int port;
        MemoryHandler.mh.plName = insertName.text;
        if (!int.TryParse(insertPort.text, NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
        {
            ipErrorText.text = "Invalid port";
            return;
        }
        if(insertIp.text.Equals("")){
            HostGame(port);
            return;
        }
        //MemoryHandler.mh.maxPlCnt = insertPlayerCount.text;
        IPEndPoint ipe;
        IPAddress ip;
        if (!IPAddress.TryParse(insertIp.text, out ip)){
            ipErrorText.text = "Invalid ip-adress";
            return;
        }
        ipe = new IPEndPoint(ip,port);
        ipErrorText.text = "";
        bool tstCnn = MemoryHandler.mh.ConnectToServer(ipe);
        if(!tstCnn){
            return;
        }
        MemoryHandler.mh.SetActiveScene(MemoryHandler.scGame);
    }
}
