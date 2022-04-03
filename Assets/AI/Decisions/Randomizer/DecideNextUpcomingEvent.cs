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

    public void Start()
    {
        _agent = FindObjectOfType<RandomizerAgent>();
        var nodes = new BinaryTreeNode<DecisionNode>[] {
                new BinaryTreeNode<DecisionNode>(AnyCurrentEventFollowOnCardsInBudget(), false),
                new BinaryTreeNode<DecisionNode>(PickRandomFollowOnCardInBudget(), true),
                new BinaryTreeNode<DecisionNode>(PickRandomCardInBudget(), true)
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
                    Debug.Log("Evaluation: Survey says " + (result ? "yes" : "no"));
                    index = result ? _decisionTree.LeftIndex(index) : _decisionTree.RightIndex(index);
                    break;
                case DecisionNodeType.Action:
                    isDecided = true; 
                    break;
            }
        } while (!isDecided);
    }

    public DecisionNode AnyCurrentEventFollowOnCardsInBudget()
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

    public DecisionNode PickRandomFollowOnCardInBudget()
    {
        Func<bool> evaluator = () =>
        {
            Debug.Log("Decision: Pick a random follow-on card thats in budget.");
            var followOnEvents = from e in climateEventManager.EventsInBudget(_agent.Budget)
                                 where e.FollowOnEvents.Contains(climateEventManager.CurrentClimateEvent)
                                 select e;
            var index = UnityEngine.Random.Range(0, followOnEvents.Count() - 1);
            SelectNextEvent(new List<ClimateEvent>(followOnEvents)[index]);
            return false; // not actually used for anything. cleanup for later.
        };
        return new DecisionNode(evaluator, DecisionNodeType.Action);
    }

    public DecisionNode PickRandomCardInBudget()
    {
        Func<bool> evaluator = () =>
        {
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
        // Trigger the UI to do stuff?
    }
}

[CustomEditor(typeof(DecideNextUpcomingEvent))]
public class DecideNextUpcomingEventEditor : Editor
{
    private DecideNextUpcomingEvent script;

    private void OnEnable()
    {
        script = (DecideNextUpcomingEvent)target;
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