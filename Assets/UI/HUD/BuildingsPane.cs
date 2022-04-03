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
    private PointOfInterestManager pointOfInterestManager;
    private PointOfInterest displayedPOI = null;

    // Start is called before the first frame update
    void Start()
    {
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

        titleTMP.text = "Build in" + (pointOfInterest.needsThe ? " the " : " ") + "<b>" + pointOfInterest.Name + "</b>";
        localPopulationTMP.text = pointOfInterest.ConsumedPopulation + " / " + pointOfInterest.TotalPopulation;
        
        // Add the list of buildings
        foreach(Transform child in buildingsList.transform) {
            Destroy(child.gameObject);
        }
        foreach(Building building in pointOfInterest.Buildings) {
            GameObject gameObject = GameObject.Instantiate(buildingPrefab, buildingsList.transform);
            BuildingRow buildingRow = gameObject.GetComponent<BuildingRow>();
            buildingRow.SetBuilding(building);
        }

        GameObject.Instantiate(newButtonPrefab, buildingsList.transform);
    }
}
