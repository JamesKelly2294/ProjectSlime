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
    public List<Building> Buildings;
    public int StartingPopulation;

    public int AvailablePopulation { get; private set; }
    public int ConsumedPopulation { get; private set; }

    [Range(0.0f, 1.0f)]
    public float BuildingDistanceFromCenter;

    private Earth _earth;
    private List<GameObject> _buildingVisuals = new List<GameObject>();

    public void Start()
    {
        _earth = FindObjectOfType<Earth>();
        FindObjectOfType<PointOfInterestManager>().RegisterPointOfInterest(this);

        gameObject.transform.name = "POI " + Name;

        AvailablePopulation = StartingPopulation - ConsumedPopulation;

        UpdateBuildingVisuals();
    }

    public void UpdateBuildingVisuals()
    {
        float angle = 0.0f;
        float angularSeparation = 360.0f / (Buildings.Count);
        foreach (var building in Buildings)
        {
            var visualsGo = Instantiate(building.VisualsPrefab);

            var newPos = transform.position + ((Quaternion.AngleAxis(angle, transform.up) * transform.right) * BuildingDistanceFromCenter);
            var dir = (newPos - _earth.transform.position).normalized;
            visualsGo.transform.position = _earth.transform.position + (dir * _earth.Radius);

            _buildingVisuals.Add(visualsGo);

            angle += angularSeparation;
        }
    }

    public void CalculateConsumedPopulation()
    {
        ConsumedPopulation = 1;
    }


}