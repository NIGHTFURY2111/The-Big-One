using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class WallJumpState : BaseState
{
    Vector3 JumpVector;
    //ControllerColliderHit hit;
    public static WallJumpState instance;
    bool jumpCompleted;
    Vector3 jumpDir;
    Vector3 lastInput;
    Vector3 wallNormal;
    public WallJumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }

    

    public override void EnterState()
    {
        jumpCompleted = false;
        wallNormal = ctx._getWallNormal;
        jumpDir = Vector3.up + wallNormal; 
        ctx.StartCoroutine(Jumping());
    }


    public override void ExitState()
    {
        
    }

    //public void getCollider(ControllerColliderHit hit)
    //{
    //    this.hit = hit;
    //}

    void Jumpfunct()
    {
        ctx._getPCC.WallJumpForce(jumpDir.normalized, ctx._jumpSpeed);
    }

    IEnumerator Jumping()
    {
        ctx._getPCC.WallJumpForce(jumpDir.normalized    ,35f);  
        yield return new WaitForSecondsRealtime(ctx._jumptime);
        jumpCompleted = true;
    }

    public override void UpdateState()
    {
      if(jumpCompleted)
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
        //else
        //{
        //    SwitchState(factory.Fall());
        //    return;
        //}

        //if (ctx._collision.gameObject.CompareTag("wall"))
        //{
        //    SwitchState(factory.WallSlide());
        //    return;
        //}
        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;
    }
}
