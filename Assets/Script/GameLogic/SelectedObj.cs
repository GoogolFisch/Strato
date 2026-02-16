using UnityEngine;

public class SelectedObj : MonoBehaviour
{
    public float angelVel = 1;
    public static SelectedObj selObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        selObj = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rot = gameObject.transform.localRotation.eulerAngles; //get the angles
        rot.Set(0f, rot.y + angelVel * Time.deltaTime, 0f); //set the angles
        gameObject.transform.localRotation = Quaternion.Euler(rot); //update the transform
        
    }
}
