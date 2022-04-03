using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsEarth : MonoBehaviour
{
    private GameObject _target;

    public void Awake()
    {
        _target = GameObject.FindObjectOfType<Earth>().gameObject;
        transform.rotation = Quaternion.LookRotation(transform.position - _target.transform.position);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _target.transform.position);
    }
}
