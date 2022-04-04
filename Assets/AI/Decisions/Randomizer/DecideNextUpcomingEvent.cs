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
            new BinaryTreeNode<DecisionNode>(AnyEventInBudget(), false), // 0
            new BinaryTreeNode<DecisionNode>(AnyCurrentEventFollowOnsInBudget(), false), // 1 left child of AnyEventInBudget
            new BinaryTreeNode<DecisionNode>(PickCheapestEventDisregardingBudget(), true), // 2 right child of AnyEventInBudget
            new BinaryTreeNode<DecisionNode>(PickRandomFollowOnsInBudget(), true), // 3 left child of AnyCurrentEventFollowOnsInBudget
            new BinaryTreeNode<DecisionNode>(AnyEventsWithResourceTypeInBudgetToTarget(ResourceType.BiodiversityPressure, ResourceType.SeaLevelPressure), false), // 4 right child of AnyCurrentEventFollowOnsInBudget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 5 left child of PickCheapestEventDisregardingBudget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 6 right child of PickCheapestEventDisregardingBudget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 7 left child of PickRandomFollowOnsInBudget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 8 right child of PickRandomFollowOnsInBudget
            new BinaryTreeNode<DecisionNode>(PickEventWithResourceTypeInBudget(ResourceType.BiodiversityPressure, ResourceType.SeaLevelPressure), true), // 9 left child of AnyEventsWithResourceTypeInBudgetToTarget
            new BinaryTreeNode<DecisionNode>(PickRandomCardInBudget(), true), // 10 right child of AnyEventsWithResourceTypeInBudgetToTarget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 11
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 12
        };
        _decisionTree = new BinaryTree<DecisionNode>(nodes);
    }

    public void Decide()
    {
        Debug.Log("Agent Budget: " + _agent.Budget);
        bool isDecided = false;
        int index = 0;
        do
        {
            var node = _decisionTree[index];
            var result = node.data.evaluation();
            switch (node.data.type)
            {
                case DecisionNodeType.Decision:
                    Debug.Log("Evaluating: Survey says " + (result ? "yes" : "no") + " (index " + index + ")");
                    index = result ? _decisionTree.LeftIndex(index) : _decisionTree.RightIndex(index);
                    break;
                case DecisionNodeType.Action:
                    isDecided = true; 
                    break;
            }
        } while (!isDecided);
    }

    public DecisionNode AnyEventInBudget()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Evaluating: Do we have any events in budget?");
            var events = from e in climateEventManager.EventsInBudget(_agent.Budget)
                                 select e;
            return events.Count() > 0;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Decision);
    }

    public DecisionNode PickCheapestEventDisregardingBudget()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Decision: Nothing in budget, pick the cheapest option.");
            var events = from e in climateEventManager.AllClimateEvents
                                 orderby e.AgentCost ascending
                                 select e;
            var climateEvent = events.FirstOrDefault();
            Debug.Assert(climateEvent != default(ClimateEvent), "Verify that we have at least one climate event to work with.");
            SelectNextEvent(climateEvent);
            return false;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
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
            var next = new List<ClimateEvent>(candidates)[index];
            SelectNextEvent(next);
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
            if(eventsInBudget.Count() > 0)
            {
                var next = new List<ClimateEvent>(eventsInBudget)[index];
                SelectNextEvent(next);
            }
            else { Debug.Log("No events in budget, doing nothing."); }
            return false; // not actually used for anything. cleanup for later.
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public void SelectNextEvent(ClimateEvent ce)
    {
        climateEventManager.NextClimateEvent = ce;

        // Trigger the UI to do stuff?
    }
}