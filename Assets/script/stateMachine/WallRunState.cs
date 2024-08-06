using UnityEngine;

public class WallRunState : BaseState
{
    float timer, angle, gravity, magnitude;
    public static WallRunState instance;
    ControllerColliderHit hit;
    Vector3 wallRunDir = new();

    public WallRunState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
        gravity = ctx._gravity;
    }

    public override void EnterState()
    {
        ctx.Collide += WallRunState.instance.getCollider;
        magnitude = (ctx._magnitude < 30) ? ctx._minWallSlideSpeed : ctx._magnitude;
        timer = ctx._gravity = ctx._moveDirectionY = 0;
        wallRunDir = Quaternion.Euler(0, Mathf.Sign(angle) * 90, 0) * hit.normal;
    }

    public override void UpdateState()
    {
        //Debug.Log(gravity);
        if (timer < ctx._wallRunTime) { timer += Time.deltaTime; }
        else { ctx._gravity = gravity / 2f; 
        }
        ctx._moveDirectionX = wallRunDir.x * magnitude;
        ctx._moveDirectionZ = wallRunDir.z * magnitude;
        CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx._gravity = gravity;
        ctx.Collide -= WallRunState.instance.getCollider;
    }

    public void getCollider(ControllerColliderHit hit)
    {
        //Debug.Log("hi");
        this.hit = hit;
    }

    public override void CheckSwitchState()
    {
        if (ctx._characterController.isGrounded )
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
    public float _angle { get => angle; set => angle = value; }
    public ControllerColliderHit _hit { get => hit; set => hit = value; }
    #endregion

}


