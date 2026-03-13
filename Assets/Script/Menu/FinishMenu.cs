using UnityEngine;
using UnityEngine.UI;

public class FinishMenu : MonoBehaviour
{
    public static FinishMenu fm;
    public Text textBox;
    public Button leaveButton;
    public Button okButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        fm = this;
    }
    void Start()
    {
        if(MemoryHandler.mh != null && MemoryHandler.mh.shan != null &&
                MemoryHandler.mh.shan.clCons != null)
            leaveButton.gameObject.SetActive(false);
        okButton.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetWin(string message){
        textBox.text = message;
        gameObject.SetActive(true);
        GameMenu.gm.gameObject.SetActive(false);
        PlayerController.pc.isActive = false;
    }
    public void ButtonExit(){
        MemoryHandler.mh.GameConnectionFailed();
        PlayerController.pc.isActive = true;
    }
    public void ButtonOK(){
        gameObject.SetActive(false);
        PlayerController.pc.isActive = true;
    }
    public void AllowExit(){
        leaveButton.gameObject.SetActive(true);
        okButton.gameObject.SetActive(false);
    }
}
