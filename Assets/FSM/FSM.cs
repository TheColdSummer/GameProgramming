using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Alert,
    Chase,
    Attack,
    Escape,
    Die
}

/*
 * This is the Finite State Machine class that manages the states of the enemy.
 */
public class FSM : MonoBehaviour
{
    private IState _currentState;
    private GameObject _player;
    private Pathfinding _pathFinding;
    private Dictionary<StateType, IState> _states = new Dictionary<StateType, IState>();
    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.LogError("Player GameObject not found.");
            return;
        }
        _pathFinding = GameObject.Find("GridMap").GetComponent<Pathfinding>();
        _states[StateType.Idle] = new IdleState(this);
        _states[StateType.Alert] = new AlertState(this);
        _states[StateType.Chase] = new ChaseState(this);
        _states[StateType.Attack] = new AttackState(this);
        _states[StateType.Escape] = new EscapeState(this);
        _states[StateType.Die] = new DieState(this);
        
        TransitionToState(StateType.Idle);
    }

    void Update()
    {
        // call the OnUpdate method of the current state each frame
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
        return StateType.Idle;
    }
    
    /*
     * The TransitionToState method is used to change the current state of the FSM.
     */
    public void TransitionToState(StateType state)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }
        _currentState = _states[state];
        _currentState.OnEnter();
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    public Pathfinding GetPathFinding()
    {
        return _pathFinding;
    }
}
