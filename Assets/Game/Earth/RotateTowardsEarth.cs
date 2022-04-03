using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsEarth : MonoBehaviour
{
    private GameObject _target;

    public void Awake()
    {
        _target = GameObject.FindObjectOfType<Earth>().gameObject;

        UpdateRotation();
    }

    private void LateUpdate()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        var lookRotation = transform.position - _target.transform.position;

        if (lookRotation != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _target.transform.position);
        }
    }
}
