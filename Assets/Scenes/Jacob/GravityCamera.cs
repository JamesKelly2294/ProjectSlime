using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCamera : MonoBehaviour
{

    public Vector3 normal = new Vector3(0, 0, -1);

    public Vector3 lookAt = new Vector3(0, 0, 0);

    public float distance = 10;

    public float scale = 0.5f;

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
        var point = lookAt + (normal * distance);
        var degree = scale * new Vector3(-1 * Physics2D.gravity.y, Physics2D.gravity.x, 0);
        var cameraLocation = RotatePointAroundPivot(point, lookAt, degree);
        transform.position = cameraLocation;
        transform.LookAt(lookAt);
    }
}
