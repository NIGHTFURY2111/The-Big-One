using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacterController 
{
    Rigidbody rigidbody;
    Collider collider;

    public abstract void Move(float OverrideMagnitude);
    public abstract bool isGrounded();
}
