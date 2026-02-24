using UnityEngine;
using System.Net;
using UnityEngine.Serialization;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class LocalLanListener : MonoBehaviour
{
    public List<(IPEndPoint,Button)> foundList = new List<(IPEndPoint,Button)>();
    public int tick = 0;
    public Text text;
    public Button button;
    public GameObject insertInto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tick++;
        if(tick < 10)return;
        tick = 0;
        IPEndPoint eps;
        byte[] arr;
        (eps,arr) = MemoryHandler.mh.GetLocalGames();
        if(eps == null)return;
        if(arr == null)return;
        IPEndPoint iterEp;
        Button iterBut;
        for(int idx = 0;idx < foundList.Count;idx++){
            (iterEp,iterBut) = foundList[idx];

            if(!(
                iterEp.Port == eps.Port && iterEp.Address.Equals(eps.Address)
                ))
                continue;
            Debug.Log($"{eps} ?= {iterEp}");
            DestroyImmediate(iterBut.gameObject);
            foundList.RemoveAt(idx);
            break;
        }
        int vers;
        string strOut;
        StructMenuListing sml = LanConnector.GetSessionInfo(arr);

        Button nbut = Instantiate(button,Vector3.zero,Quaternion.identity,insertInto.transform);
        Text cnttxt = Instantiate(text,nbut.transform.position,Quaternion.identity,nbut.transform);
        cnttxt.text = $"{sml.plCount}/{sml.maxPlCnt}";
        Text dsptxt = Instantiate(text,nbut.transform.position,Quaternion.identity,nbut.transform);
        dsptxt.text = sml.display;
        dsptxt.rectTransform.sizeDelta += new Vector2 (100, 0);
        foundList.Add((eps,nbut));
    }
}
