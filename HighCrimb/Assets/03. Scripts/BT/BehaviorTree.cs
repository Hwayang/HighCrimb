using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


abstract public class Node
{
    public string nodeName;
    public List<Node> children = new List<Node>();

    public enum EvaluationResult
    {
        Failure,
        Running,
        Success
    }

    public abstract EvaluationResult Run();
}

public class ControlFlowNode : Node
{
    public ControlFlowNode(string Type)
    {
        nodeName = Type;
    }

    public override EvaluationResult Run()
    {
        if (nodeName is "Selector")
        {
            return RunSelector();
        }
        else if (nodeName is "Sequence")
        {
            return RunSequence();
        }
        else
        {
            return EvaluationResult.Failure;
        }
    }

    private EvaluationResult RunSequence()
    {
        foreach (var node in children)
        {
            if (node.Run() is EvaluationResult.Failure)
            {
                return EvaluationResult.Failure;
            }
        }

        return EvaluationResult.Success;
    }

    private EvaluationResult RunSelector()
    {
        foreach (var node in children)
        {
            if (node.Run() is EvaluationResult.Success)
            {
                return EvaluationResult.Success;
            }
        }

        return EvaluationResult.Failure;
    }
}

public class ExcutionNode : Node
{
    Func<bool> targetAction;
    public ExcutionNode(Func<bool> action)
    {
        targetAction = action;
    }
    public override EvaluationResult Run()
    {
        if (targetAction())
        {
            return EvaluationResult.Success;
        }

        return EvaluationResult.Failure;
    }
}

public class RootNode : Node
{
    public RootNode() { nodeName = "rootNode"; }

    public override Node.EvaluationResult Run()
    {
        return EvaluationResult.Success;
    }
}
public class BehaviorTree
{
    public RootNode root;
}
