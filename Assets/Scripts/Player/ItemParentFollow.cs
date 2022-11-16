using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemParentFollow : MonoBehaviour
{
    [SerializeField] Transform targetPoint;
    [SerializeField] float followSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint.position, Time.deltaTime * followSpeed);
    }
}
