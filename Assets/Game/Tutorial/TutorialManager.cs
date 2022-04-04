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
    }

    public void BeginTutorialSequenceThree()
    {
        Debug.Log("BeginTutorialSequenceThree");
    }
}
