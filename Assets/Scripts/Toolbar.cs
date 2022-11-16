using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour
{
    [SerializeField] Color32 colActive;
    [SerializeField] Color32 colInactive;

    private void OnEnable()
    {
        ToolsUseManager.ToolSelected += UpdateToolUI;
    }

    private void OnDisable()
    {
        ToolsUseManager.ToolSelected -= UpdateToolUI;
    }

    private void UpdateToolUI(int selectedTool)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Image img = transform.GetChild(i).gameObject.GetComponent<Image>();
            if (i == selectedTool && selectedTool != -1)
                img.color = colActive;
            else
                img.color = colInactive;
        }
    }
}
