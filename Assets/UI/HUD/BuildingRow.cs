using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BuildingRow : MonoBehaviour
{

    public Image image;
    public Image pauseImage;
    public TextMeshProUGUI nameTMP;
    public EffectList effectList;
    public ConstructedBuilding constructedBuilding { get; protected set; }

    public TextMeshProUGUI flavorText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetConstructedBuilding(ConstructedBuilding constructedBuilding) {
        this.constructedBuilding = constructedBuilding;

        var building = constructedBuilding.Building;
        image.sprite = building.PreviewImage;
        nameTMP.text = building.Name;
        flavorText.text = building.Description;
        effectList.DisplayEffects(building.SortedResourceEffects);

        UpdatePauseImage();
    }

    public void UpdatePauseImage()
    {
        pauseImage.gameObject.SetActive(!constructedBuilding.Active);
    }
}
