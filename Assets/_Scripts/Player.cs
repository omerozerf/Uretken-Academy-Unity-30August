using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private Animator _animator;
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _bulletCreateTransform;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpingPower;
        [SerializeField] private List<Image> _healthImageList;
        [SerializeField] private float _fireCouldown;

        public static Player Instance;
        
        private static readonly int IS_RUNNING = Animator.StringToHash("isRunning");
        private static readonly int JUMP = Animator.StringToHash("jump");
        private static readonly int SHOOT = Animator.StringToHash("shoot");
        private static readonly int IS_DEAD = Animator.StringToHash("isDead");
        
        private bool m_IsFacingRight = true;
        private bool m_IsGrounded;
        private bool m_CanTouchAi = true;
        private bool m_CanFire = true;
        private float m_HorizontalDirection;


        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            SetHorizontalDirection();
            SetRunAnimation();
            
            Flip();
            StayInArea();
            CheckGround();
            
            TryJump();
            TryShoot();
        }
        
        private void FixedUpdate()
        {
            Move();
        }

        
        private void StayInArea()
        {
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

        private void TryShoot()
        {
            if (Input.GetMouseButtonDown(0) && m_CanFire)
            {
                StartCoroutine(EnableFireAfterCouldownCoroutine());
                
                var bullet = Instantiate(_bullet, _bulletCreateTransform.position, Quaternion.identity);

                bullet.transform.localScale = transform.localScale;
                
                _animator.SetTrigger(SHOOT);
            }
        }

        private IEnumerator EnableFireAfterCouldownCoroutine()
        {
            m_CanFire = false;

            yield return new WaitForSeconds(_fireCouldown);

            m_CanFire = true;
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

        private void Dead()
        {
            _animator.SetBool(IS_DEAD, true);
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

        
        public IEnumerator EnableTouchingAiAfterCooldownCoroutine()
        {
            m_CanTouchAi = false;

            yield return new WaitForSeconds(1.5f);

            m_CanTouchAi = true;
        }

        public bool GetCanTouch()
        {
            return m_CanTouchAi;
        }

        public void SetLastHealth()
        {
            StartCoroutine(Player.Instance.EnableTouchingAiAfterCooldownCoroutine());
            
            if (_healthImageList.Count == 0) { return; }
            
            var lastHealth = _healthImageList[^1];
            
            lastHealth.color = Color.black;
            _healthImageList.Remove(lastHealth);

            if (_healthImageList.Count == 0)
            {
                Dead();
            }
        }
    }
}