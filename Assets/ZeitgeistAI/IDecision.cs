using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecisionNodeType
{
    Decision,
    Action
}

public class DecisionNode
{
    public Func<bool> evaluation;
    public DecisionNodeType type;

    public DecisionNode(Func<bool> evaluation, DecisionNodeType type)
    {
        this.evaluation = evaluation;
        this.type = type;
    }

    public static DecisionNode NoOp = new DecisionNode(() => {
        Debug.Log("Decision: NoOp");
        return false; 
    }, DecisionNodeType.Action);
}

public interface IDecision
{
    public void Decide();
}