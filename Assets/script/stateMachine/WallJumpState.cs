using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class WallJumpState : BaseState
{
    ControllerColliderHit hit;
    public static WallJumpState instance;
    //float timer = 0.5f;
    public WallJumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
        ctx.Collide += WallJumpState.instance.getCollider;
    }


    public override void EnterState()
    {
        ctx._moveDirectionZ = hit.normal.z * ctx._magnitude;
        ctx._moveDirectionX = hit.normal.x * ctx._magnitude;
        
        ctx._moveDirectionY = ctx._jumpSpeed;
    }

    public override void ExitState()
    {
        //ctx.Collide -= WallJumpState.instance.getCollider;
        
    }
    public void getCollider(ControllerColliderHit hit)
    {
        this.hit = hit;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

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
        if (ctx._getPCC.isGrounded())
        {
            SwitchState(factory.Idle());
            return;
        }
    }
}
