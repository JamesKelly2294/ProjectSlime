using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Region
{
    Earth
}

public class PointOfInterest : MonoBehaviour
{

    public Region Region { get; set; }
    public string Name { get; set; }
    public Building Building { get; set; }

    public void Start()
    {
        FindObjectOfType<PointOfInterestManager>().RegisterPointOfInterest(this);
    }
}