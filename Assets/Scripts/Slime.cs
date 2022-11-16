using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] internal int islandIndex;
    [SerializeField] float moveForce;
    [SerializeField] float idleMoveForce;
    [SerializeField] float rotSpeed;
    [SerializeField] GameObject gooPrefab;

    internal GrowSlot target = null;
    Transform idleTarget = null;
    Rigidbody rb;
    Animator animator;
    bool isIdling = false;

    internal static System.Action<Vector3> SlimeMoveSound;
    internal static System.Action<Vector3> SlimeHitObstacle;

    public void OnAnimationAttack()
    {
        if (target == null)
            return;

        target.TakeDamageFromSlime();
    }

    public void SlimeMoveAnimSound()
    {
        SlimeMoveSound?.Invoke(rb.transform.position);
    }

    private void RerollIdleTarget()
    {
        idleTarget = SlimesMgr.instance.GetRandomPointOnIsland(islandIndex, idleTarget);
    }

    private IEnumerator IdleStrolling()
    {
        while (true)
        {
            RerollIdleTarget();
            yield return new WaitForSeconds(Random.Range(3, 8));
            idleTarget = null;
            yield return new WaitForSeconds(Random.Range(0, 4));
        }
    }

    private IEnumerator GooSpawnLoop()
    {
        yield return new WaitForSeconds(Random.Range(50, 120));
        Instantiate(gooPrefab, transform.position, transform.rotation);
        StartCoroutine(GooSpawnLoop());
    }

    private void FindTarget(int a)
    {
        if (a != islandIndex) return;
        target = SlimesMgr.instance.GetTargetSlotAtIsland(islandIndex);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        FindTarget(islandIndex);
        StartCoroutine(GooSpawnLoop());
    }

    private void OnEnable()
    {
        SlimesMgr.SlimeTargetAvailable += FindTarget;
    }

    private void OnDisable()
    {
        SlimesMgr.SlimeTargetAvailable -= FindTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SlimeHitObstacle?.Invoke(rb.transform.position);
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            if (target.currentState != GrowSlot.PlantState.Growing)
            {
                target = null;
                return;
            }

            isIdling = false;

            Vector3 moveVector = Vector3.Normalize(target.transform.position - rb.transform.position) * moveForce * Time.fixedDeltaTime;
            moveVector.y = 0;
            rb.position += moveVector;

            Vector3 rot = Quaternion.LookRotation(target.transform.position - rb.transform.position, Vector3.up).eulerAngles;
            rot.x = 0; rot.z = 0;
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.Euler(rot), rotSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(target.transform.position, rb.transform.position) < 1.6f)
            {
                animator.speed = 1;
                animator.SetInteger("state", 2);
            }
            else
            {
                animator.speed = 1;
                animator.SetInteger("state", 1);
            }
        }
        else if (!isIdling)
        {
            isIdling = true;
            animator.SetInteger("state", 0);
            StartCoroutine(IdleStrolling());
        }

        if (isIdling && idleTarget != null)
        {
            animator.speed = 1;
            animator.SetInteger("state", 3);

            Vector3 moveVector = Vector3.Normalize(idleTarget.position - rb.transform.position) * idleMoveForce * Time.fixedDeltaTime;
            moveVector.y = 0;
            rb.position += moveVector;

            Vector3 rot = Quaternion.LookRotation(idleTarget.position - rb.transform.position, Vector3.up).eulerAngles;
            rot.x = 0; rot.z = 0;
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.Euler(rot), rotSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(idleTarget.transform.position, rb.transform.position) < 1f)
            {
                animator.speed = 1;
                animator.SetInteger("state", 0);
            }
        }
        else if (isIdling && idleTarget == null)
        {
            animator.SetInteger("state", 0);
        }

        if (PlayerRespawner.instance.rb != null)
        {
            if (Vector3.Distance(PlayerRespawner.instance.rb.transform.position, transform.position) > 300)
            {
                Destroy(gameObject);
            }
        }
    }
}