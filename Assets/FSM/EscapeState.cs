using UnityEngine;

public class EscapeState : IState
{
    private FSM _fsm;
    private GameObject _enemy;
    private GameObject _player;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Enemy _enemyScript;
    private float _escapeTimer;
    private float _healTimer;
    private bool _isEscaping;
    private bool _isHealing;
    private Vector2 _escapeDir;
    private float _escapeSpeed = 12f;

    public EscapeState(FSM fsm)
    {
        _fsm = fsm;
        _enemy = fsm.gameObject;
        _player = fsm.GetPlayer();
        _rb = _enemy.GetComponent<Rigidbody2D>();
        _animator = _enemy.GetComponent<Animator>();
        _enemyScript = _enemy.GetComponent<Enemy>();
    }

    public void OnEnter()
    {
        _isEscaping = true;
        _isHealing = false;
        _escapeTimer = 0f;
        _healTimer = 0f;
        if (_player != null)
        {
            Vector2 dir = (_enemy.transform.position - _player.transform.position).normalized;
            _escapeDir = dir;
        }
        else
        {
            _escapeDir = Vector2.right;
        }
        if (_animator != null)
        {
            _animator.SetBool("left_move", _escapeDir.x < 0);
            _animator.SetBool("right_move", _escapeDir.x > 0);
        }

        _enemyScript.Escape();
    }

    public void OnExit()
    {
        if (_rb != null)
            _rb.velocity = Vector2.zero;
        if (_animator != null)
        {
            _animator.SetBool("left_move", false);
            _animator.SetBool("right_move", false);
        }
        _enemyScript.NoQuestion();
    }

    public void OnUpdate()
    {
        if (_isEscaping)
        {
            _escapeTimer += Time.deltaTime;
            if (_rb != null)
                _rb.velocity = _escapeDir * _escapeSpeed;
            if (_escapeTimer >= 3f)
            {
                _isEscaping = false;
                _isHealing = true;
                _healTimer = 0f;
                if (_rb != null)
                    _rb.velocity = Vector2.zero;
                if (_animator != null)
                {
                    _animator.SetBool("left_move", false);
                    _animator.SetBool("right_move", false);
                }
                _enemyScript.Heal();
            }
        }
        else if (_isHealing)
        {
            _healTimer += Time.deltaTime;
            if (_healTimer >= 3f)
            {
                _enemyScript.ChangeHealthDelta(50);
                _fsm.TransitionToState(StateType.Idle);
            }
        }
    }
}