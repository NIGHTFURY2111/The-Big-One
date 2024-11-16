using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class respawn : MonoBehaviour
{
    [SerializeField] private float killzone;
    [SerializeField] private List<GameObject> RespawnPoints;
    public static event Action OnRespawn;

    private Vector3 pos;



    private void Start()
    {
        RespawnTrgger.OnRespawnTriggerEvent += TriggerEventInovke;
    }
    void Update()
    {
        if (transform.position.y < killzone)
            transform.position = pos;
            
    }

    void TriggerEventInovke(Transform t)
    {
       pos = t.position;
       OnRespawn?.Invoke();
    }


}
