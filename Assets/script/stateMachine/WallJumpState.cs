using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class WallJumpState : BaseState
{
    Vector3 JumpVector;
    ControllerColliderHit hit;
    public static WallJumpState instance;
    //float timer = 0.5f;
    public WallJumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
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
    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        SwitchState(factory.GrappleStart());
        GrappleStart.instance.held = false;
        return;
    }
    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        SwitchState(factory.GrappleStart());
        GrappleStart.instance.held = true;
        return;
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
        if (ctx._isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }
        //if (ctx._collision.gameObject.CompareTag("wall"))
        //{
        //    SwitchState(factory.WallSlide());
        //    return;
        //}
        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;
    }
}
