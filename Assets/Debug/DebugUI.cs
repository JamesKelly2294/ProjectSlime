using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    private GameResourceManager rm;
    private TurnManager tm;

    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI turnText;

    // Start is called before the first frame update
    void Start()
    {
        rm = GameObject.FindObjectOfType<GameResourceManager>();
        tm = GameObject.FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        resourceText.text = string.Format("Money {0}\nEnergy {1}/{2}\nSteel {3}/{4}\nTitanium {5}/{6}\nBiomass {7}/{8}\nResearch {9:+#;-#;+0}",
            rm.money,
            rm.energyConsumption, rm.energyProduction,
            rm.steelConsumption, rm.steelProduction,
            rm.titaniumConsumption, rm.titaniumProduction,
            rm.biomassConsumption, rm.biomassProduction,
            rm.scienceProduction);

        turnText.text = string.Format("{0} A.D.\n{1} Years Until Extinction",
            tm.CurrentTurnAsYear, tm.TurnsUntilGameEndAsYear);
    }
}
