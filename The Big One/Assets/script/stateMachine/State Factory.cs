public class StateFactory
{
    PlayerStateMachine _context;

    //public StateFactory(PlayerStateMachine currentContext)
    //{
    //    _context = currentContext;
    //}

    //public BaseState Idle() 
    //{
    //    return new IdleState(_context,this);
    //}
    //public BaseState Jump() 
    //{
    //    return new JumpState(_context,this);
    //}
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
    //public BaseState Move() 
    //{
    //    return new MoveState(_context,this);
    //}
    //public BaseState Slide() 
    //{
    //    return new SlideState(_context,this);
    //}
    //public BaseState Dash() 
    //{
    //    return new DashState(_context,this);
    //}
    //public BaseState Fall() 
    //{
    //    return new FallState(_context,this);
    //}
    IdleState idleState;
    JumpState jumpState;
    MoveState moveState;
    FallState fallState;
    SlideState slideState;
    DashState dashState;
    WallSlideState wallSlideState;
    WallJumpState wallJumpState;
    WallRunState wallRunState;
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

        SlideState.Instance.slideExit += JumpState.instance.slideEnter;
        SlideState.Instance.slideExit += FallState.instance.slideEnter;
        _context.Collide += WallJumpState.Instance.getCollider;
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

    //DO THE SAME FOR ALL CLASSES
}
