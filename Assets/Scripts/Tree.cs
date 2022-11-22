using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] int maxHp;
    [SerializeField] int respawnDelay;
    [SerializeField] int coinsAmount;

    [SerializeField] Collider[] myColliders;
    [SerializeField] MeshRenderer myMeshRender;
    [SerializeField] Animator myAnimator;

    int hp;
    Vector3 defaultScale;

    public static System.Action TreeDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        defaultScale = transform.localScale;
    }

    public void OnHit(int damage)
    {
        hp -= damage;
        myAnimator.Play("TreeHit", 0, 0);

        if (hp <= 0)
        {
            ToggleActive(false);
            InventorySystem.instance.AddCoins(coinsAmount);
            TreeDestroyed?.Invoke();
            InventorySystem.instance.AddTreeCutCounter();
            StartCoroutine(RespawnTreeAfter(respawnDelay));
        }
    }

    private void ToggleActive(bool state)
    {
        for (int i = 0; i < myColliders.Length; i++)
        {
            myColliders[i].enabled = state;
        }
        
        myMeshRender.enabled = state;
    }

    IEnumerator RespawnTreeAfter(float t)
    {
        yield return new WaitForSeconds(t);
        hp = maxHp;
        transform.localScale = Vector3.zero;
        ToggleActive(true);

        while (!(transform.localScale.x > defaultScale.x-0.01f && transform.localScale.x < defaultScale.x + 0.01f))
        {
            transform.localScale += Vector3.one * Time.deltaTime * 1.7f;
            yield return new WaitForEndOfFrame();
        }
    } 
}
