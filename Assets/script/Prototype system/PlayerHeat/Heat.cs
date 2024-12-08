using Gears;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{
    PlayerStateMachine psm;

    float Adrenaline;
    [SerializeField] float AdrenalineOverflowThreshold;
    [SerializeField] float MaxAdrenaline;
    [SerializeField] float AdrenalineOverflowDecayRate;

    [SerializeField] GearScriptableObjects[] gears;

    int currGear;

    public event Action OnGearChange;

    private void Awake()
    {
        currGear = gears[0].GearLevel;
        psm = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        //GearChange();
    }

    //void GearChange()
    //{
    //    if (psm.GearAxisValue() != 0)
    //    {
    //        currGear += ((int)psm.GearAxisValue());
    //        if (currGear > 5 || currGear < 1)
    //        {
    //            currGear -= ((int)psm.GearAxisValue());
    //            return;
    //        }
    //        OnGearChange?.Invoke();
    //    }
    //}

    public void AdrenalineGain(float gain)
    {
        Adrenaline += gain;
    }

    public float GetAdrenaline()
    {
        return Adrenaline;
    }

    public GearScriptableObjects _getGearValues { get => gears[currGear-1] ;}
}
