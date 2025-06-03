using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundRangeTrigger : MonoBehaviour
{
    public enum RangeType { FootStep, GunFire }
    public RangeType rangeType;
    public GameObject enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player == null) return;
            FSM fsm = enemy.GetComponent<FSM>();
            StateType currentState = fsm.CurrentState();
            if (currentState == StateType.Idle && rangeType == RangeType.FootStep && player.IsWalking())
            {
                fsm.TransitionToState(StateType.Alert);
            }
            else if (currentState == StateType.Idle && rangeType == RangeType.GunFire && player.IsFiring())
            {
                fsm.TransitionToState(StateType.Alert);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player == null) return;
            FSM fsm = enemy.GetComponent<FSM>();
            StateType currentState = fsm.CurrentState();
            if (currentState == StateType.Idle && rangeType == RangeType.FootStep && player.IsWalking())
            {
                fsm.TransitionToState(StateType.Alert);
            }
            else if (currentState == StateType.Idle && rangeType == RangeType.GunFire && player.IsFiring())
            {
                fsm.TransitionToState(StateType.Alert);
            }
        }
    }
}
