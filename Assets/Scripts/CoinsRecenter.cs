using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRecenter : MonoBehaviour
{
    [SerializeField] float xOffset = -9.5f;

    Vector3 initialPos;
    RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        initialPos = rt.localPosition;
    }

    void RecenterSelf(int coinsAmount)
    {
        rt.localPosition = initialPos + new Vector3(xOffset * (coinsAmount.ToString().Length - 1), 0, 0);
    }

    void OnEnable()
    {
        UIMGR.UpdateCoinsUI += RecenterSelf;
    }

    void OnDisable()
    {
        UIMGR.UpdateCoinsUI -= RecenterSelf;
    }
}
