using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int currentTeam;
    public LanConnector localConnector;
    public UIManager uiMan;
    GroundState gs;
    void Awake(){
        gm = this;
        localConnector = new LanConnector(25359);
    }
    
    public void RegisterGroundService(GroundState gs){
        this.gs = gs;
    }
}
