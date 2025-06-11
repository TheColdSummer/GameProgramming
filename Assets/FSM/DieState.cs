using UnityEngine;

public class DieState : IState
{
    private FSM _fsm;
    
    public DieState(FSM fsm)
    {
        _fsm = fsm;
    }
    
    public void OnEnter()
    {
        GameObject.Destroy(_fsm);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}