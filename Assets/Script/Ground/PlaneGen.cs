using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneGen : MonoBehaviour
{
    //Mesh m;
    [Range(16,256)]
    public int edgeLength = 16;
    public float planeScale = 1;
    void Awake()
    {
        //Debug.Log("h1");
        GenerateGround geng = transform.parent.GetComponent<GenerateGround>();
        this.edgeLength = geng.edgeLength;
        this.planeScale = geng.planeScale;
        RemakeModel();
    }
    [ContextMenu("RemakeModel")]
    public void RemakeModel()
    {

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            this.AddComponent<MeshFilter>();
            meshFilter = GetComponent<MeshFilter>();
        }
        MeshCollider col = GetComponent<MeshCollider>();
        if (col == null)
        {
            this.AddComponent<MeshCollider>();
            col = GetComponent<MeshCollider>();
        }
        Mesh m = meshFilter.mesh;
        /*
        if (m != null)
        {
            Destroy(m);
        }
        if(col.sharedMesh != null)
            Destroy(col.sharedMesh);
        m = new Mesh();
        */
        if(m == null)
            m = new Mesh();
        // **/
        //int totalSize = (edgeLength + 1) * 2 + edgeLength * edgeLength - 1;
        int index = 0;
        int pointOffset = (edgeLength + 1) * (edgeLength + 1);
        int triOffset = edgeLength * edgeLength * 6;
        float factor = 2F / edgeLength;
        Vector3[] points = new Vector3[pointOffset];
        Color[] colors = new Color[pointOffset];
        Vector3[] normals = new Vector3[pointOffset];
        Vector2[] uvs = new Vector2[pointOffset];
        int[] tris = new int[triOffset];
        Vector3 vec;
        Noises noise = GroundState.groundSetting.nss;
        for (int overX = 0; overX < edgeLength + 1; overX++)
        {
            for (int overY = 0; overY < edgeLength + 1; overY++, index++)
            {
                // xy up
                float deltaXY = 0.001f;
                float h0 = noise.GetHeight(overX * planeScale * factor + transform.position.x,overY * planeScale * factor + transform.position.z);
                float h1 = noise.GetHeight(overX * planeScale * factor + transform.position.x + deltaXY,overY * planeScale * factor + transform.position.z);
                float h2 = noise.GetHeight(overX * planeScale * factor + transform.position.x,overY * planeScale * factor + transform.position.z + deltaXY);
                //float h0 = Mathf.PerlinNoise(overX * planeScale * factor + transform.position.x,overY * planeScale * factor + transform.position.z);
                //float h1 = Mathf.PerlinNoise(overX * planeScale * factor + transform.position.x + deltaXY,overY * planeScale * factor + transform.position.z);
                //float h2 = Mathf.PerlinNoise(overX * planeScale * factor + transform.position.x,overY * planeScale * factor + transform.position.z + deltaXY);
                vec = new Vector3((overX * factor - 1) * planeScale,
                                h0,
                                (overY * factor - 1) * planeScale);
                normals[index] = -Vector3.Cross(new Vector3(deltaXY,h1 - h0,0),new Vector3(0,h2 - h0,deltaXY));
                //normals[index].Normalize();
                uvs[index] = new Vector2(overX * factor, overY * factor);
                points[index] = vec;
                colors[index] = new Color(1f,1f,1f);
            }
        }

        // triangle creation
        index = 0;
        int overIndex = 0;
        for (int overX = 0; overX < edgeLength; overX++, overIndex++)
        {
            for (int overY = 0; overY < edgeLength; overY++, index++, overIndex++)
            {
                int nextIndex = overIndex + edgeLength + 1;

                // xy up
                tris[index * 6 + 0] = overIndex + 0;
                tris[index * 6 + 1] = overIndex + 1;
                tris[index * 6 + 2] = nextIndex + 0;
                tris[index * 6 + 3] = nextIndex + 0;
                tris[index * 6 + 4] = overIndex + 1;
                tris[index * 6 + 5] = nextIndex + 1;
            }
        }
        if(m.vertices.Length < points.Length){
            m.vertices = points;
            m.uv = uvs;
            m.normals = normals;
            m.colors = colors;
            //
            m.triangles = tris;
        }else{
            m.triangles = tris;
            //
            m.vertices = points;
            m.uv = uvs;
            m.normals = normals;
            m.colors = colors;
        }
        meshFilter.mesh = m;
        col.sharedMesh = m;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
