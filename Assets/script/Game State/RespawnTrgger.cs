using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrgger : MonoBehaviour
{
    public static event Action<Transform> OnRespawnTriggerEvent; 
    [SerializeField] private List<GameObject> respawnPoints;

    private void OnTriggerEnter(Collider other)
    {
        OnRespawnTriggerEvent?.Invoke(respawnPoints[0].transform);
    }
}
