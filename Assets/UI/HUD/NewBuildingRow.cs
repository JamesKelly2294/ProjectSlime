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

    private Building _building;
    private PointOfInterest _poi;
    private NewBuildingPane _newBuildingPane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetState(NewBuildingPane newBuildingPane, Building building, PointOfInterest poi) {
        image.sprite = building.PreviewImage;
        nameTMP.text = building.Name;
        costTMP.text = Mathf.Abs(building.MoneyCost).ToString();
        flavorText.text = building.Description;
        effectList.DisplayEffects(building.SortedResourceEffects);

        _poi = poi;
        _building = building;
        _newBuildingPane = newBuildingPane;

        var button = GetComponent<StandardButton>();
        if (poi.CanPurchaseBuilding(building))
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void RequestConstruction()
    {
        if (_poi.TryPurchaseAndConstructBuilding(_building))
        {
            _newBuildingPane.Hide();
        }
    }
}
