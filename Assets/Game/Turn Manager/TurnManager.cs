using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameResourceManager _gm;

    public bool GameStarted { get; protected set; }

    public int CurrentTurn { get; private set; }

    public List<ClimateEvent> tutorialEvents, realEvents;

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
            int yearsRemaining = Mathf.Min(_gm.TurnsToExtinction - CurrentTurn);
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
        climateEventManager.AllClimateEvents = new List<ClimateEvent>();
        climateEventManager.AllClimateEvents.Add(tutorialEvents[0]);

        _gm.CalculateResources();
        climateEventManager.StepClimateState(CurrentTurn);
        PublishBeginTurnNotification();

        GameStarted = true;
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

    private void PrepareEventSystem() {
        // Make sure we onboard the player
        if (CurrentTurn < tutorialEvents.Count) {
            climateEventManager.AllClimateEvents = new List<ClimateEvent>();
            climateEventManager.AllClimateEvents.Add(tutorialEvents[CurrentTurn]);
        } else if (tutorialEvents.Count == CurrentTurn) {
            climateEventManager.AllClimateEvents = realEvents;
        }
        climateEventManager.StepClimateState(CurrentTurn);
    }

    private void AdvanceToNextTurn()
    {
        CurrentTurn++;

        PrepareEventSystem();
        _gm.CalculateResources();

        _gm.ProcessEndOfTurnResourceUpdates();

        userDidHandleEvent = false;
        PublishBeginTurnNotification();
        CheckAndSendValidNextTurnEvents();
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

    public void CheckAndSendValidNextTurnEvents() {
        if (GameStarted == false)
        {
            return;
        }

        if (!IsCurrentEventHandled()) {
            PublishForbidNextTurnNotification("Current Event not Handled");
            return;
        }

        if (!_gm.ResourceManagerApprovesNextTurn()) {
            PublishForbidNextTurnNotification("At least one resource over utilized!");
            return;
        }

        PublishAllowNextTurnNotification();
    }

    public void PublishBeginTurnNotification() {
        GetComponent<PubSubSender>().Publish("turn.begin");
    }

    public void PublishAllowNextTurnNotification() {
        Debug.Log("Next Turn Allowed!");
        GetComponent<PubSubSender>().Publish("turn.next.allow");
    }

    public void PublishForbidNextTurnNotification(string reason) {
        Debug.Log("Next Turn Forbidden:" + reason);
        GetComponent<PubSubSender>().Publish("turn.next.forbidden", reason);
    }

    private bool userDidHandleEvent = false;
    public bool IsCurrentEventHandled() {
        if (climateEventManager.CurrentClimateEvent == null) { return true; }
        if (climateEventManager.CurrentClimateEvent.Responses.Count == 0) { return true; }
        return userDidHandleEvent;
    }

    public void UserDidHandleEvent() {
        userDidHandleEvent = true;
        CheckAndSendValidNextTurnEvents();
    }

    public void ResourcesChanged() {
        CheckAndSendValidNextTurnEvents();
    }

}
