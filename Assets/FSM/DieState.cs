using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState
{
    private FSM _fsm;
    private GameObject _enemy;
    
    public DieState(FSM fsm)
    {
        _fsm = fsm;
        _enemy = fsm.gameObject;
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