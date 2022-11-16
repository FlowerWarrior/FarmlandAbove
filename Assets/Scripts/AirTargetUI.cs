using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirTargetUI : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] float hitDistance;
    RectTransform rt;
    internal Transform target;
    internal bool isHittingTargetUI = false;

    internal static AirTargetUI instance;
    internal static System.Action AitTargetCorrect;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        CameraController.AfterCameraLateUpdate += LateLateUpdate;
    }

    void LateLateUpdate()
    {
        if (target != null)
        {
            rt.position = Camera.main.WorldToScreenPoint(target.position);
            img.enabled = true;

            if (Vector2.Distance(rt.localPosition, Vector2.zero) < hitDistance)
            {
                img.color = new Color32(125, 255, 88, 255);

                if (!isHittingTargetUI)
                    AitTargetCorrect?.Invoke();

                isHittingTargetUI = true;
            }
            else
            {
                img.color = new Color32(255, 143, 88, 255);
                isHittingTargetUI = false;
            }
        }
        else
        {
            img.enabled = false;
            img.color = new Color32(255, 143, 88, 255);
            isHittingTargetUI = false;
        }
    }
}
