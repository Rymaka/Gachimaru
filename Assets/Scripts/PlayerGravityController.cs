using System;
using System.Collections;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;

namespace Gachimaru.Gameplay
{
    public class PlayerGravityController : MonoBehaviour
    {
        [SerializeField] private Player _player;        
        [SerializeField] private float _baseGravity;
        [SerializeField] private float _attackGravity;
        [SerializeField] private float _onGroundGravity;
        [SerializeField] private float _currentGravity;
        [SerializeField] private float _dashingGravity;
        [SerializeField] private bool _gravityIsApplied;
        [SerializeField] private bool _isFalling;
        public event Action OnFalling;

        public bool _IsFalling => _isFalling;
        private Vector3 LastPosition;

        private bool _shouldFall;
        //refer to player state
        public bool _ShoudlFalling => _shouldFall;
         

        private void Awake()
        {
            //gravity Actions
            SetBaseGravity();
            _player.AttackController.OnAttack += SetAttackGravity;
            _player.AttackController.OnUnAttack += SetBaseGravity;
            _player.GroundController.OnGround += SetOnGroundGravity;
            _player.GroundController.OnBarelyGround += SetOnGroundGravity;
            _player.GroundController.OnUnground += ChangeGravityIsApplied;
            _player.SummonersDash.IsDashing += SetDashingGravity;
        }

        private void Start()
        {
            LastPosition = transform.position;
        }

        private void FixedUpdate()
        {
            
           // if (!_player.GroundController.IsFullyGrounded)
            //{
                _player.MovementController.ApplyGravity(_currentGravity);
           // }
            CheckShouldFall();
            CheckIsFalling();
            SetFallingGravity();
        }

        private void CheckIsFalling()
        {
            var currentPosition = transform.position;
            //TODO Подумать будет ли это влиять на StateIsJumping
            if (currentPosition.y < LastPosition.y && !_player.GroundController.IsFullyGrounded)
            {
                _isFalling = true;
                OnFalling?.Invoke();

            }
            else
            {
                _isFalling = false;
            }
            
            LastPosition = transform.position;


        }
        private void CheckShouldFall()
        {
            if (_currentGravity > 0)
            {
                _shouldFall = true;
            }
            else
            {
                _shouldFall = false;
            }
        }

        private void SetFallingGravity()
        {
            if (_player.ReadPlayerState.StateEnableGravity && !_gravityIsApplied)
            {
                _currentGravity = _baseGravity;
            }
        }

        private void ChangeGravityIsApplied()
        {
            _gravityIsApplied = false;
        }
        private void SetBaseGravity()
        {
            _currentGravity = _baseGravity;
            _gravityIsApplied = true;
        }

        private void SetOnGroundGravity()
        {
            _currentGravity = _onGroundGravity;
            _gravityIsApplied = true;
        }

        private void SetDashingGravity()
        {
            _currentGravity = _dashingGravity;

        }
        private void SetAttackGravity()
        {
            _currentGravity = _attackGravity;
        }
        
    }
}