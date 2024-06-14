using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class WallRunState : BaseState
{
    float timer, angle, gravity, magnitude;
    public static WallRunState instance;
    ControllerColliderHit hit;
    Vector3 wallRunDir = new();
    public WallRunState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        gravity = ctx._gravity;
    }


    public override void EnterState()
    {
        ctx.TEMPCollide += WallRunState.instance.tempFunction;
        magnitude = (ctx._magnitude < 30) ? ctx._minWallSlideSpeed : ctx._magnitude;
        timer = ctx._gravity = ctx._moveDirectionY = 0;
        wallRunDir = Quaternion.Euler(0, (Mathf.Sign(angle)) * 90, 0) * hit.normal;
        //Debug.Log(Vector3.Dot(wallRunDir,hit.normal));
    }

    public override void UpdateState()
    {
        if (timer < ctx._wallRunTime) { timer += Time.deltaTime; }
        else { ctx._gravity = gravity / 2f; }
        ctx._moveDirectionX = wallRunDir.x * magnitude;
        ctx._moveDirectionZ = wallRunDir.z * magnitude;
        CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx.TEMPCollide -= WallRunState.instance.tempFunction;
        ctx._gravity = gravity;
    }

    public void getCollider(ControllerColliderHit hit)
    {
        this.hit = hit;
    }

    public void getCollider(string hit)
    {
        Debug.Log(hit);
    }

    public void tempFunction(string hit)
    {

        Debug.Log(hit);
    }
    public override void CheckSwitchState()
    {


        if (ctx._characterController.isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }
        if (ctx._jump.WasPerformedThisFrame())
        {
            WallRunJumpState wrss = (WallRunJumpState)factory.WallRunJumpState();
            wrss.angle = angle;
            wrss.hit = hit;
            SwitchState(factory.WallRunJumpState());
            return;
        }

    }

    #region getter setter
    public float _angle { get { return angle; } set { angle = value; } }
    public ControllerColliderHit _hit { get { return hit; } set { hit = value; } }
    #endregion
}
