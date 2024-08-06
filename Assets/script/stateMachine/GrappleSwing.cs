using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrappleSwing : BaseState
{
    public static GrappleSwing instance;
    Vector3 dir;
    public GrappleSwing(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }
    public override void EnterState()
    {
        
    }
    public override void UpdateState()
    {
        dir = (ctx._debugGrapplePoint.transform.position - ctx.transform.position).normalized;
        //ctx._characterController.transform.RotateAround(dir, 20);
        //ctx.transform.RotateAround(ctx._debugGrapplePoint.transform.position,Vector3.up,20*Time.deltaTime);
        //ctx._moveDirection = (dir * 3f) + (ctx._playerCamera.transform.forward * 10f) + new Vector3(0,-10f,0);
    }
    public override void ExitState()
    {
        ctx._moveDirectionY = -2f;
    }
    public override void CheckSwitchState()
    {
        if (Vector3.Distance(ctx.gameObject.transform.position, ctx._debugGrapplePoint.transform.position) < 1f)
        {
            SwitchState(factory.Fall());
            return;
        }
    }
}
