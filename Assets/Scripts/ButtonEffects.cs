using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour
{
    internal static System.Action ButtonClicked;
    Button myButton;
    RectTransform rt;
    Vector3 intialScale, downScale;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnButtonClicked);
        intialScale = rt.localScale;
        downScale = intialScale * 0.9f;

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => SmallSize());
        trigger.triggers.Add(pointerDown);

        var pointerDown2 = new EventTrigger.Entry();
        pointerDown2.eventID = EventTriggerType.PointerUp;
        pointerDown2.callback.AddListener((e) => NormalSize());
        trigger.triggers.Add(pointerDown2);

        var pointerDown3 = new EventTrigger.Entry();
        pointerDown3.eventID = EventTriggerType.PointerExit;
        pointerDown3.callback.AddListener((e) => NormalSize());
        trigger.triggers.Add(pointerDown3);
    }

    private void SmallSize()
    {
        rt.localScale = downScale;
    }

    private void NormalSize()
    {
        rt.localScale = intialScale;
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke();
    }
}
