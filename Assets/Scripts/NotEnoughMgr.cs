using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotEnoughMgr : MonoBehaviour
{
    [SerializeField] GameObject notif;
    [SerializeField] float displayTime;
    [SerializeField] TextMeshProUGUI displayText;

    internal static NotEnoughMgr instance;
    internal static System.Action Notified;
    private void Awake() => instance = this;

    public void ShowNotEnoughNotifAtCursor()
    {
        ShowNotif("Not enough money");
        Notified?.Invoke();
    }

    public void ShowNotEnoughAtCenter()
    {
        ShowNotif("Not enough money", false);
        Notified?.Invoke();
    }

    public void ShowNotEnoughSeeds()
    {
        ShowNotif("Requires 3 seeds!");
        Notified?.Invoke();
    }

    private void ShowNotif(string text, bool atMousePos = true)
    {
        displayText.text = text;
        if (atMousePos)
        {
            notif.GetComponent<RectTransform>().position = Input.mousePosition;
        }
        else
        {
            notif.GetComponent<RectTransform>().localPosition = new Vector2(0, -88);
            print("hello");
        }
            
        notif.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HideAfter(displayTime));
    }

    IEnumerator HideAfter(float t)
    {
        yield return new WaitForSeconds(t);
        notif.SetActive(false);
    }
}
