using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using DG.Tweening;

public class Ai : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _flagLayerMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletCreateTransform;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpingPower;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private LayerMask _bulletLayerMask;
    [SerializeField] private float _functionCooldown; // Cooldown time in seconds
    

    private static readonly int IS_RUNNING = Animator.StringToHash("isRunning");
    private static readonly int JUMP = Animator.StringToHash("jump");
    private static readonly int SHOOT = Animator.StringToHash("shoot");
    private static readonly int IS_DEAD = Animator.StringToHash("isDead");

    private bool m_IsFacingRight = true;
    private bool m_IsGrounded;
    private bool m_CanHitFlag = true;
    private bool m_CanMove = true;
    private int m_Direction;
    private bool m_IsDead;

    private bool m_CanCallFunction = true; // For cooldown

    private void Update()
    {
        FlagControl();
        PlayerControl();

        var isTouchingBullet = Physics2D.IsTouchingLayers(_collider, _bulletLayerMask);

        if (isTouchingBullet)
        {
            GameManager.AddAiCount(1);
            Destroy(gameObject);
        }

        // Check if the current X position is within the range -8.35 to 8.35
        if (transform.position.x >= -8.35f && transform.position.x <= 8.35f && m_CanCallFunction)
        {
            // Start calling CallFunctionPeriodically
            StartCoroutine(CallFunctionWithCooldown());
        }
    }

    private IEnumerator CallFunctionWithCooldown()
    {
        m_CanCallFunction = false; // Disable calling function during cooldown

        // Call the function
        CallFunctionPeriodically();
        
        // Wait for the cooldown period
        yield return new WaitForSeconds(_functionCooldown);

        m_CanCallFunction = true; // Enable calling the function again
    }

    private void CallFunctionPeriodically()
    {
        if (m_IsDead) {return; }
        
        var bullet = Instantiate(_bullet, _bulletCreateTransform.position, Quaternion.identity);
        bullet.transform.localScale = transform.localScale;
    }

    private void PlayerControl()
    {
        var isTouchingPlayer = Physics2D.IsTouchingLayers(_collider, _playerLayerMask);

        if (isTouchingPlayer && Player.Instance.GetCanTouch())
        {
            Player.Instance.SetLastHealth();

            m_CanMove = false;
            m_IsDead = true;
            m_CanHitFlag = false;
            
            transform.DOScale(Vector3.one * 0.1f, 0.25f).OnComplete(() => { Destroy(gameObject); });
        }
    }

    private void FlagControl()
    {
        var isTouchingFlag = Physics2D.IsTouchingLayers(_collider, _flagLayerMask);

        if (isTouchingFlag && m_CanHitFlag)
        {
            m_CanHitFlag = false;
            m_CanMove = false;
            m_IsDead = true;

            transform.DOScale(Vector3.one * 0.1f, 0.5f).OnComplete(() => { Destroy(gameObject); });
            GameManager.AddFlagHealth(-1);
        }
    }

    private void FixedUpdate()
    {
        TryMove(m_Direction);
    }

    private void TryMove(int direction)
    {
        if (!m_CanMove) return;

        var movement = new Vector2(direction * _moveSpeed, _rigidbody2D.velocity.y);

        _rigidbody2D.velocity = movement;
    }

    public void SetDirection(int direction)
    {
        m_Direction = direction;
    }

    public void ReverseLocalScaleX(int number)
    {
        var localscale = transform.localScale;
        localscale.x = localscale.x * number;

        transform.localScale = localscale;
    }
}
