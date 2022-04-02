using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<Building> StartingBuildings;

    public List<Building> AvailableBuildings { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        AvailableBuildings = StartingBuildings;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
