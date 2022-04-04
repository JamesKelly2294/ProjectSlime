using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExistingBuildingRow : MonoBehaviour
{
    public Image image;
    public EffectList effectList;

    public TextMeshProUGUI flavorText;

    public ConstructedBuilding constructedBuilding;

    public StandardButton DecommissionButton;
    public StandardButton PauseButton;

    // This should really all be on a pause button script...
    public Color PauseColor;
    public Color ResumeColor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetConstructedBuilding(ConstructedBuilding constructedBuilding)
    {
        this.constructedBuilding = constructedBuilding;
        Debug.Log("my constructed building is " + constructedBuilding.Building.Name);
        var building = constructedBuilding.Building;
        image.sprite = building.PreviewImage;
        flavorText.text = building.Description;
        effectList.DisplayEffects(building.SortedResourceEffects);

        SetupPauseButton();
    }

    public void SetupPauseButton()
    {
        if (constructedBuilding.Active)
        {
            PauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
            PauseButton.GetComponentInChildren<Image>().color = PauseColor;
            PauseButton.transform.Find("Image").gameObject.SetActive(true);
        }
        else
        {
            PauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Resume";
            PauseButton.GetComponentInChildren<Image>().color = ResumeColor;
            PauseButton.transform.Find("Image").gameObject.SetActive(false);
        }
    }
}
