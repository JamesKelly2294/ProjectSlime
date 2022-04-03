using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingManager : MonoBehaviour
{
    public List<Building> Buildings;

    public List<Building> AvailableBuildings { get; private set; }

    public List<Building> ConstructionOptions(Building forBuilding)
    {
        if (forBuilding != null)
        {
            return new List<Building>();
        }
        else
        {
            var results = from b in AvailableBuildings
                          where b.Buildable
                          select b;

            return results.ToList();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AvailableBuildings = Buildings;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
