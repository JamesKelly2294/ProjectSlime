using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Region
{
    Earth
}

public class PointOfInterest : MonoBehaviour
{
    public string Name;
    public Region Region;
    public List<Building> StartingBuildings;
    public List<ConstructedBuilding> ConstructedBuildings = new List<ConstructedBuilding>();
    public int StartingPopulation;
    public bool needsThe = false;

    public int TotalPopulation { get { return AvailablePopulation + ConsumedPopulation; } }
    public int AvailablePopulation { get; private set; }
    public int ConsumedPopulation { get; private set; }

    [Range(0.0f, 1.0f)]
    public float BuildingDistanceFromCenter;

    private Earth _earth;
    private GameObject _buildingVisualsParentGO;

    public void Start()
    {
        _earth = FindObjectOfType<Earth>();
        FindObjectOfType<PointOfInterestManager>().RegisterPointOfInterest(this);

        gameObject.transform.name = "POI " + Name;

        AvailablePopulation = StartingPopulation - ConsumedPopulation;

        _buildingVisualsParentGO = new GameObject();
        _buildingVisualsParentGO.transform.parent = transform;
        _buildingVisualsParentGO.transform.position = transform.position;
        _buildingVisualsParentGO.transform.name = "Building Visuals";

        foreach(var building in StartingBuildings)
        {
            ConstructBuilding(building);
        }

        UpdateBuildingVisuals();
    }

    public void ConstructBuilding(Building b)
    {
        var constructedBuilding = new ConstructedBuilding();
        constructedBuilding.Building = b;
        constructedBuilding.Active = true;

        ConstructedBuildings.Add(constructedBuilding);
    }

    public void WasSelected()
    {
        Debug.Log(Name + " was selected");
    }

    public void WasDeselected()
    {
        Debug.Log(Name + " was deselected");
    }

    public void UpdateBuildingVisuals()
    {
        foreach (Transform child in _buildingVisualsParentGO.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (ConstructedBuildings.Count == 0)
        {
            return;
        }

        float angle = 0.0f;
        float angularSeparation = 360.0f / (ConstructedBuildings.Count);
        var sortedBuildings = ConstructedBuildings.OrderBy(o => o.Building.RegionCapital).ToList();
        for (int i = 0; i < sortedBuildings.Count; i++)
        {
            var building = sortedBuildings[i];
            var visualsGo = Instantiate(building.Building.VisualsPrefab);

            if (i == 0)
            {
                var newPos = transform.position + ((Quaternion.AngleAxis(angle, transform.forward) * transform.right) * BuildingDistanceFromCenter);
                var dir = (newPos - _earth.transform.position).normalized;
                visualsGo.transform.position = transform.position;
            } else
            {
                var newPos = transform.position + ((Quaternion.AngleAxis(angle, transform.forward) * transform.right) * BuildingDistanceFromCenter);
                var dir = (newPos - _earth.transform.position).normalized;
                visualsGo.transform.position = _earth.transform.position + (dir * _earth.Radius);
            }
            visualsGo.transform.parent = _buildingVisualsParentGO.transform;

            angle += angularSeparation;
        }
    }

    public void CalculateConsumedPopulation()
    {
        ConsumedPopulation = 1;
    }


}