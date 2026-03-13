using UnityEngine;

public class UPSDeath : MonoBehaviour
{
    public static UPSDeath upsdeath;
    ParticleSystem part;
    void Awake(){
        if(upsdeath == null)
            upsdeath = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        part=GetComponent<ParticleSystem>(); 
        if(part.isPlaying)
        {
            Debug.Log("particle Effect is live");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!part.isPlaying && upsdeath != this)
        {
            Destroy(gameObject);
        }
    }
    [ContextMenu("Play")]
    public void PlayAnimation(){
        part.Play();

    }
    public void PlayAnimation(Vector3 vec,Transform trans){
        Instantiate(part,vec,transform.rotation,trans);
    }
}
