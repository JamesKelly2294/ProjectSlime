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
        money.detail.text = (gameResourceManager.NetMoneyProduction).ToString("#,0") + "B per Year";

        science.value.text = gameResourceManager.science.ToString("#,0");
        science.detail.text = (gameResourceManager.NetScienceProduction).ToString("#,0") + " per Year";

        cleanEnergy.value.text = (gameResourceManager.energyCleanProduction - gameResourceManager.energyCleanConsumption) + " / " + gameResourceManager.energyCleanProduction;
        dirtyEnergy.value.text = (gameResourceManager.energyDirtyProduction - gameResourceManager.energyDirtyConsumption) + " / " + gameResourceManager.energyDirtyProduction;

        steel.value.text = (gameResourceManager.steelProduction - gameResourceManager.steelConsumption).ToString() + " / " + gameResourceManager.steelProduction.ToString();
        titanium.value.text = (gameResourceManager.titaniumProduction - gameResourceManager.titaniumConsumption).ToString() + " / " + gameResourceManager.titaniumProduction.ToString();
        biomass.value.text = (gameResourceManager.biomassProduction - gameResourceManager.biomassConsumption).ToString() + " / " + gameResourceManager.biomassProduction.ToString();

        globalPopulation.value.text = pointOfInterestManager.GlobalAvailablePopulation + " / " + pointOfInterestManager.GlobalTotalPopulation;
    }
}
