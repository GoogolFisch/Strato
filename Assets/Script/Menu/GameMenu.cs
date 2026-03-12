using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public static GameMenu gm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake(){
        gm = this;
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonExit(){
        MemoryHandler.mh.GameConnectionFailed();
    }
    public void ButtonContinue(){
        gameObject.SetActive(false);
    }
    public void ButtonToggleLogging(){
        DeLogger.dl.gameObject.SetActive(
                !DeLogger.dl.gameObject.activeSelf);
    }
}
