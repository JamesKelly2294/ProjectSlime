using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestManager : MonoBehaviour
{
    public List<PointOfInterest> PointsOfInterest { get; private set; }

    public PointOfInterest SelectedPointOfInterest { get; private set; }

    public void RegisterPointOfInterest(PointOfInterest pointOfInterest)
    {
        PointsOfInterest.Add(pointOfInterest);
    }

    public void SetSelectedPointOfInterest()
    {

    }

    private void Awake()
    {
        PointsOfInterest = new List<PointOfInterest>();
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
