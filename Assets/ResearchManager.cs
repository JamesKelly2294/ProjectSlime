using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    private BuildingManager _bm;

    private GameResourceManager _rm;

    private List<Building> _researchedBuildings = new List<Building>();

    // Start is called before the first frame update
    void Start()
    {
        _bm = FindObjectOfType<BuildingManager>();
        _rm = FindObjectOfType<GameResourceManager>();

        foreach(var b in _bm.Buildings)
        {
            if (b.ResearchCost == 0)
            {
                _researchedBuildings.Add(b);
            }
        }
    }

    public bool BuildingIsResearched(Building b)
    {
        return _researchedBuildings.Contains(b);
    }

    public void ResearchBuilding(Building b)
    {
        if (_researchedBuildings.Contains(b) == false)
        {
            _researchedBuildings.Add(b);
            var sender = GetComponent<PubSubSender>();
            if (sender)
            {
                sender.Publish("buildingResearched");
            Debug.Log("?buildingResearched");
            }
        }
    }
}
