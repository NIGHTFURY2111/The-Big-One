using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class WallJumpState : BaseState
{
    Vector3 JumpVector;
    ControllerColliderHit hit;
    public static WallJumpState Instance;
    //float timer = 0.5f;
    public WallJumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        Instance = this;
    }


    public override void EnterState()
    {
        //JumpVector = ctx.MovementVector() * ctx._walkingSpeed;
        //ctx._moveDirectionX = JumpVector.x;
        //ctx._moveDirectionZ = JumpVector.y;
        ctx._moveDirectionZ = hit.normal.z * ctx._walkingSpeed;
        ctx._moveDirectionX = hit.normal.x * ctx._walkingSpeed;
        

        ctx._moveDirectionY = ctx._jumpSpeed;
    }


    //public override void UpdateState()
    //{
    //    //ctx._moveDirectionX = JumpVector.x;
    //    //ctx._moveDirectionZ = JumpVector.y;s
    //    CheckSwitchState();
    //}

    public override void ExitState()
    {
        
    }

    //public override void CheckSwitchState()
    //{
    //    if(ctx._characterController.isGrounded)
    //    {
    //        SwitchState(factory.Idle());
    //        return;
    //    } 
    //}
    public void getCollider(ControllerColliderHit hit)
    {
        this.hit = hit;
    }

    public override void UpdateState()
    {
        //ctx._moveDirectionX = ctx.MovementVector().x * ctx._walkingSpeed;
        //ctx._moveDirectionZ = ctx.MovementVector().z * ctx._walkingSpeed;
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
        //idle (TODO)
        if (ctx._characterController.isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }
        //if (ctx._collision.gameObject.CompareTag("wall"))
        //{
        //    SwitchState(factory.WallSlide());
        //    return;
        //}

    }
}
