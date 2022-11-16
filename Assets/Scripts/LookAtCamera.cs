using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform camT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (camT == null)
        {
            try
            {
                camT = PlayerRespawner.instance.playerInstance.transform.GetChild(1);
            }
            catch
            {

            }
        }
        else
        {
            transform.LookAt(camT.position);
            transform.Rotate(new Vector3(0, 180, 0), Space.Self);
        }
    }
}
