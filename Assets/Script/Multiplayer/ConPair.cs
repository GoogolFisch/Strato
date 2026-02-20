using System.Net;
using UnityEngine;
using System.Collections.Generic;

public class ConPair
{
    public byte[] symkey;
    public EndPoint point;
}

public class ConPairs
{
    public byte[] symkey;
    public EndPoint point;
    public List<ConPair> pairs;
    public int Has(EndPoint ep)
    {
        for(int i = 0;i < pairs.Count; i++)
        {
            if(ep == pairs[i].point)return i;
        }
        return -1;
    }
}