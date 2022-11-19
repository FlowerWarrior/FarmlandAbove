using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractTorchUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;

    public void UpdateTime(int timeleft)
    {
        timeText.text = $"{timeleft}s";
    }
}
