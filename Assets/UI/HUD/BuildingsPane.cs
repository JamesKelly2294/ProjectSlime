using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingsPane : MonoBehaviour
{

    public TextMeshProUGUI titleTMP, localPopulationTMP;

    public GameObject buildingsList, header, content;

    public GameObject buildingPrefab, newButtonPrefab;

    private BuildingManager buildingManager;
    private NewBuildingPane newBuildingPane;
    private PointOfInterestManager pointOfInterestManager;
    private PointOfInterest displayedPOI = null;

    // Start is called before the first frame update
    void Start()
    {
        newBuildingPane = FindObjectOfType<NewBuildingPane>();
        buildingManager = GameObject.FindObjectOfType<BuildingManager>();
        pointOfInterestManager = GameObject.FindObjectOfType<PointOfInterestManager>();
        Display(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (pointOfInterestManager.SelectedPointOfInterest != displayedPOI) {
            Display(pointOfInterestManager.SelectedPointOfInterest);
        }
    }

    void Display(PointOfInterest pointOfInterest) {
        displayedPOI = pointOfInterest;
        if (pointOfInterest == null) {
            header.SetActive(false);
            content.SetActive(false);
            return;
        } else {
            header.SetActive(true);
            content.SetActive(true);
        }

        titleTMP.text = "Projects in" + (pointOfInterest.needsThe ? " the " : " ") + "<b>" + pointOfInterest.Name + "</b>";
        localPopulationTMP.text = pointOfInterest.AvailablePopulation + " / " + pointOfInterest.TotalPopulation;
        
        // Add the list of buildings
        foreach(Transform child in buildingsList.transform) {
            Destroy(child.gameObject);
        }
        foreach(ConstructedBuilding constructedBuilding in pointOfInterest.ConstructedBuildings) {
            GameObject gameObject = GameObject.Instantiate(buildingPrefab, buildingsList.transform);
            BuildingRow buildingRow = gameObject.GetComponent<BuildingRow>();
            buildingRow.SetConstructedBuilding(constructedBuilding);
            buildingRow.GetComponent<StandardButton>().onClick.AddListener(delegate { this.OpenBuildMenu(buildingRow); });
        }

        var newButtonGO = GameObject.Instantiate(newButtonPrefab, buildingsList.transform);
        newButtonGO.GetComponent<StandardButton>().onClick.AddListener(delegate { this.OpenBuildMenu(null); });
    }

    public void OpenBuildMenu(BuildingRow sender)
    {
        Debug.Log("OpenBuildMenu " + sender);
        if (sender)
        {
            newBuildingPane.Display(displayedPOI, sender.constructedBuilding);
        }
        else
        {
            newBuildingPane.Display(displayedPOI, null);
        }
    }
}
