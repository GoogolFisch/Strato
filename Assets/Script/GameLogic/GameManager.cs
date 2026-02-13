using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int currentTeam;
    GroundState gs;
    void Awake(){
        gm = this;
    }
    
    public void RegisterGroundService(GroundState gs){
        this.gs = gs;
    }
}
