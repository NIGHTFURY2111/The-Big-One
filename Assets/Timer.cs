using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] bool TimerOn;
    [SerializeField] bool TimerOff;
    public static event Action OnTimerStart;
    public static event Action OnTimerStop;

    private void OnTriggerEnter(Collider other)
    {
        if (TimerOn)
            OnTimerStart?.Invoke();
        if (TimerOff)
            OnTimerStop?.Invoke();
    }
}
