using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestManager : MonoBehaviour
{
    public List<PointOfInterest> pointsOfInterest { get; private set; }

    public void RegisterPointOfInterest(PointOfInterest pointOfInterest)
    {
        pointsOfInterest.Add(pointOfInterest);
    }

    private void Awake()
    {
        pointsOfInterest = new List<PointOfInterest>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
