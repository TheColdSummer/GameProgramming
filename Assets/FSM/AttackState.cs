using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private FSM _fsm;
    private GameObject _enemy;
    
    public AttackState(FSM fsm)
    {
        _fsm = fsm;
        _enemy = fsm.gameObject;
    }
    
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}