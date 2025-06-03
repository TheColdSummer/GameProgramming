using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Alert,
    Chase,
    Attack,
    Die
}

public class FSM : MonoBehaviour
{
    private IState _currentState;
    private Dictionary<StateType, IState> _states = new Dictionary<StateType, IState>();
    void Start()
    {
        _states[StateType.Idle] = new IdleState(this);
        _states[StateType.Alert] = new AlertState(this);
        _states[StateType.Chase] = new ChaseState(this);
        _states[StateType.Attack] = new AttackState(this);
        _states[StateType.Die] = new DieState(this);
        
        TransitionToState(StateType.Idle);
    }

    void Update()
    {
        _currentState.OnUpdate();
    }

    public StateType CurrentState()
    {
        foreach (var state in _states)
        {
            if (state.Value == _currentState)
            {
                return state.Key;
            }
        }
        throw new Exception("State not found");
    }
    
    public void TransitionToState(StateType state)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }
        _currentState = _states[state];
        _currentState.OnEnter();
    }
}
