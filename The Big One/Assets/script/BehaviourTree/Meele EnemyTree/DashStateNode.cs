
using System.Collections;
using UnityEngine;

public class DashStateNode : Node
{
    Transform enemytranform;
    MeeleEnemyTree enemy;
    float speed;
    CharacterController charaCont;
    bool isDashing = false;
    Vector3 dashDir;


    public DashStateNode(Transform enemytranform, float speed,CharacterController charaCOnt,MeeleEnemyTree enemy)
    {
        this.enemytranform = enemytranform;
        this.enemy = enemy;
        this.speed = speed;
        this.charaCont = charaCOnt;
    }   
    public override NodeState Evaluate()
    {

        if (!isDashing )
        {
           
            enemy.StartCoroutine(dashing());
            state = NodeState.Success;

        }

        charaCont.Move(dashDir * Time.deltaTime * speed);
        state = NodeState.Running;
        return state;
    }
    IEnumerator dashing()
    {
        isDashing = true;
        dashDir = enemytranform.forward;
        
        yield return new WaitForSecondsRealtime(3f);
        isDashing = false;
        
        
    }
   
}
