using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourceManagerDebug : MonoBehaviour
{
    public int money = 1000;

    public int moneyProduction = 100;

    public int scienceProduction = 0;

    public int biomassConsumption = 0;

    public int biomassProduction = 0;

    public int energyConsumption = 0;

    public int energyProduction = 0;

    public int energyCleanProduction = 0;

    public int energyDirtyProduction = 0;

    public int steelConsumption = 0;

    public int steelProduction = 0;

    public int titaniumConsumption = 0;

    public int titaniumProduction = 0;

    public int currentSeaLevels = 0;
    public int maxSeaLevels = 10;
    public int minSeaLevels = 0;
    public int seaLevelPressure = 0;

    public int currentBiodiversity = 10;
    public int minBiodiversity = 0;
    public int maxBiodiversity = 10;
    public int biodiversityPressure = 0;

    private GameResourceManager _gm;

    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _gm.SetDebugValues(this);
    }
}
