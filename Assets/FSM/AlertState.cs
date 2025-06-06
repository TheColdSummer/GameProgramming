using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IState
{
    private FSM _fsm;
    private GameObject _enemy;
    private GameObject _player;
    private List<Node> _pathToPlayer;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Pathfinding _pathfinding;
    private int _currentNodeIndex;
    private float _moveSpeed = 20f;
    private bool _pathReady = false;

    public AlertState(FSM fsm)
    {
        _fsm = fsm;
        _enemy = fsm.gameObject;
        _player = fsm.GetPlayer();
        _rb = _enemy.GetComponent<Rigidbody2D>();
        _pathfinding = fsm.GetPathFinding();
        _animator = _enemy.GetComponent<Animator>();
    }

    public void OnEnter()
    {
        _enemy.GetComponent<Enemy>().Question();
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
        _pathReady = false;
        if (_player != null)
        {
            _pathfinding.FindPathAsyncThreaded(
                _enemy.transform.position,
                _player.transform.position,
                (path) =>
                {
                    _pathToPlayer = path;
                    _currentNodeIndex = 0;
                    _pathReady = true;
                }
            );
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }
    //
    // private IEnumerator FindPathCoroutine()
    // {
    //     _pathfinding.FindPathAsync(
    //         _enemy.transform.position,
    //         _player.transform.position,
    //         (path) =>
    //         {
    //             _pathToPlayer = path;
    //             _currentNodeIndex = 0;
    //             _pathReady = true;
    //         }
    //     );
    //     yield return null;
    // }

    public void OnExit()
    {
        _enemy.GetComponent<Enemy>().NoQuestion();
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
        if (!_pathReady) return;
        if (_pathToPlayer == null || _currentNodeIndex >= _pathToPlayer.Count)
        {
            Debug.Log("No path to player");
            _fsm.TransitionToState(StateType.Idle);
            return;
        }
        Vector2 targetPos = _pathToPlayer[_currentNodeIndex].worldPosition;
        Vector2 currentPos = _rb.position;
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, _moveSpeed * Time.deltaTime);
        _rb.MovePosition(newPos);
        Vector2 dir = targetPos - currentPos;
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

        if (Vector2.Distance(newPos, targetPos) < 0.1f)
        {
            _currentNodeIndex++;
        }
    }
}