using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WallRunState : BaseState
{
    public static WallRunState instance;
    float angle;
    Vector3 wallRunDir;
    float gavity;

    public RaycastHit hit;
    public WallRunState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
       
    }


    public override void EnterState()
    {
        hit = ctx._getPCC._wallHit;
        ctx._getisWall = true;
        //ctx._getPCC._setvelocityVector = new Vector3(ctx._getPCC._getvelocityVector.normalized.x,0, ctx._getPCC._getvelocityVector.normalized.z);

        angle = Vector3.SignedAngle(hit.normal, new Vector3(ctx._getPCC.GetCurrentHorizontal().x,0,ctx._getPCC.GetCurrentHorizontal().y), Vector3.up);

        
        Vector3 wallRunDir = Quaternion.Euler(0, (Mathf.Sign(angle)) * 90, 0) * hit.normal;

        ctx._moveDirection = wallRunDir;


        if (ctx._getPCC._currentHorizontalVelocityMagnitude < ctx._wallRunSpeed)
        {
            ctx._getPCC._TGTvelocityMagnitude = ctx._wallRunSpeed;
        }
        else
        {
            ctx._getPCC._TGTvelocityMagnitude = ctx._getPCC._currentHorizontalVelocityMagnitude;
        }
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedState()
    {
        ctx._getPCC.Move();
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
        if(!ctx._getPCC.WallRunCheckRight(ctx._getWallRunAngle, ctx._getWallRunRaycastDistance) && !ctx._getPCC.WallRunCheckLeft(ctx._getWallRunAngle, ctx._getWallRunRaycastDistance) )
        {
            SwitchState(factory.Idle());
            return;
        }
        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;
    }

    #region getter setter
    public float _angle { get { return angle; } set { angle = value; } }
    //public ControllerColliderHit _hit { get { return hit; } set {  hit = value; } } 
    #endregion
}
