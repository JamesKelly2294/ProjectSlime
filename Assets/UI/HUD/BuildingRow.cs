using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BuildingRow : MonoBehaviour
{

    public Image image;
    public TextMeshProUGUI nameTMP;
    public EffectList effectList;

    public TextMeshProUGUI flavorText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BuildingManager buildingManager = GameObject.FindObjectOfType<BuildingManager>();
        Building building = buildingManager.Buildings[15];
        SetBuilding(building);
    }

    void SetBuilding(Building building) {
        image.sprite = building.PreviewImage;
        nameTMP.text = building.Name;
        flavorText.text = building.Description;
        effectList.DisplayEffects(building.SortedResourceEffects);
    }
}
