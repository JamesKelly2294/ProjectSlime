using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{

    public ResourceSummary money, cleanEnergy, dirtyEnergy, steel, titanium, biomass, science, globalPopulation;

    private GameResourceManager gameResourceManager;
    private PointOfInterestManager pointOfInterestManager;

    // Start is called before the first frame update
    void Start()
    {
        gameResourceManager = GameObject.FindObjectOfType<GameResourceManager>();
        pointOfInterestManager = GameObject.FindObjectOfType<PointOfInterestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        money.value.text = gameResourceManager.money.ToString("#,0") + "B";
        money.detail.text = gameResourceManager.moneyProduction.ToString("#,0") + "B per Year";
        
        cleanEnergy.value.text = gameResourceManager.energyCleanConsumption + " / " + gameResourceManager.energyCleanProduction;
        dirtyEnergy.value.text = gameResourceManager.energyDirtyConsumption + " / " + gameResourceManager.energyDirtyProduction;

        steel.value.text = gameResourceManager.steelConsumption.ToString() + " / " + gameResourceManager.steelProduction.ToString();
        titanium.value.text = gameResourceManager.titaniumConsumption.ToString() + " / " + gameResourceManager.titaniumProduction.ToString();
        biomass.value.text = gameResourceManager.biomassConsumption.ToString() + " / " + gameResourceManager.biomassProduction.ToString();
        science.value.text = gameResourceManager.scienceProduction.ToString();

        globalPopulation.value.text = pointOfInterestManager.GlobalConsumedPopulation + " / " + pointOfInterestManager.GlobalTotalPopulation;
    }
}
