using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourceManager : MonoBehaviour
{    
    public int money { get; private set; } = 2500;

    public int moneyProduction { get; private set; } = 100;

    public int moneyConsumption { get; private set; } = 0;

    public int NetMoneyProduction { get { return moneyProduction - moneyConsumption; } }

    public int science { get; private set; } = 0;
    public int scienceProduction { get; private set; } = 0;
    public int scienceConsumption { get; private set; } = 0;
    public int NetScienceProduction { get { return scienceProduction - scienceConsumption; } }

    public int biomassConsumption { get; private set; } = 0;

    public int biomassProduction { get; private set; } = 0;

    public int NetBiomassProduction { get { return biomassProduction - biomassConsumption; } }

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

    public int NetEnergyProduction { get { return energyProduction - energyConsumption; } }

    public int energyCleanProduction { get; private set; } = 0;

    public int energyDirtyProduction { get; private set; } = 0;

    public int steelConsumption { get; private set; } = 0;

    public int steelProduction { get; private set; } = 0;

    public int NetSteelProduction { get { return steelProduction - steelConsumption; } }

    public int titaniumConsumption { get; private set; } = 0;

    public int titaniumProduction { get; private set; } = 0;

    public int NetTitaniumProduction { get { return titaniumProduction - titaniumConsumption; } }

    public int currentSeaLevels { get; private set; } = 0;
    public int maxSeaLevels { get; private set; } = 10;
    public int minSeaLevels { get; private set; } = 0;

    public int currentBiodiversity { get; private set; } = 10;
    public int minBiodiversity { get; private set; } = 0;
    public int maxBiodiversity { get; private set; } = 10;

    public int timeToExtinction { get; private set; } = 51;

    private BuildingManager _bm;
    private PointOfInterestManager _poim;
    private ClimateEventManager _em;
    private TurnManager _tm;

    public void SetDebugValues(GameResourceManagerDebug debug)
    {
        if (debug == null) { return; }
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
        currentBiodiversity = debug.currentBiodiversity;
        minBiodiversity = debug.minBiodiversity;
        maxBiodiversity = debug.maxBiodiversity;
    }

    public int TurnsToExtinction
    {
        get
        {
            return timeToExtinction;
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
    public Sprite biodiveristySprite, biodiveristySprite2x, biodiveristySprite3x;
    public Sprite biodiveristyPreasureSprite, biodiveristyPreasureSprite2x, biodiveristyPreasureSprite3x;
    public Sprite seaLevelSprite, seaLevelSprite2x, seaLevelSprite3x;
    public Sprite seaLevelPreasureSprite, seaLevelPreasureSprite2x, seaLevelPreasureSprite3x;
    public Sprite timeToExtictionSprite, timeToExtictionSprite2x, timeToExtictionSprite3x;

    public int GetValueForResourceType(ResourceType resourceType, ClimateEventRequirement.ResourceMode mode, bool isOneShot)
    {
        if (isOneShot)
        {
            switch(resourceType)
            {
                case ResourceType.Money:
                    return money;
                case ResourceType.Biodiversity:
                    return currentBiodiversity;
                case ResourceType.SeaLevel:
                    return currentSeaLevels;
                case ResourceType.Pop:
                    break;
                // Add time to extiction here.
            }
            return -1337; // user error
        }

        var isProduction = mode == ClimateEventRequirement.ResourceMode.Production;
        switch (resourceType)
        {
            case ResourceType.Energy:
                return isProduction ? energyCleanProduction + energyDirtyProduction : energyCleanConsumption + energyDirtyConsumption;
            case ResourceType.CleanEnergy:
                return isProduction ? energyCleanProduction : energyCleanConsumption;
            case ResourceType.DirtyEnergy:
                return isProduction ? energyDirtyProduction : energyDirtyConsumption;
            case ResourceType.LowTechMat:
                return isProduction ? steelProduction : steelConsumption;
            case ResourceType.HighTechMat:
                return isProduction ? titaniumProduction : titaniumConsumption;
            case ResourceType.Biomass:
                return isProduction ? biomassProduction : biomassConsumption;
            case ResourceType.Research:
                return isProduction ? scienceProduction : scienceConsumption;
        }
        return -1337; // user error
    }

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
            case ResourceType.Biodiversity:
                return new Sprite[] {biodiveristySprite, biodiveristySprite2x, biodiveristySprite3x};
            case ResourceType.BiodiversityPressure:
                return new Sprite[] {biodiveristyPreasureSprite, biodiveristyPreasureSprite2x, biodiveristyPreasureSprite3x};
            case ResourceType.SeaLevel:
                return new Sprite[] {seaLevelSprite, seaLevelSprite2x, seaLevelSprite3x};
            case ResourceType.SeaLevelPressure:
                return new Sprite[] {seaLevelPreasureSprite, seaLevelPreasureSprite2x, seaLevelPreasureSprite3x};
            case ResourceType.TimeToExtiction:
                return new Sprite[] {timeToExtictionSprite, timeToExtictionSprite2x, timeToExtictionSprite3x};
            default:
                return new Sprite[] {};
        }
    }

    public bool ResourceManagerApprovesNextTurn()
    {
        return  NetEnergyProduction >= 0 
                && NetSteelProduction >= 0 
                && NetTitaniumProduction >= 0 
                && NetBiomassProduction >= 0;
    }

    public Sprite GetSpriteForResourceWithOneIndexedScale(ResourceType resourceType, int oneIndexedScale) {
        return GetSpritesForResource(resourceType)[oneIndexedScale - 1];
    }

    public bool CanAffordResourceConsumption(ResourceType type, int amount)
    {
        amount = Mathf.Abs(amount);
        switch (type)
        {
            case ResourceType.Energy:
                return NetEnergyProduction - amount >= 0;
            case ResourceType.LowTechMat:
                return NetSteelProduction - amount >= 0;
            case ResourceType.HighTechMat:
                return NetTitaniumProduction - amount >= 0;
            case ResourceType.Biomass:
                return NetBiomassProduction - amount >= 0;
            default:
                return true;
        }
    }

    public bool CanAffordResourceOneShot(ResourceType type, int amount)
    {
        amount = Mathf.Abs(amount);
        switch (type)
        {
            case ResourceType.Money:
                return money - amount >= 0;
            case ResourceType.Research:
                return science - amount >= 0;
            default:
                return true;
        }
    }

    public void SpendMoney(int amount)
    {
        amount = Mathf.Abs(amount);

        money -= amount;
    }

    public void SpendResearch(int amount)
    {
        amount = Mathf.Abs(amount);

        science -= amount;
    }

    public void ProcessEndOfTurnResourceUpdates()
    {
        money += NetMoneyProduction;
        science += NetScienceProduction;
    }

    public void CalculateResources()
    {
        moneyProduction = 0;
        moneyConsumption = 0;
        scienceProduction = 0;
        scienceConsumption = 0;
        biomassConsumption = 0;
        biomassProduction = 0;
        energyConsumption = 0;
        energyCleanProduction = 0;
        energyDirtyProduction = 0;
        steelConsumption = 0;
        steelProduction = 0;
        titaniumConsumption = 0;
        titaniumProduction = 0;
        currentSeaLevels = 0;
        currentBiodiversity = maxBiodiversity;

        List<ResourceEffect> activeResourceEffects = new List<ResourceEffect>();

        // Step one, calculate effects from all of our buildings
        foreach(var poi in _poim.PointsOfInterest)
        {
            foreach(var b in poi.ConstructedBuildings)
            {
                if (b.Active == false) { continue; }
                foreach(var effect in b.Building.ResourceEffects)
                {
                    activeResourceEffects.Add(effect);
                }
            }
        }


        // Step two, calculate effects from all events
        foreach(var decision in _em.ActiveClimateDecisions)
        {
            foreach (var effect in decision.choice.ResourceEffects)
            {
                activeResourceEffects.Add(effect);
            }
        }

        foreach (var decision in _em.InactiveClimateDecisions)
        {
            foreach (var effect in decision.choice.ResourceEffects)
            {
                if (effect.AffectedResource == ResourceType.Biodiversity || effect.AffectedResource == ResourceType.SeaLevel)
                {
                    activeResourceEffects.Add(effect);
                }
            }
        }

        Debug.Log("Latest Response:" + _em.LatestResponse);
        if(_em.LatestResponse != null) {
           foreach (var effect in _em.LatestResponse.ResourceEffects)
            {
                activeResourceEffects.Add(effect);
            }
        }

        // Step three, calculate stats from all active effects
        foreach(var effect in activeResourceEffects)
        {
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
                    if (effect.EffectAmount > 0)
                    {
                        scienceProduction += effect.EffectAmount;
                    }
                    else
                    {
                        scienceConsumption -= effect.EffectAmount;
                    }
                    break;
                case ResourceType.Pop:
                    break;
                case ResourceType.Biodiversity:
                    currentBiodiversity -= effect.EffectAmount;
                    break;
                case ResourceType.SeaLevel:
                    currentSeaLevels += effect.EffectAmount;
                    break;
            }
        }

        currentBiodiversity = Mathf.Min(Mathf.Max(currentBiodiversity, minBiodiversity), maxBiodiversity);
        currentSeaLevels = Mathf.Min(Mathf.Max(currentSeaLevels, minSeaLevels), maxSeaLevels);

        _tm.ResourcesChanged();
    }

    // Start is called before the first frame update
    void Start()
    {
        _bm = FindObjectOfType<BuildingManager>();
        _poim = FindObjectOfType<PointOfInterestManager>();
        _em = FindObjectOfType<ClimateEventManager>();
        _tm = FindObjectOfType<TurnManager>();
    }

    // Update is called once per framee
    void Update()
    {
        
    }

    public void EventSummoned(PubSubListenerEvent e)
    {
        if (!(e.value is ClimateEvent))
        {
            return;
        }

        var climateEvent = (ClimateEvent)e.value;

        var pubSubSender = GetComponent<PubSubSender>();
        foreach (var specialEffect in climateEvent.SpecialEffects)
        {
            pubSubSender.Publish("special.event." + specialEffect.Type);
        }
    }

    public void EventResponseSelected(PubSubListenerEvent e)
    {
        if(!(e.value is ClimateEventResponse))
        {
            return;
        }

        var response = (ClimateEventResponse)e.value;

        var pubSubSender = GetComponent<PubSubSender>();
        foreach(var specialEffect in response.SpecialEffects)
        {
            pubSubSender.Publish("special.event." + specialEffect.Type);
        }
    }
}
