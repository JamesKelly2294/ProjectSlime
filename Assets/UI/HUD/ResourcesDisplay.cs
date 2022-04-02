using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{

    public ResourceSummary money, energy, steel, titanium, biomass, science;

    private GameResourceManager gameResourceManager;

    // Start is called before the first frame update
    void Start()
    {
        gameResourceManager = GameObject.FindObjectOfType<GameResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        money.value.text = gameResourceManager.money.ToString("#,0") + "B";
        money.detail.text = gameResourceManager.moneyProduction.ToString("#,0") + "B per Year";
        
        energy.value.text = gameResourceManager.energyProduction.ToString() + " / " + gameResourceManager.energyProduction.ToString();
        if (gameResourceManager.energyProduction != 0) {
            energy.detail.text = Mathf.RoundToInt(((float)gameResourceManager.energyCleanProduction / (float)gameResourceManager.energyProduction) * 100).ToString() + "% Clean Energy";
            energy.detail.gameObject.SetActive(true);
        } else {
            energy.detail.gameObject.SetActive(false);
        }

        steel.value.text = gameResourceManager.steelConsumption.ToString() + " / " + gameResourceManager.steelProduction.ToString();
        titanium.value.text = gameResourceManager.titaniumConsumption.ToString() + " / " + gameResourceManager.titaniumProduction.ToString();
        biomass.value.text = gameResourceManager.biomassConsumption.ToString() + " / " + gameResourceManager.biomassProduction.ToString();
        science.value.text = gameResourceManager.scienceProduction.ToString();
    }
}
