public abstract class BaseState
{
    public Enemy enemy;
    public StateMachine stateMachine;
    public virtual void Enter() { }
    public virtual void Perform() { }
    public virtual void Exit() { }
    
}