using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        float dt = Time.deltaTime * 10;
        if (Input.GetKey(KeyCode.W)) {
            rigidbody.velocity += new Vector3(0,0,dt);
        } else if (Input.GetKey(KeyCode.S)) {
            rigidbody.velocity -= new Vector3(0,0,dt);
        }
        
        if (Input.GetKey(KeyCode.A)) {
            rigidbody.velocity -= new Vector3(dt,0,0);
        } else if (Input.GetKey(KeyCode.D)) {
            rigidbody.velocity += new Vector3(dt,0,0);
        } 
    }
}
