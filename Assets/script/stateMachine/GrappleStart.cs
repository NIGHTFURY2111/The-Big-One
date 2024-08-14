using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GrappleStart : BaseState
{
    public static GrappleStart instance;
    float speed = 10f;
    Vector3 dynamicLoc;
    float t;
    public bool held;
    public GrappleStart(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }
    public override void EnterState()
    {
        t = 0.1f;
        ctx._lineRenderer.enabled = true;
        ctx._lineRenderer.positionCount = 2;
        dynamicLoc = ctx.transform.position;

        if (Physics.Raycast(ctx.transform.position,ctx._playerCamera.transform.forward,out RaycastHit raycastHit))
        {
            ctx._debugGrapplePoint.transform.position = raycastHit.point;
        }
    }
    public override void UpdateState()
    {
        ctx._moveDirectionX = ctx.MovementVector().x * speed;
        ctx._moveDirectionZ = ctx.MovementVector().z * speed;
        if((dynamicLoc != ctx._debugGrapplePoint.transform.position))
        {
            t += 0.5f;
            dynamicLoc = Vector3.Lerp(dynamicLoc, ctx._debugGrapplePoint.transform.position, t * Time.deltaTime);
        }
        //if (ctx._grapple.IsInProgress())
        //{
        //    held = true;
        //}
        //else
        //{
        //    held = false;
        //}
        CheckSwitchState();
    }
    public override void LateUpdateState()
    {
        ctx._lineRenderer.SetPosition(0, ctx.transform.position);
        ctx._lineRenderer.SetPosition(1, dynamicLoc);
    }
    public override void ExitState()
    {
        ctx._lineRenderer.enabled = false;
        t = 0f;
        //ctx._lineRenderer.positionCount = 0;
    }
    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("swing");
    }
    public override void CheckSwitchState()
    {


        if (dynamicLoc == ctx._debugGrapplePoint.transform.position)
        {

            //ctx._grapple.canceled += OnActionCanceled;
            //ctx._grapple.performed += OnActionPerformed;

            if (held == true)
            {
                SwitchState(factory.GrappleSwing());
                return;
            }
            else
            {
                SwitchState(factory.GrapllePull());
                return;
            }
            //if ()
            //{
            //    Debug.Log("grapple swing");
            //}
            //else
            //{
            //    SwitchState(factory.GrapllePull());
            //    return;
            //    }
            //}
        }
    }
}