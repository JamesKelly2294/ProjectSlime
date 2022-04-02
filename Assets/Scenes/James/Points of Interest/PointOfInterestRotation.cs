using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestRotation : MonoBehaviour
{
    public GameObject target;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
    }
}
