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
    private GameResourceManager _rm;

    public void Start()
    {
        _earth = FindObjectOfType<Earth>();
        _rm = FindObjectOfType<GameResourceManager>();
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

    public bool CanPurchaseBuilding(Building b)
    {
        var popCost = b.RecurringCostForType(ResourceType.Pop);
        if (popCost > AvailablePopulation)
        {
            return false;
        }

        foreach(var effect in b.ResourceEffects)
        {
            if (effect.EffectAmount < 0 && !_rm.CanAffordResourceConsumption(effect.AffectedResource, effect.EffectAmount))
            {
                return false;
            }
        }

        if (!_rm.CanAffordResourceOneShot(ResourceType.Money, b.MoneyCost))
        {
            return false;
        }

        return true;
    }

    public void DecommissionBuilding(ConstructedBuilding b)
    {
        ConstructedBuildings.Remove(b);
        UpdateBuildingVisuals();

        PubSubSender sender = GetComponent<PubSubSender>();
        if (sender)
        {
            sender.Publish("buildingDecommissioned");
        }
    }

    public bool CanToggleBuildingPause(ConstructedBuilding b)
    {
        return true;
    }

    public bool ToggleBuildingPauseIfAble(ConstructedBuilding b)
    {
        var canToggle = CanToggleBuildingPause(b);

        if (canToggle)
        {
            PauseBuilding(b);
        }

        return canToggle;
    }

    void PauseBuilding(ConstructedBuilding b)
    {
        b.Active = !b.Active;

        var popCost = b.Building.RecurringCostForType(ResourceType.Pop);
        if (b.Active)
        {
            AvailablePopulation -= popCost;
            ConsumedPopulation += popCost;
        }
        else
        {
            AvailablePopulation += popCost;
            ConsumedPopulation -= popCost;
        }

        PubSubSender sender = GetComponent<PubSubSender>();
        if (sender)
        {
            sender.Publish("buildingPauseToggled");
        }
    }

    public bool TryPurchaseAndConstructBuilding(Building b)
    {
        if (!CanPurchaseBuilding(b))
        {
            return false;
        }

        _rm.SpendMoney(b.MoneyCost);
        ConstructBuilding(b);
        UpdateBuildingVisuals();
        PubSubSender sender = GetComponent<PubSubSender>();
        if (sender)
        {
            sender.Publish("buildingConstructed");
        }

        return true;
    }

    private void ConstructBuilding(Building b)
    {
        var popCost = b.RecurringCostForType(ResourceType.Pop);
        var constructedBuilding = new ConstructedBuilding();
        constructedBuilding.Building = b;
        constructedBuilding.Active = true;

        ConstructedBuildings.Add(constructedBuilding);

        AvailablePopulation -= popCost;
        ConsumedPopulation += popCost;
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
        var sortedBuildings = ConstructedBuildings.OrderBy(o => !o.Building.RegionCapital).ToList();
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
            visualsGo.AddComponent<RotateTowardsEarth>();

            angle += angularSeparation;
        }
    }

    public void CalculateConsumedPopulation()
    {
        ConsumedPopulation = 1;
    }


}