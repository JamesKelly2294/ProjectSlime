using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{

    [Range(1, 100)]
    public float speed = 10;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        // if (Input.GetKey(KeyCode.W)) {
        //     rigidbody.velocity += new Vector2(0, 1f);
        // } else if (Input.GetKey(KeyCode.S)) {
        //     rigidbody.velocity += new Vector2(0, -1f);
        // } else if (Input.GetKey(KeyCode.A)) {
        //     rigidbody.velocity += new Vector2(-1f, 0);
        // } else if (Input.GetKey(KeyCode.D)) {
        //     rigidbody.velocity += new Vector2(1f, 0);
        // } else if (Input.GetKey(KeyCode.Space)) {
        //     rigidbody.velocity = new Vector2(0, 0);
        // }

        var gravity = 9.81f;
        var dt = gravity * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.W)) {
            Physics2D.gravity += new Vector2(0, dt);
        } else if (Input.GetKey(KeyCode.S)) {
            Physics2D.gravity -= new Vector2(0, dt);
        } else if (Input.GetKey(KeyCode.A)) {
            Physics2D.gravity -= new Vector2(dt, 0);
        } else if (Input.GetKey(KeyCode.D)) {
            Physics2D.gravity += new Vector2(dt, 0);
        } else if (Input.GetKey(KeyCode.Space)) {
            Physics2D.gravity = new Vector2(0, 0);
        }

        var mag = Physics2D.gravity.magnitude;
        if ( mag > gravity) {
            Physics2D.gravity *= (gravity / mag);
        }

        print(Physics2D.gravity);
    }
}
