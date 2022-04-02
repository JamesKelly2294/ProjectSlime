using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Region
{
    Earth
}

public class PointOfInterest : MonoBehaviour
{
    public string Name;
    public Region Region;
    public Building Building;

    public void Start()
    {
        FindObjectOfType<PointOfInterestManager>().RegisterPointOfInterest(this);

        gameObject.transform.name = "POI " + Name;
    }
}