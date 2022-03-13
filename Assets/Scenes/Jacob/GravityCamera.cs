using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCamera : MonoBehaviour
{

    public Vector3 normal = new Vector3(0, 0, -1);

    public Vector3 lookAt = new Vector3(0, 0, 0);

    public float distance = 10;

    public float scale = 0.5f;

    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    // Update is called once per frame
    void Update()
    {
        // var point = lookAt + (normal * distance);
        // var degree = scale * new Vector3(Physics.gravity.x, 0, Physics.gravity.z);
        // var cameraLocation = RotatePointAroundPivot(point, lookAt, degree);
        // transform.position = cameraLocation;
        // transform.LookAt(lookAt);

        transform.localRotation = Quaternion.Euler(0, 0, scale * Physics.gravity.x) * Quaternion.Euler( -(scale * Physics.gravity.z) + 90, 0, 0);
        // transform.position = new Vector3(-scale * Physics.gravity.x,  distance, -scale * Physics.gravity.z) + ball.transform.position;
        transform.position = ball.transform.position + new Vector3(0, distance, 0);
        //transform.LookAt(lookAt);
    }
}
