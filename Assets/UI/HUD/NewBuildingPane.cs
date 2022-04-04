using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class NewBuildingPane : MonoBehaviour
{

    public TextMeshProUGUI titleTMP;

    public GameObject buildingsList, content, background;

    public ExistingBuildingRow existingBuildingRow;

    public GameObject buildingPrefab;

    public FilterButtonRow filterButtonRow;

    private BuildingManager buildingManager;
    private ResearchManager researchManager;
    private PointOfInterestManager pointOfInterestManager;
    private PointOfInterest displayedPOI = null;
    private ConstructedBuilding displayedExistingBuilding = null;

    private List<Building> constructionOptions;

    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GameObject.FindObjectOfType<BuildingManager>();
        researchManager = FindObjectOfType<ResearchManager>();
        pointOfInterestManager = GameObject.FindObjectOfType<PointOfInterestManager>();
        Display(null, null);
    }

    // Update is called once per frame
    void Update()
    {
        if (displayedPOI != null && pointOfInterestManager.SelectedPointOfInterest != displayedPOI)
        {
            Display(pointOfInterestManager.SelectedPointOfInterest, null);
        }
    }

    public void Hide()
    {
        Display(null, null);
    }

    public void Display(PointOfInterest pointOfInterest, ConstructedBuilding existingBuilding) {
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
            filterButtonRow.gameObject.SetActive(false);
        }
        else
        {
            titleTMP.text = "Fund Project in" + (pointOfInterest.needsThe ? " the " : " ") + "<b>" + pointOfInterest.Name + "</b>";
            existingBuildingRow.gameObject.SetActive(false);
            filterButtonRow.gameObject.SetActive(true);
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

        var sortedConstructionOptions = (from b in constructionOptions
                                        orderby displayedPOI.CanPurchaseBuilding(b) == false
                                        select b).ToList();

        foreach (Building building in sortedConstructionOptions)
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
            newBuildingRow.SetState(this, building, researchManager.BuildingIsResearched(building), displayedPOI);
        }
    }

    public void TogglePauseConstructedBuildingIfAble()
    {
        displayedPOI.ToggleBuildingPauseIfAble(displayedExistingBuilding);
    }

    public void DecommissionConstructedBuilding()
    {
        Debug.Log("DecommissionConstructedBuilding");
        displayedPOI.DecommissionBuilding(displayedExistingBuilding);
        Hide();
    }
}
