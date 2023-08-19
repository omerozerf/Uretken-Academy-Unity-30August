using System;
using UnityEngine;

namespace _Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpingPower;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Animator _animator;

        private static readonly int IS_RUNNING = Animator.StringToHash("isRunning");
        
        private bool m_IsFacingRight = true;
        private bool m_IsGrounded;
        private float m_HorizontalDirection;


        private void Update()
        {
            SetHorizontalDirection();
            
            Flip();
            Jump();
            
            CheckGround();

            _animator.SetBool(IS_RUNNING, Input.GetAxisRaw("Horizontal") != 0);
        }

        private void FixedUpdate()
        {
            Move();
        }
        
        
        private void CheckGround()
        {
            m_IsGrounded = Physics2D.IsTouchingLayers(_collider, _groundLayerMask);
        }
        
        private void Jump()
        {
            if (!m_IsGrounded) { return; }
            
            var velocity = _rigidbody2D.velocity;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody2D.velocity = new Vector2(velocity.x, _jumpingPower);
            }
        }
        
        private void SetHorizontalDirection()
        {
            m_HorizontalDirection = Input.GetAxisRaw("Horizontal");
        }

        private void Move()
        {
            var movement = new Vector2(m_HorizontalDirection * _moveSpeed, _rigidbody2D.velocity.y);

            _rigidbody2D.velocity = movement;
        }

        private void Flip()
        {
            if (m_IsFacingRight && m_HorizontalDirection < 0f || !m_IsFacingRight && m_HorizontalDirection > 0f)
            {
                m_IsFacingRight = !m_IsFacingRight;

                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;

                transform.localScale = localScale;
            }
        }
    }
}