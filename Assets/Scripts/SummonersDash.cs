using System;
using System.Collections;
using UnityEngine;
using Gachimaru.InputSystem;

namespace Gachimaru.Gameplay
{
//улетает на трамплине
//нужны эффекты
    public class SummonersDash : SkillBase
    {
        [SerializeField] private Player _player;
        [Header("Dash")] 
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashTime;
        [SerializeField] private float _dashForceMultiplier;
        private float _startTime;
        private Vector3 direction;
        private Vector3 _dashDirection;

        private static float horizontalInput;
        float verticalInput;

        private Vector3 _InputDirection;

        //refer to player state
        public bool StateDashing => _isDashing;

        [SerializeField] Rigidbody _rigidBody;
        public event Action IsDashing;
        public event Action RemoveMoveSpeed;
        public event Action RestoreMoveSpeed;
        private float _dissapearingTime;
        [SerializeField] private bool _canDash;
        [SerializeField] private bool _isDashing;
        public bool _IsDashing => _isDashing;
        [SerializeField] private bool _isAppearing;
        [SerializeField] private bool _isDissapearing;
        public bool IsDissapearing => _isDissapearing;
        public bool IsAppearing => _isAppearing;
        [Header("Input")] 
        //[SerializeField] private KeyCode _dashKey;
        [SerializeField] private Vector3 _forwardCamera;
        [SerializeField] private Vector3 _rightCamera;
        void Start()
        {
            _dissapearingTime = _player.BlinkShaderSkript.DissapearingTime;
            _isDashing = false;
        }
        

        private void FixedUpdate()
        {
            if (_isDashing && _canDash)
            {
                IsDashing?.Invoke();
            }
        }

        private void GetInputs()
        {
            horizontalInput = _player.InputController.FloatAxisHorizontal;
            verticalInput = _player.InputController.FloatAxisVertical;
            _InputDirection = _player.MovementController._MoveDirection;
            _forwardCamera = _player.MovementController.ForwardCameraOrientation;
            _rightCamera = _player.MovementController.RightCameraOrientation;
            direction = _InputDirection.normalized;
            direction.y = 0;
        }

        protected override void Cast()
        {
            Dashing();
        }
        private void Dashing()
        {
            GetInputs();
            StartCoroutine(DissapearIE());
        }
        
        IEnumerator DissapearIE()
        {
            _startTime = Time.time;
            while (Time.time < _startTime + _dissapearingTime)
            {
                _isDissapearing = true;
                yield return null;
            }
            
            StartCoroutine(DashIE());
            _isDashing = true;
            _isDissapearing = false;
        }
        
        IEnumerator DashIE()
        {
            Debug.Log("Dashing");
            
            _startTime = Time.time;
            while (Time.time < _startTime + _dashTime)
            {
                if (direction == Vector3.zero || (horizontalInput == 0 && verticalInput > 0))
                {
                    _dashDirection = _forwardCamera.normalized;
                }
                else
                {
                    _dashDirection = _forwardCamera * verticalInput + _rightCamera * horizontalInput;
                }
                RemoveMoveSpeed?.Invoke();
                _rigidBody.AddForce(_dashDirection * (_dashSpeed * _dashForceMultiplier));
                _isAppearing = false; 
                yield return null;
            }
            RestoreMoveSpeed?.Invoke();
            _isDashing = false;
            _isAppearing = true;
        }
    }
}

