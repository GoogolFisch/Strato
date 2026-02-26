using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class MainMenu : MonoBehaviour
{
    public static MainMenu mm;
    public InputField insertName;
    public InputField insertIp;
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
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate(){
        mm = this;
    }

    public void SetIp(IPEndPoint ep){
        insertIp.text = $"{ep.Address}:{LanConnector.PORT}";
    }

    public void JoinGame(){
        IPEndPoint ip;
        string error = LocalLanListener.CreateIPEndPoint(insertIp.text,out ip);
        if(error != null && !error.Equals("")){
            ipErrorText.text = error;
            return;
        }
        ipErrorText.text = "";
        bool tstCnn = MemoryHandler.mh.ConnectToServer(ip);
        if(tstCnn){
            MemoryHandler.mh.SetActiveScene(MemoryHandler.mh.scGame);
        }
    }
}
