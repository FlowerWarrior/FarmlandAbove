using UnityEngine;
public enum Rarity
{
    Common = 0,
    Rare = 1,
    Epic = 2
}
public struct CodeSF
{
    public static bool IsAlmostZero(float value, float errorMargin)
    {
        if (value > -errorMargin/2 && value < errorMargin/2)
        {
            return true;
        }

        return false;
    }

    public static float WrapSinValue(float val)
    {
        // Reset when sin period, seamless loop
        if (val >= Mathf.PI * 2)
        {
            return 0;
        }
        return val;
    }

    public static float GetSinValueFalloffMultiplier(float val)
    {
        float multiplier = 1;
        if (CodeSF.IsSinValueFalling(val))
            multiplier = 1.6f;

        return multiplier;
    }

    private static bool IsSinValueFalling(float val)
    {
        if (val > Mathf.PI * 1/2 && val < Mathf.PI * 2/2)
        {
            return true;
        }

        if (val > Mathf.PI * 3/2 && val < Mathf.PI * 4/2)
        {
            return true;
        }

        return false;
    }

    public enum VegetableType
    {
        Beetroot,
        Carrot, 
        Tomato
    }

    public static Color32 GetRarityColor(Rarity rarity)
    {
        Color32 col = Color.white;

        switch (rarity)
        {
            case Rarity.Common:
                col = new Color32(199, 236, 238, 255);
                break;
            case Rarity.Rare:
                col = new Color32(186, 220, 88, 255);
                break;
            case Rarity.Epic:
                col = new Color32(224, 86, 253, 255);
                break;
        }

        return col;
    }

    public static void DestroyAllChildren(Transform target)
    {
        for (int i = 0; i < target.childCount; i++)
        {
            MonoBehaviour.Destroy(target.GetChild(i).gameObject);
        }
    }
}
