using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlowArea : MonoBehaviour
{
    [SerializeField] float blowPower;
    internal bool isBlowing = false;

    private void OnTriggerStay(Collider other)
    {
        if (!isBlowing)
            return;

        if (other.gameObject.layer == 7)
        {
            Vector3 blowVector = Camera.main.transform.forward * blowPower;
            blowVector.y /= 4f;
            other.gameObject.GetComponent<Rigidbody>().AddForce(blowVector, ForceMode.Acceleration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -transform.right * 3);
    }
}
