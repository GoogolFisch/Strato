using UnityEngine;

public class SettingsChanger : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float InitialFOV;
    [SerializeField] private float ZoomedFOV;
    [SerializeField] private float ZoomStrength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 60;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
