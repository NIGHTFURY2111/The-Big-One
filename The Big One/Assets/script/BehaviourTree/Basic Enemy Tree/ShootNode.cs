using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootNode : Node
{
    GameObject bullet;

    Transform shootpoint;
    BasicEnemyTree enemy;
    bool shot = false;
    

    public ShootNode(GameObject bullet, Transform shootpoint, BasicEnemyTree enemy)
    {
        this.bullet = bullet;
        this.shootpoint = shootpoint;
        this.enemy = enemy;
       
       
    }

    IEnumerator shooting()
    {
        shot = true;
        enemy._bullets--;
        GameObject bulletshot = GameObject.Instantiate(bullet, shootpoint.position, shootpoint.rotation);
        yield return new WaitForSecondsRealtime(0.5f);
        shot = false;

    }
    public override NodeState Evaluate()
    {
        if (!shot)
        {
            enemy.StartCoroutine(shooting());
            state = NodeState.Success;

        }
        else
        {
            state = NodeState.Running;
        }
        return state;
    }
}