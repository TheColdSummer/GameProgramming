using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private enum IdleAction { Stay, Move, ReturnToCenter, AdjustPosition }
    private IdleAction _currentAction;
    private FSM _fsm;
    private GameObject _enemy;
    private Enemy _enemyScript;
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _actionTime;
    private float _timer;
    private Vector2 _moveDirection;
    private float _moveSpeed = 0.6f;
    private EnemyCollisionRelay _collisionRelay;
    private Pathfinding _pathfinding;
    private List<Node> _pathToCenter;
    private int _currentNodeIndex;
    private bool _pathReady;
    
    public IdleState(FSM fsm)
    {
        _fsm = fsm;
        _enemy = fsm.gameObject;
        _enemyScript = _enemy.GetComponent<Enemy>();
        _rb = _enemy.GetComponent<Rigidbody2D>();
        _animator = _enemy.GetComponent<Animator>();
        _collisionRelay = _enemy.GetComponent<EnemyCollisionRelay>();
        if (_collisionRelay != null)
        {
            _collisionRelay.onWallCollision += OnWallCollision;
        }

        if (_enemyScript == null || _rb == null || _animator == null)
        {
            Debug.LogError("IdleState initialization failed: Missing components on enemy.");
        }
        _pathfinding = fsm.GetPathFinding();
    }

    public void OnEnter()
    {
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
        ChooseRandomAction();
    }

    public void OnExit()
    {
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
        if (_rb != null)
            _rb.velocity = Vector2.zero;
        if (_animator != null)
        {
            _animator.SetBool("left_move", false);
            _animator.SetBool("right_move", false);
        }
        if (_collisionRelay != null)
            _collisionRelay.onWallCollision -= OnWallCollision;
    }

    public void OnUpdate()
    {
        if (_enemyScript == null || _rb == null) return;

        if (_currentAction == IdleAction.AdjustPosition)
        {
            MoveAround(_moveSpeed * 3f);
        }

        GameObject idleRangeObj = _enemyScript.GetIdleRange();
        if (idleRangeObj != null)
        {
            CircleCollider2D range = idleRangeObj.GetComponent<CircleCollider2D>();
            if (range != null)
            {
                Vector2 center = range.transform.position;
                float radius = range.radius * range.transform.lossyScale.x;
                float dist = Vector2.Distance(_enemy.transform.position, center);

                if (dist > radius)
                {
                    if (_currentAction != IdleAction.ReturnToCenter)
                    {
                        _currentAction = IdleAction.ReturnToCenter;
                        StartPathToCenter(center);
                    }
                }
            }
        }

        switch (_currentAction)
        {
            case IdleAction.Stay:
                _animator.SetBool("left_move", false);
                _animator.SetBool("right_move", false);
                _rb.velocity = Vector2.zero;
                _timer += Time.deltaTime;
                if (_timer >= _actionTime)
                    ChooseRandomAction();
                break;
            case IdleAction.Move:
                MoveAround(_moveSpeed);
                break;
            case IdleAction.ReturnToCenter:
                MoveToIdleRangeCenterByPath();
                break;
        }
    }

    private void MoveAround(float moveSpeed)
    {
        if (_moveDirection.x < 0)
        {
            _animator.SetBool("left_move", true);
            _animator.SetBool("right_move", false);
        }
        else if (_moveDirection.x > 0)
        {
            _animator.SetBool("left_move", false);
            _animator.SetBool("right_move", true);
        }
        else
        {
            _animator.SetBool("left_move", false);
            _animator.SetBool("right_move", false);
        }
        _rb.velocity = _moveDirection * moveSpeed;
        _timer += Time.deltaTime;
        if (_timer >= _actionTime)
        {
            _rb.velocity = Vector2.zero;
            ChooseRandomAction();
        }
    }

    private void ChooseRandomAction()
    {
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = false;
        _timer = 0f;
        _actionTime = Random.Range(1f, 4f);
        if (Random.value < 0.5f)
        {
            _currentAction = IdleAction.Stay;
        }
        else
        {
            _currentAction = IdleAction.Move;
            float angle = Random.Range(0f, 2f * Mathf.PI);
            _moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        }
    }

    // private void MoveToIdleRangeCenter()
    // {
    //     GameObject idleRangeObj = _enemyScript.GetIdleRange();
    //     if (idleRangeObj == null) return;
    //     Vector2 center = idleRangeObj.transform.position;
    //     Vector2 dir = (center - (Vector2)_enemy.transform.position).normalized;
    //     if (dir.x < 0)
    //     {
    //         _animator.SetBool("left_move", true);
    //         _animator.SetBool("right_move", false);
    //     }
    //     else if (dir.x > 0)
    //     {
    //         _animator.SetBool("left_move", false);
    //         _animator.SetBool("right_move", true);
    //     }
    //     else
    //     {
    //         _animator.SetBool("left_move", false);
    //         _animator.SetBool("right_move", false);
    //     }
    //     _rb.velocity = dir * _moveSpeed;
    //
    //     CircleCollider2D range = idleRangeObj.GetComponent<CircleCollider2D>();
    //     if (range != null)
    //     {
    //         float radius = range.radius * range.transform.lossyScale.x;
    //         float dist = Vector2.Distance(_enemy.transform.position, center);
    //         if (dist <= radius * 0.98f)
    //         {
    //             _rb.velocity = Vector2.zero;
    //             ChooseRandomAction();
    //         }
    //     }
    // }
    
    private void StartPathToCenter(Vector2 center)
    {
        _pathReady = false;
        _pathToCenter = null;
        _currentNodeIndex = 0;
        if (_pathfinding != null)
        {
            _pathfinding.FindPathAsyncThreaded(
                _enemy.transform.position,
                center,
                (path) =>
                {
                    _pathToCenter = path;
                    _currentNodeIndex = 0;
                    _pathReady = true;
                }
            );
        }
    }

    private void MoveToIdleRangeCenterByPath()
    {
        if (!_pathReady) return;
        if (_pathToCenter == null || _currentNodeIndex >= _pathToCenter.Count)
        {
            _rb.velocity = Vector2.zero;
            _currentAction = IdleAction.AdjustPosition;
            _timer = 0f;
            _actionTime = Random.Range(1f, 3f);
            float angle = Random.Range(0f, 2f * Mathf.PI);
            _moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            _pathReady = false;
            return;
        }
        _enemy.GetComponent<CapsuleCollider2D>().isTrigger = true;

        Vector2 targetPos = _pathToCenter[_currentNodeIndex].worldPosition;
        Vector2 currentPos = _rb.position;
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, 14 * _moveSpeed * Time.deltaTime);
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

        GameObject idleRangeObj = _enemyScript.GetIdleRange();
        if (idleRangeObj != null)
        {
            Vector2 center = idleRangeObj.transform.position;
            CircleCollider2D range = idleRangeObj.GetComponent<CircleCollider2D>();
            if (range != null)
            {
                float radius = range.radius * range.transform.lossyScale.x;
                float dist = Vector2.Distance(_enemy.transform.position, center);
                if (dist <= radius * 0.98f)
                {
                    _rb.velocity = Vector2.zero;
                    ChooseRandomAction();
                }
            }
        }
    }

    private void OnWallCollision(Collision2D collision)
    {
        _rb.velocity = Vector2.zero;
        ChooseRandomAction();
    }
}