using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public GroundState gs;
    void Awake(){
        gm = this;
    }
    
    public void RegisterGroundService(GroundState gs){
        this.gs = gs;
    }
}
