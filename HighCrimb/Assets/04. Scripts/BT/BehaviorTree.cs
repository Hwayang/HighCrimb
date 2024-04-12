using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
abstract class Node
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

class ControlFlowNode : Node
{
    public ControlFlowNode(string Type)
    {
        nodeName = Type;
    }

    public override evaluationResult Run()
    {
        if(nodeName is "Selector")
        {
            return RunSelector();
        }
        else if(nodeName is "Sequence")
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
        for(int i = 0; i < children.Count; i++)
        {
            if (children[i].Run() == evaluationResult.Failure)
            {
                return evaluationResult.Failure;
            }
        }
       
        return evaluationResult.Success;
    }

    private evaluationResult RunSelector()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].Run() == evaluationResult.Success)
            {
                return evaluationResult.Success;
            }
        }
        
        return evaluationResult.Failure;
    }
}

class ExcutionNode : Node
{
    Func<bool> targetAction;
    public ExcutionNode(Func<bool> action)
    {
        targetAction = action;
    }
    public override evaluationResult Run()
    {
        if(targetAction())
        {
            return evaluationResult.Success;
        }

        return evaluationResult.Failure;
    }
}

class RootNode : Node
{
    RootNode() { nodeName = "rootNode"; }

    public override void Run()
    {
        
    }
}


public class BehaviorTree : MonoBehaviour
{
    RootNode A;

    private void Awake()
    {
        A.children.Add(new ControlFlowNode("sequence"));
    }
}
