using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInput : MonoBehaviour
{
    static int pointOfInterestLayer = 7;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int layerMask = ~0;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(Input.mousePosition, ray.direction * hit.distance, Color.yellow);

                if (hit.collider.gameObject.layer == pointOfInterestLayer)
                {
                    Debug.Log("Hit point of interest!");
                }
            }
        }
    }
}
