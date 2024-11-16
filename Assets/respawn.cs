using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class respawn : MonoBehaviour
{
    public static event Action<Transform> OnRespawnTriggerEvent;
    public static event Action OnRespawn;

    [SerializeField] private float killzone;
    Transform RespawnPoint;

    private Vector3 pointPosition;
    private Quaternion pointRotation;



    private void Start()
    {
        OnRespawnTriggerEvent += TriggerEventInovke;
    }
    void Update()
    {
        if (transform.position.y < killzone) { RespawnPlayer(); }
    }

    void TriggerEventInovke(Transform t)
    {
        pointPosition = t.position;
        pointRotation = t.rotation;

    }

    public void OnRestart(InputValue pressed)
    {
        if (pressed.isPressed)
        {
            RespawnPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            RespawnPoint = other.transform.GetChild(0);
            OnRespawnTriggerEvent?.Invoke(RespawnPoint.transform);
        }
    }

    private void RespawnPlayer()
    {
        transform.position = pointPosition;
        transform.rotation = pointRotation;
        OnRespawn?.Invoke();
    }
}
