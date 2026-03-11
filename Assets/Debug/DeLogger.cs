using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeLogger : MonoBehaviour
{
    public static DeLogger dl;
    public List<Text> logs;
    public const int COUNT = 32;
    public Text insText;
    public bool alive;
    public bool isActive = false;
    void Awake(){
        DeLogger.dl = this;
        alive = true;
        gameObject.SetActive(false);
    }
    void Start(){
    }

    void FixedUpdate(){
        dl = this;
    }

    public void Log(string str){
        if(!alive)return;
        if(logs.Count >= COUNT){
            Destroy(logs[0].gameObject);
            logs.RemoveAt(0);
        }
        Text newT = Instantiate(insText,Vector3.zero,
                        Quaternion.identity,transform);
        newT.text = str;
        logs.Add(newT);
    }
    void OnDestroy(){
        alive = false;
    }
}
