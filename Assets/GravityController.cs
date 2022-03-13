using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{

    float speed = 1;
    
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

        var gravity = 9.81f;
        var dt = gravity * Time.deltaTime * speed;

        if (Input.GetKey(KeyCode.W)) {
            Physics.gravity += new Vector3(0,0,dt);
        } else if (Input.GetKey(KeyCode.S)) {
            Physics.gravity += new Vector3(0,0,-dt);
        }
        
        if (Input.GetKey(KeyCode.A)) {
            Physics.gravity += new Vector3(-dt,0,0);
        } else if (Input.GetKey(KeyCode.D)) {
            Physics.gravity += new Vector3(dt,0,0);
        } 

        Physics.gravity *= (gravity / Physics.gravity.magnitude);



        // Physics.gravity = RotatePointAroundPivot(new Vector3(0, -9.81f, 0), Vector3.zero, new Vector3(-tiltY, 0, -tiltX));
        print(Physics.gravity);
    }
}
