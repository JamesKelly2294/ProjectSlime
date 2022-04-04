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

    public Image resourceCostImage;
    public Sprite moneySprite;
    public Sprite researchSprite;

    public TextMeshProUGUI flavorText;

    public bool buildingCanBePurchased;

    private bool _isResearched;
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

    public void SetState(NewBuildingPane newBuildingPane, Building building, bool isResearched, PointOfInterest poi) {
        image.sprite = building.PreviewImage;
        nameTMP.text = building.Name;
        costTMP.text = Mathf.Abs(!isResearched ? building.ResearchCost : building.MoneyCost).ToString();
        resourceCostImage.sprite = !isResearched ? researchSprite : moneySprite;
        flavorText.text = building.Description;
        effectList.DisplayEffects(building.SortedResourceEffects);

        _poi = poi;
        _building = building;
        _newBuildingPane = newBuildingPane;
        _isResearched = isResearched;

        var button = GetComponent<StandardButton>();
        if (poi.CanPurchaseBuilding(building))
        {
            buildingCanBePurchased = true;
        }
        else
        {
            buildingCanBePurchased = false;
        }

        button.interactable = buildingCanBePurchased;
    }

    public void RequestConstruction()
    {
        var wasResearched = _isResearched;
        if (_poi.TryPurchaseAndConstructBuilding(_building))
        {
            if (wasResearched)
            {
                _newBuildingPane.Hide();
            }
        }
    }
}
