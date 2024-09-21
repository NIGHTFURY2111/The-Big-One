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
       
    }


    public override void EnterState()
    {
        ctx._getisWall = true;
        ctx._getPCC._setvelocityVector = new Vector3(ctx._getPCC._getvelocityVector.x,0, ctx._getPCC._getvelocityVector.z);
       

    }

    public override void UpdateState()
    {

        CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx._getisWall = false;
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
        if(!ctx._getPCC.WallRunCheckRight(ctx._getWallRunAngle, ctx._getWallRunRaycastDistance) && !ctx._getPCC.WallRunCheckLeft(ctx._getWallRunAngle, ctx._getWallRunRaycastDistance) || ctx._getPCC._currentHorizontalVelocityMagnitude < 1f )
        {
            SwitchState(factory.Idle());
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
