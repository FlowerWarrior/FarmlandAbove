using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WelcomeTxt : MonoBehaviour
{
    public static WelcomeTxt instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowTitle(string text)
    {
        GetComponent<TextMeshProUGUI>().text = $"{text}";
        GetComponent<Animator>().Play("ShowTitle", 0, 0);
    }
}
