using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private FSM _fsm;
    private GameObject _enemy;
    private Enemy _enemyScript;
    private GameObject _player;
    private List<Node> _pathToPlayer;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Pathfinding _pathfinding;
    private int _currentNodeIndex;
    private float _moveSpeed = 15f;
    private bool _pathReady = false;
    private float _timer = 0f;
    private float _updateInterval = 3f;

    public ChaseState(FSM fsm)
    {
        _fsm = fsm;
        _enemy = fsm.gameObject;
        _player = GameObject.Find("Player");
        _rb = _enemy.GetComponent<Rigidbody2D>();
        _pathfinding = GameObject.Find("GridMap").GetComponent<Pathfinding>();
        _animator = _enemy.GetComponent<Animator>();
        _enemyScript = _enemy.GetComponent<Enemy>();
    }

    public void OnEnter()
    {
        _enemyScript.ExclaimYellow();
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
        _pathReady = false;
        _timer = 0f;
        UpdatePath();
    }

    public void OnExit()
    {
        _enemyScript.NoQuestion();
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = false;
        if (_rb != null)
            _rb.velocity = Vector2.zero;
        if (_animator != null)
        {
            _animator.SetBool("left_move", false);
            _animator.SetBool("right_move", false);
        }

        _pathToPlayer = null;
        _currentNodeIndex = 0;
        _pathReady = false;
    }

    public void OnUpdate()
    {
        _enemyScript.weaponControl.AimAt(_player.transform.position);
        _timer += Time.deltaTime;
        if (_timer >= _updateInterval)
        {
            _timer = 0f;
            UpdatePath();
        }

        if (!_pathReady) return;
        if (_pathToPlayer == null || _currentNodeIndex >= _pathToPlayer.Count)
        {
            _fsm.TransitionToState(StateType.Idle);
            return;
        }

        Vector2 targetPos = _pathToPlayer[_currentNodeIndex].worldPosition;
        Vector2 currentPos = _rb.position;
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, _moveSpeed * (1 + Difficulty.GetDifficulty() * 0.1f) * Time.deltaTime);
        _rb.MovePosition(newPos);
        Vector2 dir = targetPos - currentPos;
        if (_animator != null)
        {
            if (dir.x < 0)
            {
                _animator.SetBool("left_move", true);
                _animator.SetBool("right_move", false);
            }
            else if (dir.x > 0)
            {
                _animator.SetBool("left_move", false);
                _animator.SetBool("right_move", true);
            }
            else
            {
                _animator.SetBool("left_move", false);
                _animator.SetBool("right_move", false);
            }
        }

        if (Vector2.Distance(newPos, targetPos) < 0.1f)
        {
            _currentNodeIndex++;
        }
    }

    private void UpdatePath()
    {
        if (_player != null)
        {
            _pathfinding.FindPathAsyncThreaded(
                _enemy.GetComponent<CapsuleCollider2D>().bounds.center,
                _player.GetComponent<CapsuleCollider2D>().bounds.center,
                (path) =>
                {
                    _pathToPlayer = path;
                    _currentNodeIndex = 0;
                    _pathReady = true;
                }
            );
        }
    }
}