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
    }

    public void PauseConstructedBuilding()
    {

    }

    public void DecommissionConstructedBuilding()
    {

    }
}
