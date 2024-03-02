public class StateFactory
{
    PlayerStateMachine _context;
    public StateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public BaseState Idle() 
    {
        return new IdleState(_context,this);
    }
    public BaseState Jump() 
    {
        return new JumpState(_context,this);
    }
    public BaseState Move() 
    {
        return new MoveState(_context,this);
    }
    public BaseState Slide() 
    {
        return new SlideState(_context,this);
    }
    public BaseState Dash() 
    {
        return new DashState(_context,this);
    }
    public BaseState Fall() 
    {
        return new FallState(_context,this);
    }

    //DO THE SAME FOR ALL CLASSES
}
