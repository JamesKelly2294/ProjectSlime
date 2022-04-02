using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    [Range(1.0f, 20.0f)]
    public float Radius;

    private GameObject _visuals;

    // Start is called before the first frame update
    void Start()
    {
        _visuals = transform.Find("Visuals").gameObject;
        _visuals.transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
