using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateGround : MonoBehaviour
{
    [Range(4,16)]
    public int GROUND_TILES = 32;
    [Range(16,64)]
    public int edgeLength = 64;
    public float planeScale = 4;
    public GameObject planeTiles;
    void Awake(){

        //Debug.Log("h0");
        float reposXY = GROUND_TILES * planeScale;
        for(int px = 0;px < GROUND_TILES;px++){
            for(int py = 0;py < GROUND_TILES;py++){
                Vector3 vpos = new Vector3(planeScale * px * 2 - reposXY,0,planeScale * py * 2 - reposXY);
                Instantiate(planeTiles,vpos,Quaternion.identity,transform);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Instantiate(objectToSpawn, transform); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
