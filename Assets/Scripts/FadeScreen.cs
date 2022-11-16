using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayerRespawner.PlayerFellBelow += PlayAnimRespawn;
    }

    private void OnDisable()
    {
        PlayerRespawner.PlayerFellBelow -= PlayAnimRespawn;
    }

    private void PlayAnimRespawn()
    {
        myAnimator.Play("RespawnFade", 0, 0);
    }
}
