using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ReturnEdges : MonoBehaviour
{
    public Vector3 edgeCoordinate (Transform playerposi)
    {
        Vector3 playerToEnemyVector = (Quaternion.AngleAxis(-90,Vector3.up) * (transform.position - playerposi.position)).normalized ;
       
        return playerToEnemyVector + transform.position;
    }

}
