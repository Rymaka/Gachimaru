using Gachimaru.Gameplay;
using UnityEngine;

    public class EnemyGravityScript : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Rigidbody _body;
        [SerializeField] private float _currentGravity;
        [SerializeField] private float _gravityForce;
        [SerializeField] private float _baseGravity;

        private void Start()
        {
            _currentGravity = _baseGravity;
        }


        private void FixedUpdate()
        {
            CheckGravity();
            _body.AddForce(Vector3.down * _gravityForce);
        }

        private void CheckGravity()
        {
            _currentGravity = _baseGravity;
            _gravityForce = _currentGravity;
        }
    }
