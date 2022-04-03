using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourceManager : MonoBehaviour
{    
    public int money { get; private set; } = 1000;

    public int moneyProduction { get; private set; } = 100;

    public int moneyConsumption { get; private set; } = 0;

    public int scienceProduction { get; private set; } = 0;

    public int biomassConsumption { get; private set; } = 0;

    public int biomassProduction { get; private set; } = 0;

    public int energyConsumption { get; private set; } = 0;

    public int energyCleanConsumption
    {
        get {
            var cleanEnergyNeeded = energyConsumption;
            if (cleanEnergyNeeded <= energyCleanProduction)
            {
                return cleanEnergyNeeded;
            }
            else
            {
                return energyCleanProduction;
            }
        }
    }

    public int energyDirtyConsumption {
        get {
            return energyConsumption - energyCleanConsumption;
        }
    }

    public int energyProduction { get { return energyCleanProduction + energyDirtyProduction; } }

    public int energyCleanProduction { get; private set; } = 0;

    public int energyDirtyProduction { get; private set; } = 0;

    public int steelConsumption { get; private set; } = 0;

    public int steelProduction { get; private set; } = 0;

    public int titaniumConsumption { get; private set; } = 0;

    public int titaniumProduction { get; private set; } = 0;

    public int currentSeaLevels { get; private set; } = 0;
    public int maxSeaLevels { get; private set; } = 10;
    public int minSeaLevels { get; private set; } = 0;
    // the higher the pressure, the greater the likelyhood of receiving a
    // sea level rise event
    public int seaLevelPressure { get; private set; } = 0;

    public int currentBiodiversity { get; private set; } = 10;
    public int minBiodiversity { get; private set; } = 0;
    public int maxBiodiversity { get; private set; } = 10;
    // the higher the pressure, the greater the likelyhood of receiving a
    // biodiversity drop event
    public int biodiversityPressure { get; private set; } = 0;

    private BuildingManager _bm;
    private PointOfInterestManager _poim;
    private ClimateEventManager _em;

    public void SetDebugValues(GameResourceManagerDebug debug)
    {
        money = debug.money;
        moneyProduction = debug.moneyProduction;
        scienceProduction = debug.scienceProduction;
        biomassConsumption = debug.biomassConsumption;
        biomassProduction = debug.biomassProduction;
        energyConsumption = debug.energyConsumption;
        energyCleanProduction = debug.energyCleanProduction;
        energyDirtyProduction = debug.energyDirtyProduction;
        steelConsumption = debug.steelConsumption;
        steelProduction = debug.steelProduction;
        titaniumConsumption = debug.titaniumConsumption;
        titaniumProduction = debug.titaniumProduction;
        currentSeaLevels = debug.currentSeaLevels;
        maxSeaLevels = debug.maxSeaLevels;
        minSeaLevels = debug.minSeaLevels;
        seaLevelPressure = debug.seaLevelPressure;
        currentBiodiversity = debug.currentBiodiversity;
        minBiodiversity = debug.minBiodiversity;
        maxBiodiversity = debug.maxBiodiversity;
        biodiversityPressure = debug.biodiversityPressure;
    }

    public int BiodiversityYearsRemaining
    {
        get
        {
            return 5;
        }
    }

    public int SeaLevelsYearsRemaining
    {
        get
        {
            return 5;
        }
    }

    public Sprite biomassSprite, biomassSprite2x, biomassSprite3x;
    public Sprite energySprite, energySprite2x, energySprite3x;
    public Sprite dirtyEnergySprite, dirtyEnergySprite2x, dirtyEnergySprite3x;
    public Sprite cleanEnergySprite, cleanEnergySprite2x, cleanEnergySprite3x;
    public Sprite moneySprite, moneySprite2x, moneySprite3x;
    public Sprite scienceSprite, scienceSprite2x, scienceSprite3x;
    public Sprite steelSprite, steelSprite2x, steelSprite3x;
    public Sprite titaniumSprite, titaniumSprite2x, titaniumSprite3x;
    public Sprite localPopulationSprite, localPopulationSprite2x, localPopulationSprite3x;
    public Sprite globalPopulationSprite, globalPopulationSprite2x, globalPopulationSprite3x;

    public Sprite[] GetSpritesForResource(ResourceType resourceType) {
        switch (resourceType) {
            case ResourceType.Biomass:
                return new Sprite[] {biomassSprite, biomassSprite2x, biomassSprite3x};
            case ResourceType.Energy:
                return new Sprite[] {energySprite, energySprite2x, energySprite3x};
            case ResourceType.DirtyEnergy:
                return new Sprite[] {dirtyEnergySprite, dirtyEnergySprite2x, dirtyEnergySprite3x};
            case ResourceType.CleanEnergy:
                return new Sprite[] {cleanEnergySprite, cleanEnergySprite2x, cleanEnergySprite3x};
            case ResourceType.Money:
                return new Sprite[] {moneySprite, moneySprite2x, moneySprite3x};
            case ResourceType.Research:
                return new Sprite[] {scienceSprite, scienceSprite2x, scienceSprite3x};
            case ResourceType.LowTechMat:
                return new Sprite[] {steelSprite, steelSprite2x, steelSprite3x};
            case ResourceType.HighTechMat:
                return new Sprite[] {titaniumSprite, titaniumSprite2x, titaniumSprite3x};
            case ResourceType.Pop:
                return new Sprite[] {localPopulationSprite, localPopulationSprite2x, localPopulationSprite3x};
            default:
                return new Sprite[] {};
        }
    }

    public Sprite GetSpriteForResourceWithOneIndexedScale(ResourceType resourceType, int oneIndexedScale) {
        return GetSpritesForResource(resourceType)[oneIndexedScale - 1];
    }

    public void CalculateResources()
    {
        Debug.Log("CalculateResources");
        moneyProduction = 0;
        scienceProduction = 0;
        biomassConsumption = 0;
        biomassProduction = 0;
        energyConsumption = 0;
        energyCleanProduction = 0;
        energyDirtyProduction = 0;
        steelConsumption = 0;
        steelProduction = 0;
        titaniumConsumption = 0;
        titaniumProduction = 0;
        seaLevelPressure = 0;
        biodiversityPressure = 0;

        List<ResourceEffect> activeResourceEffects = new List<ResourceEffect>();

        // Step one, calculate effects from all of our buildings
        foreach(var poi in _poim.PointsOfInterest)
        {
            foreach(var b in poi.Buildings)
            {
                foreach(var effect in b.ResourceEffects)
                {
                    activeResourceEffects.Add(effect);
                }
            }
        }


        // Step two, calculate effects from all active events
        foreach(var decision in _em.ActiveClimateDecisions)
        {
            foreach (var effect in decision.choice.ResourceEffects)
            {
                activeResourceEffects.Add(effect);
            }
        }


        // Step three, calculate stats from all active effects
        foreach(var effect in activeResourceEffects)
        {
            Debug.Log("Calculating effect " + effect.AffectedResource + " , " + effect.EffectAmount);
            switch (effect.AffectedResource)
            {
                case ResourceType.Money:
                    if (effect.EffectAmount > 0)
                    {
                        moneyProduction += effect.EffectAmount;
                    }
                    else
                    {
                        moneyConsumption -= effect.EffectAmount;
                    }
                    break;
                case ResourceType.Energy:
                    energyConsumption -= effect.EffectAmount;
                    break;
                case ResourceType.CleanEnergy:
                    energyCleanProduction += effect.EffectAmount;
                    break;
                case ResourceType.DirtyEnergy:
                    energyDirtyProduction += effect.EffectAmount;
                    break;
                case ResourceType.LowTechMat:
                    if (effect.EffectAmount > 0)
                    {
                        steelProduction += effect.EffectAmount;
                    }
                    else
                    {
                        steelConsumption -= effect.EffectAmount;
                    }
                    break;
                case ResourceType.HighTechMat:
                    if (effect.EffectAmount > 0)
                    {
                        titaniumProduction += effect.EffectAmount;
                    }
                    else
                    {
                        titaniumConsumption -= effect.EffectAmount;
                    }
                    break;
                case ResourceType.Biomass:
                    if (effect.EffectAmount > 0)
                    {
                        biomassProduction += effect.EffectAmount;
                    }
                    else
                    {
                        biomassConsumption -= effect.EffectAmount;
                    }
                    break;
                case ResourceType.Research:
                    scienceProduction += effect.EffectAmount;
                    break;
                case ResourceType.Pop:
                    break;
                case ResourceType.Biodiversity:
                    currentBiodiversity += effect.EffectAmount;
                    break;
                case ResourceType.BiodiversityPressure:
                    biodiversityPressure += effect.EffectAmount;
                    break;
                case ResourceType.SeaLevel:
                    currentSeaLevels += effect.EffectAmount;
                    break;
                case ResourceType.SeaLevelPressure:
                    seaLevelPressure += effect.EffectAmount;
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _bm = FindObjectOfType<BuildingManager>();
        _poim = FindObjectOfType<PointOfInterestManager>();
        _em = FindObjectOfType<ClimateEventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
