<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/BaseState.cs

public abstract class BaseState 
========
public abstract class BaseState
>>>>>>>> Stashed changes:Assets/script/stateMachine/BaseState.cs
{
    protected PlayerStateMachine ctx;
    protected StateFactory factory;
    public BaseState(PlayerStateMachine ctx,StateFactory factory)
    {
        this.ctx = ctx;
        this.factory = factory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public virtual void LateUpdateState() { }
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/BaseState.cs
   
    protected void SwitchState(BaseState next) 
========
    public virtual void FixedState() { }

    protected void SwitchState(BaseState next)
>>>>>>>> Stashed changes:Assets/script/stateMachine/BaseState.cs
    {
        ctx._currentState.ExitState();
        ctx._currentState = next;
        ctx._currentState.EnterState();
    }

}
