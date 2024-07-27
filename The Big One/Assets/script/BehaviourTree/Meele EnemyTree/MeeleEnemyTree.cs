using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MeeleEnemyTree : BehaviourTree
{
    CharacterController charaCont;
    private void Awake()
    {
        charaCont = GetComponent<CharacterController>();

    }
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node> 
        {
            new PlayerFoundNode(transform),
            new AimNode(transform,transform),
            new CheckLOSNode(transform),
            new DashStateNode(transform,50f,charaCont,this)
        });
        return root;
    }
}
