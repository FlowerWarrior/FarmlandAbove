using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildLimitText : MonoBehaviour
{
    TextMeshProUGUI myText;

    private void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        BuildMode.CantPlace += OnCantPlace;
    }

    private void OnDisable()
    {
        BuildMode.CantPlace -= OnCantPlace;
    }

    private void OnCantPlace()
    {
        StopAllCoroutines();
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        myText.enabled = true;
        yield return new WaitForSeconds(1.3f);
        myText.enabled = false;
    }
}
