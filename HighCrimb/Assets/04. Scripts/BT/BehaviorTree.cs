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

    public enum evaluationResult
    {
        Failure,
        Running,
        Success
    }

    public abstract evaluationResult Run();
}

public class ControlFlowNode : Node
{
    public ControlFlowNode(string Type)
    {
        nodeName = Type;
    }

    public override evaluationResult Run()
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
            return evaluationResult.Failure;
        }
    }

    private evaluationResult RunSequence()
    {
        foreach (var node in children)
        {
            if (node.Run() is evaluationResult.Failure)
            {
                return evaluationResult.Failure;
            }
        }

        return evaluationResult.Success;
    }

    private evaluationResult RunSelector()
    {
        foreach (var node in children)
        {
            if (node.Run() is evaluationResult.Success)
            {
                return evaluationResult.Success;
            }
        }

        return evaluationResult.Failure;
    }
}

public class ExcutionNode : Node
{
    Func<bool> targetAction;
    public ExcutionNode(Func<bool> action)
    {
        targetAction = action;
    }
    public override evaluationResult Run()
    {
        if (targetAction())
        {
            return evaluationResult.Success;
        }

        return evaluationResult.Failure;
    }
}

public class RootNode : Node
{
    public RootNode() { nodeName = "rootNode"; }

    public override Node.evaluationResult Run()
    {
        return evaluationResult.Success;
    }
}
public class BehaviorTree
{
    public RootNode root;
}
