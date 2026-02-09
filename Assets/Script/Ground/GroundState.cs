
using UnityEngine;

public class GroundState : MonoBehaviour
{
    public static GroundState groundSetting;
    Noises nss;
    Noises GetNoise(){
        if(nss == null){
            nss = new Noises();
        }
        return nss;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager.gm.RegisterGroundService(this);
        groundSetting = this;
        if(nss == null){
            nss = new Noises();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}