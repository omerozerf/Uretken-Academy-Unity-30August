using System;
using UnityEngine;

namespace _Scripts
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed;


        private void Start()
        {
            Destroy(gameObject, 5f);
        }

        private void Update()
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
