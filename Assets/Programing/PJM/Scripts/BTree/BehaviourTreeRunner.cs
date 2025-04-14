using System.Collections;
using System.Collections.Generic;
using Programing.PJM.Scripts.BTree.Nodes;
using UnityEngine;

public class BehaviourTreeRunner
{
    private BaseNode _rootNode;

    public BehaviourTreeRunner(BaseNode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Evaluate();
    }
}
