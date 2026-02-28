using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeLogger : MonoBehaviour
{
    public static DeLogger dl;
    public List<Text> logs;
    public const int COUNT = 32;
    public Text insText;
    void Awake(){
        DeLogger.dl = this;
    }

    void FixedUpdate(){
        dl = this;
    }

    public void Log(string str){
        if(logs.Count >= COUNT){
            Destroy(logs[0].gameObject);
            logs.RemoveAt(0);
        }
        Text newT = Instantiate(insText,Vector3.zero,
                        Quaternion.identity,transform);
        newT.text = str;
        logs.Add(newT);
    }
}
