using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourceManager : MonoBehaviour
{    
    public int money { get; private set; } = 1000;

    public int moneyProduction { get; private set; } = 100;

    public int scienceProduction { get; private set; } = 0;

    public int biomassConsumption { get; private set; } = 0;

    public int biomassProduction { get; private set; } = 0;

    public int energyConsumption { get; private set; } = 0;

    public int energyProduction { get; private set; } = 0;

    public int energyCleanProduction { get; private set; } = 0;

    public int energyDirtyProduction { get; private set; } = 0;

    public int steelConsumption { get; private set; } = 0;

    public int steelProduction { get; private set; } = 0;

    public int titaniumConsumption { get; private set; } = 0;

    public int titaniumProduction { get; private set; } = 0;

    public int currentSeaLevels { get; private set; } = 0;
    public int maxSeaLevels { get; private set; } = 10;
    public int minSeaLevels { get; private set; } = 0;

    public int currentBiodiversity { get; private set; } = 10;
    public int minBiodiversity { get; private set; } = 0;
    public int maxBiodiversity { get; private set; } = 10;

    public void SetDebugValues(GameResourceManagerDebug debug)
    {
        money = debug.money;
        moneyProduction = debug.moneyProduction;
        scienceProduction = debug.scienceProduction;
        biomassConsumption = debug.biomassConsumption;
        biomassProduction = debug.biomassProduction;
        energyConsumption = debug.energyConsumption;
        energyProduction = debug.energyProduction;
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
    public Sprite moneySprite, moneySprite2x, moneySprite3x;
    public Sprite scienceSprite, scienceSprite2x, scienceSprite3x;
    public Sprite steelSprite, steelSprite2x, steelSprite3x;
    public Sprite titaniumSprite, titaniumSprite2x, titaniumSprite3x;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
