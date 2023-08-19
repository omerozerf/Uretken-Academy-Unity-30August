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
        private static readonly int JUMP = Animator.StringToHash("jump");
        private static readonly int SHOOT = Animator.StringToHash("shoot");
        
        private bool m_IsFacingRight = true;
        private bool m_IsGrounded;
        private float m_HorizontalDirection;


        private void Update()
        {
            SetHorizontalDirection();
            SetRunAnimation();
            
            Flip();
            
            TryJump();
            TryFire();
            
            CheckGround();


            if (transform.position.x < -8.6)
            {
                var pos = transform.position;
                pos.x = -8.6f;

                transform.position = pos;
            }
            if (transform.position.x > 8.6)
            {
                var pos = transform.position;
                pos.x = 8.6f;

                transform.position = pos;
            }
        }
        
        private void FixedUpdate()
        {
            Move();
        }


        private void TryFire()
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("Fire!");
                
                _animator.SetTrigger(SHOOT);
            }
        }
        
        private void SetRunAnimation()
        {
            _animator.SetBool(IS_RUNNING, CanRunAnimation());
        }

        private bool CanRunAnimation()
        {
            return (Input.GetAxisRaw("Horizontal") != 0) && m_IsGrounded;
        }
        
        private void CheckGround()
        {
            m_IsGrounded = Physics2D.IsTouchingLayers(_collider, _groundLayerMask);
        }
        
        private void TryJump()
        {
            if (!m_IsGrounded) { return; }
            
            var velocity = _rigidbody2D.velocity;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody2D.velocity = new Vector2(velocity.x, _jumpingPower);
                
                _animator.SetTrigger(JUMP);
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