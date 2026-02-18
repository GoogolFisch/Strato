using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System.Collections;
using System;
using UnityEngine.UI;

public class InfoPannel : MonoBehaviour
{
    public Button button;
    public Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generate(List<string> texts,List<UnityAction> callBack)
    {
        Vector3 offset = Vector3.zero;
        for(int idx = 0;idx < texts.Count;idx++){
            Button nbut = Instantiate(button,offset,Quaternion.identity,transform);
            if(callBack.Count > idx)
                nbut.onClick.AddListener(callBack[idx]);
            Text ntxt = Instantiate(text,Vector3.zero,Quaternion.identity,nbut.transform);

            ntxt.text = texts[idx];
            offset -= Vector3.up * 30;
        }
        
    }
    
}
