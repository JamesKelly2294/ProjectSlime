using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NewBuildingRow : MonoBehaviour
{

    public Image image;
    public TextMeshProUGUI nameTMP;
    public TextMeshProUGUI costTMP;
    public EffectList effectList;

    public TextMeshProUGUI flavorText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBuilding(Building building) {
        image.sprite = building.PreviewImage;
        nameTMP.text = building.Name;
        costTMP.text = Mathf.Abs(building.MoneyCost).ToString();
        flavorText.text = building.Description;
        effectList.DisplayEffects(building.SortedResourceEffects);
    }
}
