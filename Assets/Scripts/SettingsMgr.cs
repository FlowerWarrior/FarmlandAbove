using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Sync the frame rate to the screen's refresh rate
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
