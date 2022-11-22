using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIArrow : MonoBehaviour
{
    RectTransform rt;
    [SerializeField] Animator uiAnimator;
    [SerializeField] Image uiImage;
    [SerializeField] Vector3 pos_waterTool;
    [SerializeField] Vector3 pos_axeTool;
    [SerializeField] Vector3 pos_blowTool;
    [SerializeField] Vector3 pos_Upgrade;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        ToggleUIArrow(false);
    }

    private void OnEnable()
    {
        QuestMgr.ShowQuest += UpdateTarget;
    }

    private void OnDisable()
    {
        QuestMgr.ShowQuest -= UpdateTarget;
    }

    private void UpdateTarget(quest currentQuest)
    {
        Vector3 pos;
        switch (currentQuest)
        {
            case quest.SelectWaterTool:
                pos = pos_waterTool;
                break;
            case quest.SelectAxeTool:
                pos = pos_axeTool;
                break;
            case quest.SelectBlowTool:
                pos = pos_blowTool;
                break;
            case quest.SelectSprinkler:
                pos = pos_blowTool;
                break;
            case quest.BuyTutorialUpgrade:
                pos = pos_Upgrade;
                break;
            default:
                ToggleUIArrow(false);
                return;
        }
        rt.localPosition = pos;
        ToggleUIArrow(true);
    }

    private void ToggleUIArrow(bool state)
    {
        uiAnimator.enabled = state;
        uiImage.enabled = state;
    }
}
