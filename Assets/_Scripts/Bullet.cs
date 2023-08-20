using System;
using UnityEngine;

namespace _Scripts
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private LayerMask _soldierLayerMask;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private BulletType _bulletType;


        private void Start()
        {
            Destroy(gameObject, 5f);
        }

        private void Update()
        {
            Move();

            var isTouchingSoldier = Physics2D.IsTouchingLayers(_collider, _soldierLayerMask);

            if (isTouchingSoldier)
            {
                if (_bulletType == BulletType.Ai)
                {
                    Player.Instance.SetLastHealth();
                }
                
                Destroy(gameObject);
            }
        }

        
        private void Move()
        {
            switch (transform.localScale.x)
            {
                case 1:
                {
                    transform.Translate(Vector3.right * (_speed * Time.deltaTime));

                    break;
                }
                case -1:
                {
                    transform.Translate(Vector3.left * (_speed * Time.deltaTime));

                    break;
                }
            }
        }
    }
}
