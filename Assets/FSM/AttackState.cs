using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private FSM _fsm;
    private GameObject _enemy;
    private GameObject _player;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Pathfinding _pathfinding;
    private Enemy _enemyScript;
    private float _fireCooldown;
    private float _fireDuration;
    private float _fireTimer;
    private bool _isFiring;

    public AttackState(FSM fsm)
    {
        _fsm = fsm;
        _enemy = fsm.gameObject;
        _player = fsm.GetPlayer();
        _rb = _enemy.GetComponent<Rigidbody2D>();
        _pathfinding = fsm.GetPathFinding();
        _animator = _enemy.GetComponent<Animator>();
        _enemyScript = _enemy.GetComponent<Enemy>();
        ResetFireCooldown();
    }

    public void OnEnter()
    {
        _enemy.GetComponent<Enemy>().ExclaimRed();
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
        ResetFireCooldown();
        _isFiring = false;
        _fireTimer = 0f;
    }

    public void OnExit()
    {
        _enemy.GetComponent<Enemy>().NoQuestion();
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = false;
        _isFiring = false;
        _fireTimer = 0f;
        _enemyScript.weaponControl.StopFire();
    }

    public void OnUpdate()
    {
        _enemyScript.weaponControl.AimAt(_player.transform.position);
        _fireTimer += Time.deltaTime;

        if (_isFiring)
        {
            if (_fireTimer >= _fireDuration)
            {
                _isFiring = false;
                _fireTimer = 0f;
                _enemyScript.weaponControl.StopFire();
                ResetFireCooldown();
            }
            else
            {
                _enemyScript.weaponControl.StartFire();
            }
        }
        else
        {
            if (_fireTimer >= _fireCooldown)
            {
                _isFiring = true;
                _fireDuration = Random.Range(0.3f, 0.6f);
                _fireTimer = 0f;
            }
        }
    }

    private void ResetFireCooldown()
    {
        _fireCooldown = Random.Range(0.4f, 2f);
    }
}