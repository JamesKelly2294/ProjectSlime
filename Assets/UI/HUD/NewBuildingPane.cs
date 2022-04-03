using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewBuildingPane : MonoBehaviour
{

    public TextMeshProUGUI titleTMP;

    public GameObject buildingsList, content, background;

    public ExistingBuildingRow existingBuildingRow;

    public GameObject buildingPrefab;

    public FilterButtonRow filterButtonRow;

    private BuildingManager buildingManager;
    private PointOfInterestManager pointOfInterestManager;
    private PointOfInterest displayedPOI = null;
    private ConstructedBuilding displayedExistingBuilding = null;

    private List<Building> constructionOptions;

    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GameObject.FindObjectOfType<BuildingManager>();
        pointOfInterestManager = GameObject.FindObjectOfType<PointOfInterestManager>();
        Display(null, null);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Display(PointOfInterest pointOfInterest, ConstructedBuilding existingBuilding) {
        Debug.Log("Display " + pointOfInterest + ", " + existingBuilding);
        displayedPOI = pointOfInterest;
        displayedExistingBuilding = existingBuilding;
        if (pointOfInterest == null) {
            content.SetActive(false);
            background.SetActive(false);
            return;
        } else {
            content.SetActive(true);
            background.SetActive(true);
        }

        if (existingBuilding != null)
        {
            titleTMP.text = "<b>" + (existingBuilding.Building.Name) + "</b>";
            existingBuildingRow.gameObject.SetActive(true);
            existingBuildingRow.SetConstructedBuilding(existingBuilding);
        }
        else
        {
            titleTMP.text = "Fund Project in" + (pointOfInterest.needsThe ? " the " : " ") + "<b>" + pointOfInterest.Name + "</b>";
            existingBuildingRow.gameObject.SetActive(false);
        }
        
        // Add the list of buildings
        var b = existingBuilding != null ? existingBuilding.Building : null;
        constructionOptions = buildingManager.ConstructionOptions(b);
        UpdateShownBuildings();
    }

    public void UpdateShownBuildings()
    {
        foreach (Transform child in buildingsList.transform)
        {
            if (child.GetComponent<NewBuildingRow>())
            {
                Destroy(child.gameObject);
            }
        }

        foreach (Building building in constructionOptions)
        {
            bool shouldShowBuilding = false;
            foreach (var effect in building.ResourceEffects)
            {
                var bit = (1 << (int)effect.AffectedResource);
                if ((filterButtonRow.ResourceTypeFilterMask & bit) != 0 && effect.EffectAmount > 0)
                {
                    shouldShowBuilding = true;
                    break;
                }
            }

            if (!shouldShowBuilding)
            {
                continue;
            }

            GameObject gameObject = GameObject.Instantiate(buildingPrefab, buildingsList.transform);
            NewBuildingRow newBuildingRow = gameObject.GetComponent<NewBuildingRow>();
            newBuildingRow.SetBuilding(building);
        }
    }

}
