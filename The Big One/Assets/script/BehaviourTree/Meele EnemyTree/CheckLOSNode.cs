using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLOSNode : Node
{
    Transform enemyTransform;
    RaycastHit hit;
    public CheckLOSNode(Transform enemyTransform)
    {
        this.enemyTransform = enemyTransform;
        
    }

    public override NodeState Evaluate()
    {
        Physics.Raycast(enemyTransform.position, enemyTransform.forward, out hit);
        if (hit.collider.CompareTag("Player"))
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
        
    }
}
