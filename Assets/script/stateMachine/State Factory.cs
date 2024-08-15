public class StateFactory
{
    PlayerStateMachine _context;

    IdleState idleState;
    JumpState jumpState;
    MoveState moveState;
    FallState fallState;
    SlideState slideState;
    DashState dashState;
    WallSlideState wallSlideState;
    WallJumpState wallJumpState;
    WallRunState wallRunState;
    GrappleStart grappleStart;
    GrapplePull grapplePull;
    GrappleSwing grappleSwing;
    public StateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        slideState = new SlideState(_context,this);
        idleState = new IdleState(_context,this);
        jumpState = new JumpState(_context,this);
        moveState = new MoveState(_context,this);
        fallState = new FallState(_context,this);
        dashState = new DashState(_context,this);
        wallJumpState = new WallJumpState(_context,this);
        wallSlideState = new WallSlideState(_context,this);
        wallRunState = new WallRunState(_context,this);
        grappleStart = new GrappleStart(_context,this);
        grapplePull = new GrapplePull(_context,this);
        grappleSwing = new GrappleSwing(_context,this); 

        //SlideState.Instance.slideExit += JumpState.instance.slideEnter;
        _context.Collide += WallJumpState.instance.getCollider;
        //_context.Collide += WallRunState.instance.getCollider;
        // ctx.Collide += FallState.instance.wallCollide;(un/subscribed in fallstate)
    }

    public BaseState Idle() 
    {
        return idleState;
    }
    public BaseState Jump() 
    {
        return jumpState;
    }
    //public BaseState JumpSpecial(SlideState slideState)
    //{
        
    //    JumpState jump = new JumpState(_context, this);
    //    jump.lastSlide = slideState;
    //    return jump;
    //}
    //public BaseState FallSpecial(JumpState jumpState)
    //{
        
    //    FallState jump = new FallState(_context, this);
    //    jump.lastSlide = jumpState.lastSlide;
    //    return jump;
    //}
    public BaseState Move() 
    {
        return moveState;
    }
    public BaseState Slide() 
    {
        return slideState;
    }
    public BaseState Dash() 
    {
        return dashState;
    }
    public BaseState Fall() 
    {
        return fallState;
    }
    public BaseState WallSlide()
    {
        return wallSlideState;
    }
    public BaseState WallJump()
    {
        return wallJumpState;
    }
    public BaseState WallRun()
    {
        return wallRunState;
    }
    public BaseState GrappleStart()
    {
        return grappleStart;
    }
    public BaseState GrapllePull()
    {
        return grapplePull;
    }

    public BaseState GrappleSwing()
    {
        return grappleSwing;
    }
    //DO THE SAME FOR ALL CLASSES
}
