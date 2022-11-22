using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfirePoint : I_Interactable
{
    [SerializeField] AudioSource fireAudioSource;
    [SerializeField] Light fireLight;
    [SerializeField] ParticleSystem fireParticles;

    internal bool isFireLit = false;

    private void Awake()
    {
        myType = interactablePoint.Campfire;
        ToggleFire(false);
    }

    private void ToggleFire(bool newState)
    {
        isFireLit = newState;
        fireAudioSource.enabled = newState;
        fireLight.enabled = newState;

        var em = fireParticles.emission;
        em.enabled = newState;
    }

    public void LightUpCampfire()
    {
        ToggleFire(true);
    }
}
