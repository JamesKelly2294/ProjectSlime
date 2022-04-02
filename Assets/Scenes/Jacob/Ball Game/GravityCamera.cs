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
        transform.localRotation = Quaternion.Euler(0, 0, scale * Physics.gravity.x) * Quaternion.Euler( -(scale * Physics.gravity.z) + 90, 0, 0);
        transform.position = ball.transform.position;
        transform.localPosition += new Vector3(0, distance, 0);
    }
}
