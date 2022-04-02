using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsEarth : MonoBehaviour
{
    private GameObject _target;

    public void Start()
    {
        _target = GameObject.FindObjectOfType<Earth>().gameObject;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _target.transform.position);
    }
}
