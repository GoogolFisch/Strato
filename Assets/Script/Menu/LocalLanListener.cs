using UnityEngine;
using System.Net;
using UnityEngine.Serialization;
using System.Collections;
using System;
using System.Globalization;
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
        Debug.Log($"${eps},{arr}");
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
            //Debug.Log($"{eps} ?= {iterEp}");
            DestroyImmediate(iterBut.gameObject);
            foundList.RemoveAt(idx);
            break;
        }
        Debug.Log(eps);
        StructMenuListing sml = LanConnector.GetSessionInfo(arr);

        Button nbut = Instantiate(button,Vector3.zero,Quaternion.identity,insertInto.transform);
        nbut.onClick.AddListener(() => {MainMenu.mm.SetIp(eps);});
        Text cnttxt = Instantiate(text,nbut.transform.position,Quaternion.identity,nbut.transform);
        cnttxt.text = $"{sml.plCount}/{sml.maxPlCnt}";
        Text dsptxt = Instantiate(text,nbut.transform.position,Quaternion.identity,nbut.transform);
        dsptxt.text = sml.display;
        dsptxt.rectTransform.sizeDelta += new Vector2 (100, 0);
        foundList.Add((eps,nbut));
    }

    // Handles IPv4 and IPv6 notation.
    public static string CreateIPEndPoint(string endPoint,out IPEndPoint ipe)
    {
        IPAddress ip;
        string[] ep = endPoint.Split(':');
        ipe = null;
        if (ep.Length < 2) return "Invalid endpoint format";
        if (ep.Length > 2)
        {
            if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                return "Invalid ipv6-adress";
        }
        else
        {
            if (!IPAddress.TryParse(ep[0], out ip))
                return "Invalid ipv4-adress";
        }
        int port;
        if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
        {
            return "Invalid port";
        }
        ipe = new IPEndPoint(ip,port);
        return "";
    }
}
