
using UnityEngine;


public class AimNode : Node
{
    Transform player;
    Transform enemyTransform;
    Transform shootpoint;
    Vector3 shootDir;
    
    public AimNode(Transform enemy,Transform Shootpoint)
    {
        enemyTransform = enemy;
        player = GameObject.FindWithTag("Player").transform;
        
        shootpoint = Shootpoint;
    }
    public override NodeState Evaluate()
    {
        shootDir =  player.position - enemyTransform.position;
        enemyTransform.forward = new Vector3(shootDir.x,enemyTransform.forward.y,shootDir.z);
        shootpoint.forward = shootDir;

        state = NodeState.Success;
        return state;
    }
}