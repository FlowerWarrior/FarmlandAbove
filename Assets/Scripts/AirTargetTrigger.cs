using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTargetTrigger : MonoBehaviour
{
    internal bool isAirRayHit = false;
    bool isTouchingRay = false;

    private void FixedUpdate() // Order of Exec 1
    {
        isTouchingRay = false;
        StartCoroutine(CheckAtFixedUpdateEnd());
    }

    private void OnTriggerStay(Collider other) // Order of Exec 2
    {
        if (other.gameObject.layer == 10) //airray layer
        {
            isTouchingRay = true;
        }
    }

    private IEnumerator CheckAtFixedUpdateEnd() // Order of Exec 3
    {
        yield return new WaitForFixedUpdate();
        isAirRayHit = isTouchingRay;
    }
}
