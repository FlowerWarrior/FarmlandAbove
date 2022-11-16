using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    [SerializeField] Slider hpslider;

    // Start is called before the first frame update
    void Start()
    {
        HideHpBar();
        hpslider.gameObject.SetActive(false);
    }

    public void ShowAfterHit(float value)
    {
        StopAllCoroutines();
        hpslider.value = value;
        StartCoroutine(ShowBar(10));
    }

    private void ShowHpBar()
    {
        hpslider.gameObject.SetActive(true);
    }

    public void HideHpBar()
    {
        hpslider.gameObject.SetActive(false);
    }

    IEnumerator ShowBar(float delay)
    {
        ShowHpBar();
        yield return new WaitForSeconds(delay);
        HideHpBar();
    }
}
