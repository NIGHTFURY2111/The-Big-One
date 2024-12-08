using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gears
{
    [CreateAssetMenu(fileName = "Gears", menuName = "BigOne/Player/Gears", order = 1)]
    public class GearScriptableObjects : ScriptableObject
    {
        public int GearLevel;

        [Header("General Movement")]
        public float walkingSpeed;
        public float slideSpeed;
        public float forceAppliedInAir;
        public float maxVelocityAddedInAir;
        public float idleDragDebug;
        public float gravity;
        public float jumpSpeed;

        [Header("Wall Movement")]
        public float wallRunSpeed;
        public float minWallRunSpeedReq;

        [Header("gear Settings")]
        public float adrenalineDecayRate;
        public float adrenalineGainMultiplier;
        public float maxGearSpeedLimit;
    }
}