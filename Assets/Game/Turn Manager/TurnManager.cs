using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameResourceManager _gm;

    public int CurrentTurn { get; private set; }

    public int CurrentTurnAsYear
    {
        get
        {
            return (2050 + CurrentTurn);
        }
    }

    public int TurnsUntilGameEndAsYear
    {
        get
        {
            int yearsRemaining = Mathf.Min(_gm.BiodiversityYearsRemaining, _gm.SeaLevelsYearsRemaining);
            return yearsRemaining;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gm = GameObject.FindObjectOfType<GameResourceManager>();

        StartGame();
    }

    private void StartGame()
    {
        _gm.CalculateResources();
    }

    private void ProcessEndOfTurnEvents()
    {

    }

    private bool EndGameConditionIsReached()
    {
        return false;
    }

    private void EndGame()
    {

    }

    private void AdvanceToNextTurn()
    {
        CurrentTurn++;

        _gm.CalculateResources();
    }

    public void EndTurn()
    {
        ProcessEndOfTurnEvents();

        if (EndGameConditionIsReached())
        {
            EndGame();
        }
        else
        {
            AdvanceToNextTurn();
        }
    }
}
