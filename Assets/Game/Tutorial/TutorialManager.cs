using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private StatsPane _statsPane;
    private BuildingsPane _buildingsPane;
    private NewBuildingPane _newBuildingPane;
    private ResourcesDisplay _resourcesDisplay;
    private YearDisplay _yearDisplay;
    private CameraInput _cameraInput;
    private MouseOrbit _mouseOrbit;

    private PointOfInterestManager _pointOfInterestManager;

    // Start is called before the first frame update
    void Start()
    {
        _statsPane = FindObjectOfType<StatsPane>();
        _buildingsPane = FindObjectOfType<BuildingsPane>();
        _newBuildingPane = FindObjectOfType<NewBuildingPane>();
        _resourcesDisplay = FindObjectOfType<ResourcesDisplay>();
        _yearDisplay = FindObjectOfType<YearDisplay>();
        _cameraInput = FindObjectOfType<CameraInput>();
        _mouseOrbit = FindObjectOfType<MouseOrbit>();
        _pointOfInterestManager = FindObjectOfType<PointOfInterestManager>();

        BeginTutorialSequenceZero();
    }

    public void SkipTutorial()
    {

    }

    void SetYearsUntilExtinctionVisible(bool isVisible)
    {
        _yearDisplay.yearsRemaining.gameObject.SetActive(isVisible);
        _yearDisplay.yearsUntilExtinctionLabel.gameObject.SetActive(isVisible);
    }

    public void BeginTutorialSequenceZero()
    {
        _statsPane.gameObject.SetActive(false);
        _buildingsPane.gameObject.SetActive(false);
        _newBuildingPane.gameObject.SetActive(false);
        _resourcesDisplay.gameObject.SetActive(false);

        SetYearsUntilExtinctionVisible(false);
    }

    public void BeginTutorialSequenceOne()
    {
        Debug.Log("BeginTutorialSequenceOne");

        SetYearsUntilExtinctionVisible(true);
    }

    public void BeginTutorialSequenceTwo()
    {
        Debug.Log("BeginTutorialSequenceTwo");

        _statsPane.gameObject.SetActive(true);
    }

    public void BeginTutorialSequenceThree()
    {
        Debug.Log("BeginTutorialSequenceThree");

        _resourcesDisplay.gameObject.SetActive(true);
    }

    public void BeginTutorialSequenceFour()
    {
        Debug.Log("BeginTutorialSequenceFour");

        _buildingsPane.gameObject.SetActive(true);
        _newBuildingPane.gameObject.SetActive(true);

        PointOfInterest northAmericaPOI = null;
        foreach(var poi in _pointOfInterestManager.PointsOfInterest)
        {
            if (poi.Name.Contains("North America"))
            {
                northAmericaPOI = poi;
                break;
            }
        }
        if (northAmericaPOI == null)
        {
            northAmericaPOI = _pointOfInterestManager.PointsOfInterest[0];
        }

        _pointOfInterestManager.SetSelectedPointOfInterest(northAmericaPOI);
    }
}
