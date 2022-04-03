using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<Building> Buildings;

    public List<Building> AvailableBuildings { get; private set; }

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
