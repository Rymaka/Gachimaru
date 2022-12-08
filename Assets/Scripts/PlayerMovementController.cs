using System;
using Gachimaru.InputSystem;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Gachimaru.Gameplay
{
    public class PlayerMovementController : MonoBehaviour
    {
        //make it private
        [SerializeField] public Rigidbody _body;
        [SerializeField] private Player _player;
        
        [Header("Movement Settings")]
        [SerializeField] private float _maxSpeed;
        [SerializeField] private AnimationCurve SpeedUpCurve;
        [SerializeField] private float SpeedUpTimer = 0;
        [SerializeField] private float RotationSpeed;
        [Header("Jump Settings")]   
        [SerializeField] private float _jumpForce;
        [SerializeField] private bool _jumping;
        [SerializeField] private bool _falling => _player.GravityController._IsFalling;
        public bool _Jumping => _jumping;
        [Header("Rotation Settings")]
        private Vector3 _moveDirection;
        
        
        [SerializeField]  private Transform _cameraOrientation;

        public Vector3 _MoveDirection => _moveDirection;
        public Vector3 ForwardCameraOrientation => _cameraOrientation.forward;
        public Vector3 RightCameraOrientation => _cameraOrientation.right;
        public event Action OnJump;

        
        //General
        private float floatHorizontal;
        private float floatVertical;
        //makeit private
        private Vector3 _velocity;
        private float time;
        private bool stop;
        private bool _canMove;
        private bool _freezeY;
        private void Awake()
        {
            InputController.AddActionOnKey(KeyCode.Space, Jump);

            _player.AttackController.OnAttack += ForceStop;
            _player.AttackController.OnUnAttack += UnStop;
            //_player.SummonersDash.IsDashing += FreezeY;
            _player.SummonersDash.RemoveMoveSpeed += removeMoveSpeed;
            _player.SummonersDash.RestoreMoveSpeed += restoreMoveSpeed;
            _player.MovementController.OnJump += ChangeJumpingToTrue;
           // _player.GravityController.ReturnVelocity += restoreMoveSpeed;
            //_player.GravityController.ReturnVelocity += Move;
            //_player.GravityController.RemoveVelocity += ForceStop;
            // _player.GravityController.RemoveVelocity += FreezeY;
            

        }

        private void Start()
        {
            _jumping = false;
            _canMove = true;
        }

        private void FixedUpdate()
        {
            GetAxis();
            RotatePlayer();
            ChangeJumpingToFalse();
            if (_canMove)
            {
                Move();
            }
        }

        private void GetAxis()
        {
            floatHorizontal = _player.InputController.FloatAxisHorizontal;
            floatVertical = _player.InputController.FloatAxisVertical;
        }
        private void Move()
        {
                _moveDirection = (_cameraOrientation.forward * floatVertical +
                                 _cameraOrientation.right * floatHorizontal).normalized;
                
                Acceleration();
                if (!stop && !_freezeY)
                {
                    _body.velocity = _velocity;
                }
        }

        private void ChangeJumpingToFalse()
        {
            if (_falling)
            {
                _jumping = false;
            }
        }
        private void ChangeJumpingToTrue()
        {
            _jumping = true;
        }


        private void removeMoveSpeed()
        {
            _canMove = false;
            stop = true;
        }

        private void restoreMoveSpeed()
        {
            _canMove = true;
            stop = false;

        }
        private void RotatePlayer()
        {
            if (_moveDirection != Vector3.zero)
            {
                var rotationVelocity = new Vector3(_moveDirection.x, 0, _moveDirection.z);
                transform.forward = Vector3.Slerp(transform.forward, rotationVelocity,Time.deltaTime * RotationSpeed);
            }
        }
        private void Acceleration()
        {
            var forward = _body.transform.forward;
            var localMoveDirection = new Vector3(forward.x, 0, forward.z);
            
            _velocity = localMoveDirection * (_maxSpeed * SpeedUpCurve.Evaluate(SpeedUpTimer) * _moveDirection.magnitude);
            _velocity.y = _body.velocity.y;
            if (floatHorizontal == 0 && floatVertical == 0)
            {
                SpeedUpTimer = 0;
            }
            else
            {
                SpeedUpTimer += Time.deltaTime;
            }
        }

        private void Jump()
        {
            if (_player.GroundController.IsFullyGrounded)
            {
                OnJump?.Invoke();
                _body.AddForce(Vector3.up * _jumpForce);
            }

        }

        public void ApplyGravity(float gravityForce)
        {

            if (_player.GroundController._GroundedOnDash)
            {
                FreezeY();
                _freezeY = true;
            }
            else
            {
                _freezeY = false;
                _body.AddForce(Vector3.down * gravityForce);
            }
        }

        private void ForceStop()
        {
            _body.velocity = Vector3.zero;
            stop = true;
        }

        private void FreezeY()
        {
            _velocity = Vector3.zero;
            var velocity = _body.velocity;
            velocity.y = 0;
            _body.velocity = velocity;

        }
        private void UnStop()
        {
            _body.velocity = _velocity;
            stop = false;
        }
        
    }
}