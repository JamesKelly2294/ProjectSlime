using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotationControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [Range(0.0f, 15.0f)]
    public float rotationSpeed = 1.0f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * rotationSpeed);
        }

    }
}
