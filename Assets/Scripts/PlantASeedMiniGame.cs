using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantASeedMiniGame : MonoBehaviour
{
    [SerializeField] RectTransform indicator;
    [SerializeField] TextMeshProUGUI textResult;
    [SerializeField] float moveWidth;
    [SerializeField] float moveSpeed;

    float timer = 0.25f;
    bool isChoosing = true;

    internal SeedData seed;
    internal static System.Action seedMiniGameOpened;
    internal static System.Action<SeedData, int> PlantASeed;
    internal static System.Action IndicatorStopped;

    bool didShowWhenTutorial = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        int rnd = Random.Range(0, 2);

        if (rnd == 0 || !didShowWhenTutorial) // open this minigame
        {
            isChoosing = true;
            textResult.text = "CLICK TO CHOOSE";
            seedMiniGameOpened?.Invoke();
            didShowWhenTutorial = true;
        }
        else // skip mini game
        {
            int boostValue = 0;
            InventorySystem.instance.RemoveItemFromInventory(seed);
            PlantASeed?.Invoke(seed, boostValue);
            UIMGR.instance.CloseAllMenus();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * moveSpeed;
        if (timer > 2)
            timer = 0;

        float value = Mathf.PingPong(timer, 1);
        float x = Mathf.Lerp(-moveWidth / 2, moveWidth / 2, value);

        if (isChoosing)
            indicator.localPosition = new Vector3(x, 0, 0);
        
        if (Input.GetButtonDown("UseTool") && isChoosing)
        {
            // confirmed
            isChoosing = false;
            StartCoroutine(PlantAfter(1f, x));
        }
    }

    IEnumerator PlantAfter(float sec, float x)
    {
        float distance = Mathf.Abs(0 - x);
        int boostValue = 0;

        if (distance < 35)
        {
            boostValue = 30;
        }
        else if (distance < 35 + 100)
        {
            boostValue = 15;
        }
        else if (distance < 35 + 100 + 128)
        {
            boostValue = 0;
        }

        textResult.text = $"You got {boostValue}% boost!";
        IndicatorStopped?.Invoke();

        yield return new WaitForSeconds(sec);

        InventorySystem.instance.RemoveItemFromInventory(seed);
        PlantASeed?.Invoke(seed, boostValue);
        UIMGR.instance.CloseAllMenus();
    }
}
