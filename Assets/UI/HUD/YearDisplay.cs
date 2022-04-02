using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YearDisplay : MonoBehaviour
{

    public TextMeshProUGUI currentYear, yearsRemaing;

    private TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentYear.text = turnManager.CurrentTurnAsYear.ToString();
        yearsRemaing.text = turnManager.TurnsUntilGameEndAsYear.ToString();
    }
}
