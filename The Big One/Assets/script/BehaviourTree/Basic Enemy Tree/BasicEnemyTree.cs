
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyTree : BehaviourTree
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootpoint;
    [SerializeField] float bullets ;
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node> 
        { 
        new PlayerFoundNode(transform),
        new CheckBulletsNode(this,bullets),
        new AimNode(transform,shootpoint),
        new CheckLOSNode(shootpoint),
        new ShootNode(bullet,shootpoint,this) 
        });
        return root;
    }
    public float _bullets { get{return bullets; } set { bullets = value; } }

 
}
