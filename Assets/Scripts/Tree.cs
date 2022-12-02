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

        float falloff = 0.6f;
        float speed = 2.2f;

        float timer = 0f;
        while (timer <= 1f)
        {
            float val = -Mathf.Pow(1-Mathf.Pow(timer, falloff), 1/falloff) + 1;
            transform.localScale = Vector3.Lerp(Vector3.zero, defaultScale, val);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime * speed;
        }
    } 
}
