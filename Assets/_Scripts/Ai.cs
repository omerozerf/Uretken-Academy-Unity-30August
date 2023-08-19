using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class Ai : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletCreateTransform;
    [SerializeField] private Transform _flagTransform;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpingPower;
    
    private static readonly int IS_RUNNING = Animator.StringToHash("isRunning");
    private static readonly int JUMP = Animator.StringToHash("jump");
    private static readonly int SHOOT = Animator.StringToHash("shoot");
    private static readonly int IS_DEAD = Animator.StringToHash("isDead");
        
    private bool m_IsFacingRight = true;
    private bool m_IsGrounded;
    private int m_Direction;


    private void FixedUpdate()
    {
        Move(m_Direction);
    }

    private void Move(int direction)
    {
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
