using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrapplePull : BaseState
{
    public static GrapplePull instance;
    Vector3 finalloc;
    Vector3 dir;
    public GrapplePull(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }
    public override void EnterState()
    {   
        ctx._getPCC._TGTvelocityMagnitude = ctx._walkingSpeed;
        ctx._getPCC._drag = ctx._getMoveDragDebug;
        //finalloc = ctx._debugGrapplePoint.transform.position;
        dir = (ctx._debugGrapplePoint.transform.position - ctx.transform.position).normalized;
        //dir = ctx._playerCamera.transform.InverseTransformDirection(finalloc);
        //ctx._debugGrapplePoint.transform.LookAt(ctx.gameObject.transform.position);
        //dir = -ctx._debugGrapplePoint.transform.forward;
    }
    public override void UpdateState()
    {
       ctx._moveDirection = dir.normalized;
       ctx._getPCC.Move(10);
       CheckSwitchState();
    }
    public override void ExitState()
    {
        //ctx._moveDirectionY = -2f;
    }
    public override void CheckSwitchState()
    {
        if (Vector3.Distance(ctx.gameObject.transform.position,ctx._debugGrapplePoint.transform.position) < 10f) 
        {
            SwitchState(factory.Fall());
            return;
        }
    }
}