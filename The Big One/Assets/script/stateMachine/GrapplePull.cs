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
        //finalloc = ctx._debugGrapplePoint.transform.position;
        dir = (ctx._debugGrapplePoint.transform.position - ctx.transform.position).normalized;
        //dir = ctx._playerCamera.transform.InverseTransformDirection(finalloc);
        //ctx._debugGrapplePoint.transform.LookAt(ctx.gameObject.transform.position);
        //dir = -ctx._debugGrapplePoint.transform.forward;
    }
    public override void UpdateState()
    {
       ctx._moveDirection = dir * 100f;
       CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx._moveDirectionY = -2f;
    }
    public override void CheckSwitchState()
    {
        if (Vector3.Distance(ctx.gameObject.transform.position,ctx._debugGrapplePoint.transform.position) < 1f) 
        {
            SwitchState(factory.Fall());
            return;
        }
    }
}