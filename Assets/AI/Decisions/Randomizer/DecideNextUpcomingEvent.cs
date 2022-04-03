using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class DecideNextUpcomingEvent : MonoBehaviour, IDecision
{
    public ClimateEventManager climateEventManager;
    
    private BinaryTree<DecisionNode> _decisionTree;
    private RandomizerAgent _agent;
    private GameResourceManager _grm;

    public AnimationCurve BiodiversitySeaLevelProbability;

    public void Start()
    {
        _agent = FindObjectOfType<RandomizerAgent>();
        _grm = FindObjectOfType<GameResourceManager>();
        var nodes = new BinaryTreeNode<DecisionNode>[] {
                new BinaryTreeNode<DecisionNode>(AnyCurrentEventFollowOnsInBudget(), false),
                new BinaryTreeNode<DecisionNode>(PickRandomFollowOnsInBudget(), true), // left child of AnyCurrentEventFollowOnsInBudget
                new BinaryTreeNode<DecisionNode>(AnyEventsWithResourceTypeInBudgetToTarget(ResourceType.BiodiversityPressure, ResourceType.SeaLevelPressure), false), // right child of AnyCurrentEventFollowOnsInBudget
                new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // left child of PickRandomFollowOnsInBudget
                new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // right child of PickRandomFollowOnsInBudget
                new BinaryTreeNode<DecisionNode>(PickEventWithResourceTypeInBudget(ResourceType.BiodiversityPressure, ResourceType.SeaLevelPressure), true), // left child of AnyEventsWithResourceTypeInBudgetToTarget
                new BinaryTreeNode<DecisionNode>(PickRandomCardInBudget(), true) // right child of AnyEventsWithResourceTypeInBudgetToTarget
        };
        _decisionTree = new BinaryTree<DecisionNode>(nodes);
    }

    public void Decide()
    {
        bool isDecided = false;
        int index = 0;
        do
        {
            var node = _decisionTree[index];
            var result = node.data.evaluation();
            switch (node.data.type)
            {
                case DecisionNodeType.Decision:
                    Debug.Log("Evaluating: Survey says " + (result ? "yes" : "no"));
                    index = result ? _decisionTree.LeftIndex(index) : _decisionTree.RightIndex(index);
                    break;
                case DecisionNodeType.Action:
                    isDecided = true; 
                    break;
            }
        } while (!isDecided);
    }

    public DecisionNode AnyCurrentEventFollowOnsInBudget()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Evaluating: Do we have any follow-on cards available and in budget?");
            var followOnEvents = from e in climateEventManager.EventsInBudget(_agent.Budget)
                                 where e.FollowOnEvents.Contains(climateEventManager.CurrentClimateEvent)
                                 select e;
            var result = followOnEvents.Count() > 0;
            return result;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Decision);
    }

    public DecisionNode PickRandomFollowOnsInBudget()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Action: Pick a random follow-on card that's in budget.");
            var followOnEvents = from e in climateEventManager.EventsInBudget(_agent.Budget)
                                 where e.FollowOnEvents.Contains(climateEventManager.CurrentClimateEvent)
                                 select e;
            var index = UnityEngine.Random.Range(0, followOnEvents.Count() - 1);
            SelectNextEvent(new List<ClimateEvent>(followOnEvents)[index]);
            return false; // not actually used for anything. cleanup for later.
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public DecisionNode AnyEventsWithResourceTypeInBudgetToTarget(params ResourceType[] targets)
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Evaluating: Are there any cards with " + targets.ToString() + " that we can target?");
            var chance = BiodiversitySeaLevelProbability.Evaluate(_grm.biodiversityPressure / 10.0f);
            if (UnityEngine.Random.Range(0.0f, 1.0f) < chance)
            {
                return false;
            }
            return climateEventManager.EventsWithResourceTypeInResponse(_agent.Budget, targets).Count() > 0;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Decision);
    }

    public DecisionNode PickEventWithResourceTypeInBudget(params ResourceType[] targets)
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Action: Target event with resource type " + targets.ToString());
            var candidates = climateEventManager.EventsWithResourceTypeInResponse(_agent.Budget, targets);
            var index = UnityEngine.Random.Range(0, candidates.Count());
            SelectNextEvent(new List<ClimateEvent>(candidates)[index]);
            return false;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public DecisionNode PickRandomCardInBudget()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Assert(GuardClauses.IsNotEmpty(climateEventManager.AllClimateEvents), "No climate events to work with!");
            Debug.Log("Decision: Pick a random card thats in budget.");
            var eventsInBudget = climateEventManager.EventsInBudget(_agent.Budget);
            var index = UnityEngine.Random.Range(0, eventsInBudget.Count() - 1);
            SelectNextEvent(new List<ClimateEvent>(eventsInBudget)[index]);
            return false; // not actually used for anything. cleanup for later.
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public void SelectNextEvent(ClimateEvent ce)
    {
        climateEventManager.NextClimateEvent = ce;

        // TODO: Figure out what to actually do with the budget.
        _agent.Budget = _agent.Budget - (int)(climateEventManager.NextClimateEvent.AgentCost * 100) + _agent.BudgetProduction;

        // Trigger the UI to do stuff?
    }
}

[CustomEditor(typeof(DecideNextUpcomingEvent))]
public class DecideNextUpcomingEventEditor : Editor
{
    private DecideNextUpcomingEvent script;
    private GameResourceManager grm;

    private void OnEnable()
    {
        script = (DecideNextUpcomingEvent)target;
        grm = FindObjectOfType<GameResourceManager>();
        var grmDebug = FindObjectOfType<GameResourceManagerDebug>();
        grm.SetDebugValues(grmDebug);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.IsPlaying(script)) { return; }
        if (GUILayout.Button("Decide!"))
        {
            script.Decide();
        }
    }
}