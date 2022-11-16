using UnityEngine;

[CreateAssetMenu(menuName = "new Vegetable")]
public class VegetableData : Item
{
    public int[] value;
    public GameObject prefab;
    public int fullyGrownTicksRequired;
    public float waterThirst;
    public SeedData seedClass;
}
