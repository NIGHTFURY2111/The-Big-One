
using UnityEngine;


public class PlayerFoundNode : Node
{
    Transform enemyTransform;

    public PlayerFoundNode(Transform enemy)
    {
        enemyTransform = enemy;
        
    }
    public override NodeState Evaluate()
    {
        foreach (Collider c in Physics.OverlapSphere(enemyTransform.position, 20f))
        {
            if (c.CompareTag("Player"))
            {
                
                state = NodeState.Success;
                return state;
            }
        }
        state = NodeState.Failure;
        return state;
    }

}