using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WallRunState : BaseState
{
    public static WallRunState instance;
    ControllerColliderHit hit;
    float angle;
    Vector3 wallRunDir = new();
    float gavity;
    public WallRunState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        gavity = ctx._gravity;
    }


    public override void EnterState()
    {
        ctx._gravity = 0;
        ctx._moveDirectionY = 0;
        wallRunDir = Quaternion.Euler(0, (Mathf.Sign(angle))*90, 0)*hit.normal;
        //Debug.Log(Vector3.Dot(wallRunDir,hit.normal));
        
        
        
    }

    public override void UpdateState()
    {
        ctx._moveDirectionX = wallRunDir.x * ctx._walkingSpeed;
        ctx._moveDirectionZ = wallRunDir.z * ctx._walkingSpeed;
        CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx._gravity = gavity;
    }

    //public void getCollider(ControllerColliderHit hit)
    //{
    //    this.hit = hit;
    //}
    //public void getangle()
    //{

    //}
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
    {

        if (ctx._isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }
        if (ctx._jump.WasPerformedThisFrame())
        {
            SwitchState(factory.WallJump());
            return;
        }
        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;
    }

    #region getter setter
    public float _angle { get { return angle; } set { angle = value; } }
    public ControllerColliderHit _hit { get { return hit; } set {  hit = value; } } 
    #endregion
}
