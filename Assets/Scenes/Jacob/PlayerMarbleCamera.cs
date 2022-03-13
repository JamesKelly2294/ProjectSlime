using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarbleCamera : MonoBehaviour
{

     public GameObject ball;

     public float distance = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ball.transform.position;
        transform.localPosition += new Vector3(0, distance, 0);
    }
}
