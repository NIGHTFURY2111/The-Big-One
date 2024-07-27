
using System.Collections;
using UnityEngine;

public class CheckBulletsNode : Node
{
    bool isReloading = false;
    BasicEnemyTree enemy;
    float bullets;
   public CheckBulletsNode(BasicEnemyTree enemy,float bullets)
    {
        
        this.enemy = enemy;
        this.bullets = bullets;

    }

    public override NodeState Evaluate()
    {
        if(enemy._bullets == 0 && !isReloading)
        {
            enemy.StartCoroutine(reloading());
            return NodeState.Failure;
        }
        if(isReloading ) 
        {
            return NodeState.Running;
        }
        return NodeState.Success;
    }
    IEnumerator reloading()
    {
        isReloading = true;
        yield return new WaitForSecondsRealtime(2f);
        isReloading = false;
        enemy._bullets = bullets;
    }

}
