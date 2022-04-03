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

    private ClimateEventManager climateEventManager;

    // Start is called before the first frame update
    void Start()
    {
        _gm = GameObject.FindObjectOfType<GameResourceManager>();
        climateEventManager = GameObject.FindObjectOfType<ClimateEventManager>();

        StartGame();
    }

    private void StartGame()
    {
        _gm.CalculateResources();
        climateEventManager.StepClimateState(CurrentTurn);
        PublishBeginTurnNotification();
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
        climateEventManager.StepClimateState(CurrentTurn);

        CurrentTurn++;

        _gm.CalculateResources();

        PublishBeginTurnNotification();
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

    public void PublishBeginTurnNotification() {
        GetComponent<PubSubSender>().Publish("turn.begin");
    }
}
