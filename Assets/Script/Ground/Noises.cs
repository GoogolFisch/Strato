using UnityEngine;

public class Noises
{
    public int seed;
    //public List<Resources> res;
    public Noises(int seed){
        this.seed = seed;
    }
    public Noises(){
        this.seed = Random.Range(0,0x10000) + (Random.Range(0,0x10000) <<16);
    }

    internal float pushIn(float px,int bs=16,int mask=0xffff){
        return px + ((seed >> bs) & mask);
    }
    public static void rotate(float px,float py,out float rx,out float ry,float angle){
        rx = px * Mathf.Cos(angle) + py * Mathf.Sin(angle);
        ry = -px * Mathf.Sin(angle) + py * Mathf.Cos(angle);
    }

    public float GetHeight(float px,float py){
        float rx,ry;
        px *= 0.3f;
        py *= 0.3f;
        rotate(px,py,out rx,out ry,Mathf.PI * 0.33f);
        float akku = 0;
        akku += Mathf.PerlinNoise(pushIn(px,0),pushIn(py,16));
        akku += Mathf.PerlinNoise(pushIn(rx,0),pushIn(ry,16)) * 0.3333f;
        return akku / 1.3333f;
    }
}
