using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingGem : MonoBehaviour
{
    Vector2 targetPos = new Vector2(-800f, - 458.7f);
    RectTransform rt;

    Image img;
    Color32 colOpaque = new Color32(255, 255, 255, 255);
    Color32 colTransparent = new Color32(255, 255, 255, 0);

    bool flyEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        Vector3 offset = Vector3.zero;
        offset.x = Random.Range(-80f, 80f);
        offset.y = Random.Range(-80f, 80f);

        rt.localPosition += offset;
        img.color = colTransparent;

        StartCoroutine(RandomDelay());
    }

    IEnumerator RandomDelay()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0f));
        flyEnabled = true;
        img.color = colOpaque;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flyEnabled)
            return;

        rt.localPosition = Vector3.Lerp(rt.localPosition, targetPos, 4 * Time.deltaTime);
        rt.localScale = Vector3.Lerp(rt.localScale, Vector3.zero, 2 * Time.deltaTime);
        img.color = Color.Lerp(img.color, colTransparent, 0.7f * Time.deltaTime);

        if (rt.localScale.x < 0.3f) 
        {
            Destroy(gameObject);
        }
    }
}
