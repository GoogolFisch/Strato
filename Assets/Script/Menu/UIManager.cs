using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System;

public class UIManager : MonoBehaviour
{
    public InfoPannel makePan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public InfoPannel genAndShowUI(Vector3 position,List<string> texts,List<UnityAction> callBack)
    {
        InfoPannel iPan = Instantiate(makePan,position,Quaternion.identity,transform);
        iPan.generate(texts,callBack);
        return iPan;
    }
}
