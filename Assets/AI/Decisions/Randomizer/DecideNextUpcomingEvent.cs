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
            new BinaryTreeNode<DecisionNode>(AnyEventInBudgetWhichHasRequirmentsMet(), false), // 0
            new BinaryTreeNode<DecisionNode>(AnyFollowOnEvents(), false), // 1 left child of AnyEventInBudget
            new BinaryTreeNode<DecisionNode>(PickCheapestEventDisregardingBudget(), true), // 2 right child of AnyEventInBudget
            new BinaryTreeNode<DecisionNode>(PickRandomFollowOn(), true), // 3 left child of AnyCurrentEventFollowOnsInBudget
            new BinaryTreeNode<DecisionNode>(AnyEventsWithResourceTypeToPrioritize(ResourceType.Biodiversity, ResourceType.SeaLevel), false), // 4 right child of AnyCurrentEventFollowOnsInBudget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 5 left child of PickCheapestEventDisregardingBudget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 6 right child of PickCheapestEventDisregardingBudget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 7 left child of PickRandomFollowOnsInBudget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 8 right child of PickRandomFollowOnsInBudget
            new BinaryTreeNode<DecisionNode>(PickEventWithResourceType(ResourceType.Biodiversity, ResourceType.SeaLevel), true), // 9 left child of AnyEventsWithResourceTypeInBudgetToTarget
            new BinaryTreeNode<DecisionNode>(PickRandomCardInBudget(), true), // 10 right child of AnyEventsWithResourceTypeInBudgetToTarget
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 11
            new BinaryTreeNode<DecisionNode>(DecisionNode.NoOp, true), // 12
        };
        _decisionTree = new BinaryTree<DecisionNode>(nodes);
    }

    public void Decide()
    {
        Debug.Log("Agent Budget: " + _agent.Budget);
        Debug.Log("Events in System: " + climateEventManager.AllClimateEvents.Count());
        bool isDecided = false;
        int index = 0;
        do
        {
            var node = _decisionTree[index];
            var result = node.data.evaluation();
            switch (node.data.type)
            {
                case DecisionNodeType.Decision:
                    Debug.Log("\tSurvey says " + (result ? "yes" : "no") + " (index " + index + ")");
                    index = result ? _decisionTree.LeftIndex(index) : _decisionTree.RightIndex(index);
                    break;
                case DecisionNodeType.Action:
                    isDecided = true; 
                    break;
            }
        } while (!isDecided);
    }

    public DecisionNode AnyEventInBudgetWhichHasRequirmentsMet()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Decision: Are there any events that are in budget that have their requirements met?");
            return _agent.UsableEvents.Count() > 0;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Decision);
    }

    public DecisionNode PickCheapestEventDisregardingBudget()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Action: Nothing in budget, pick the cheapest option.");
            var next = _agent.EventsWithRequirementsMet.First();
            SelectNextEvent(next);
            return false;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public DecisionNode AnyFollowOnEvents()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Decision: Do we have any follow-on events available and in budget?");
            return _agent.FollowOnEvents.Count() > 0;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Decision);
    }

    public DecisionNode PickRandomFollowOn()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Action: Pick a random follow on event that's in budget.");
            var index = UnityEngine.Random.Range(0, _agent.FollowOnEvents.Count() - 1);
            var next = new List<ClimateEvent>(_agent.FollowOnEvents)[index];
            SelectNextEvent(next);
            return false; // not actually used for anything. cleanup for later.
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public DecisionNode AnyEventsWithResourceTypeToPrioritize(params ResourceType[] targets)
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Decision: Are there any cards with " + targets.ToString() + " that we can target?");
            var anyTargets = false;
            foreach(var resource in targets)
            {
                var chance = 0.0f;
                switch (resource) {
                    case ResourceType.Biodiversity:
                        float inverseBiodiversity = Mathf.Clamp(_grm.maxBiodiversity - _grm.currentBiodiversity, _grm.minBiodiversity, _grm.maxBiodiversity);
                        chance = BiodiversitySeaLevelProbability.Evaluate(inverseBiodiversity / _grm.maxBiodiversity);
                        if (chance >= 0.5)
                        {
                            Debug.Log("\tRandomly choose to check for biodiversity pressure events based on current biodiversity.");
                            anyTargets = anyTargets || _agent.UsableEventsWithResourceTypeInResponse(ResourceType.BiodiversityPressure).Count() > 0;
                        }
                        else
                        {
                            Debug.Log("\tBiodiversity pressure event may exist but we randomly decided to ignore that this time.");
                        }
                        break;
                    case ResourceType.SeaLevel:
                        chance = BiodiversitySeaLevelProbability.Evaluate((float)_grm.currentSeaLevels / _grm.maxSeaLevels);
                        if (chance >= 0.5)
                        {
                            Debug.Log("\tRandomly choose to check for sea level pressure events.");
                            anyTargets = anyTargets || _agent.UsableEventsWithResourceTypeInResponse(ResourceType.SeaLevelPressure).Count() > 0;
                        }
                        else
                        {
                            Debug.Log("\tSea level pressure event may exist but we randomly decided to ignore that this time.");
                        }
                        break;
                    default:
                        anyTargets = anyTargets || _agent.UsableEventsWithResourceTypeInResponse(resource).Count() > 0;
                        break;
                }
            }
            return anyTargets;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Decision);
    }

    public DecisionNode PickEventWithResourceType(params ResourceType[] targets)
    {
        Func<bool> evaluator = () =>
        {
            var newTargets = targets
                .Select(t => t == ResourceType.Biodiversity ? ResourceType.BiodiversityPressure : t)
                .Select(t => t == ResourceType.SeaLevel ? ResourceType.SeaLevelPressure : t);
            var candidates = _agent.UsableEventsWithResourceTypeInResponse(newTargets.ToArray());
            var index = UnityEngine.Random.Range(0, candidates.Count());
            var next = new List<ClimateEvent>(candidates)[index];
            Debug.Log("Action: Picked an event with specific resource type");
            Debug.Log("\tEvent: " + next.FlavorText);
            SelectNextEvent(next);
            return false;
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public DecisionNode PickRandomCardInBudget()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Assert(GuardClauses.IsNotEmpty(_agent.UsableEvents.ToList()), "No climate events to work with!");
            var index = UnityEngine.Random.Range(0, _agent.UsableEvents.Count() - 1);
            if(_agent.UsableEvents.Count() > 0)
            {
                var next = _agent.UsableEvents.ToList()[index];
                Debug.Log("Action: Picked a random event in budget");
                Debug.Log("\tEvent: " + next.FlavorText);
                SelectNextEvent(next);
            }
            else { Debug.Log("\tNo events in budget, doing nothing."); }
            return false; // not actually used for anything. cleanup for later.
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public void SelectNextEvent(ClimateEvent ce)
    {
        climateEventManager.CurrentClimateEvent = ce;

        // Trigger the UI to do stuff?
    }
}