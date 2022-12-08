using System;
using UnityEngine;

namespace Gachimaru.Gameplay
{
    public class PlayerGroundController : MonoBehaviour
    {
        [SerializeField] private Transform _rayTransform;
        [SerializeField] private float _rayLength;
        [SerializeField] private float _fullyGroundedDistance;
        [SerializeField] private float _barelyGroundedDistance;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Player _player;

        //Fully grounded means that the character is hits the ground with the smallest raycast
        //with the IsFullyGrounded = true character can jump
        public bool IsFullyGrounded => _isFullyGrounded;
        [SerializeField] private bool _isFullyGrounded;
        //Barely grounded means that the character is above the fully grounded point but still not fully ungrounded
        public bool IsBarelyGrounded => _isBarelyGrounded;
        [SerializeField] private bool _isBarelyGrounded;
        //if not grounded and not barely grounded
        [SerializeField] private bool _isUnGrounded;
        
        
        [SerializeField] private bool _isGroundedOnDash;
        public bool _GroundedOnDash;
        [SerializeField] private Color color;
        [SerializeField] private float _groundOnDash;
        
        
        //refer to player state
        public bool StateUnGrounded => _isUnGrounded;
        public bool StateBarelyGrounded => _isBarelyGrounded;
        public bool StateIsFullyGrounded => _isFullyGrounded;
        
        //Actions
        public event Action OnGround;
        public event Action OnBarelyGround;
        public event Action OnUnground;
        

        
        private void FixedUpdate()
        {
            UpdateGround();
            DrawDebugRay();
        }

        private void UpdateGround()
        {
            FullyGrounded();
            BarelyGrounded();
            UnGrounded();
            GroundedOnDash();
        }


        private void FullyGrounded()
        {
            var rayStartPosition = _rayTransform.position;
            Physics.Raycast(rayStartPosition, Vector3.down, out var hit, _rayLength, _groundMask);
            var hitPosition = hit.point;

            var isGrounded = (hitPosition - rayStartPosition).magnitude < _fullyGroundedDistance;

            if (isGrounded)
            {
                OnGround?.Invoke();
                _isFullyGrounded = true;
                
            }
            else
            {
                _isFullyGrounded = false;
            }
            
        }
        
        private void BarelyGrounded()
        {
            var rayStartPosition = _rayTransform.position;
            Physics.Raycast(rayStartPosition, Vector3.down, out var hit, _rayLength, _groundMask);
            var hitPosition = hit.point;

            var isBarelyGrounded = (hitPosition - rayStartPosition).magnitude < _barelyGroundedDistance;

            if (isBarelyGrounded)
            {
                OnBarelyGround?.Invoke();
                _isBarelyGrounded = true;
                
            }
            else
            {
                _isBarelyGrounded = false;
            }
            
        }

        private void UnGrounded()
        {
            if (!_isBarelyGrounded && !_isFullyGrounded)
            {
                _isUnGrounded = true;
                OnUnground?.Invoke();

            }
            else
            {
                _isUnGrounded = false;
            }

        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(_rayTransform.position, _groundOnDash);
        }

        private void GroundedOnDash()
        {
            _isGroundedOnDash = Physics.CheckSphere(_rayTransform.position, _groundOnDash, _groundMask);
            if (_isGroundedOnDash && _player.SummonersDash._IsDashing && !_player.MovementController._Jumping )
            {
                _GroundedOnDash = true;
            }
            else
            {
                _GroundedOnDash = false;
            }
            
        }
        
        private void DrawDebugRay()
        {
            Debug.DrawRay(_rayTransform.position, Vector3.down * _rayLength, Color.red);
            Debug.DrawRay(_rayTransform.position, Vector3.down * _barelyGroundedDistance, Color.yellow);
            Debug.DrawRay(_rayTransform.position, Vector3.down * _fullyGroundedDistance, Color.blue);


        }
    }
}

