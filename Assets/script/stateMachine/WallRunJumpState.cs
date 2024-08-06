using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class WallRunJumpState : BaseState
{
    public static WallRunJumpState instance;
    public WallRunJumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }

    public float angle;
    public ControllerColliderHit hit;

    public override void EnterState()
    {
        angle = Vector3.SignedAngle(hit.normal, hit.moveDirection, Vector3.up);

        Vector3 wallRunDir = Quaternion.Euler(0, (Mathf.Sign(angle)) * 45, 0) * hit.normal;

        ctx._moveDirectionZ = wallRunDir.z * ctx._magnitude;
        ctx._moveDirectionX = wallRunDir.x * ctx._magnitude;

        ctx._moveDirectionY = ctx._jumpSpeed;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    //public void getCollider(ControllerColliderHit hit) {   this.hit = hit;    }


    public override void CheckSwitchState()
    {   //dash fall idle

        //Dash
        if (ctx._dash.WasPerformedThisFrame())
        {
            SwitchState(factory.Dash());
            return;
        }
        //Fall
        if (ctx._moveDirectionY <= 0f)
        {
            SwitchState(factory.Fall());
            return;
        }
        //idle
        if (ctx._characterController.isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }

    }
}
