using System;
using Gachimaru.InputSystem;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Gachimaru.Gameplay
{

    public class ReadPlayerState : MonoBehaviour
    {
        [SerializeField] private Player _player;
        //Player state
        [SerializeField] private bool _stateAttacking;
        [SerializeField] private bool _stateDashing;
        [SerializeField] private bool _stateShouldFalling;
        [SerializeField] private bool _stateUnGrounded;
        [SerializeField]  private bool _stateBarelyGrounded;
        [SerializeField]  private bool _stateIsFullyGrounded;
        [SerializeField]  private bool _stateEnableGravity;
        [SerializeField]  private bool _stateIsFalling;
        [SerializeField]  private bool _IsJumping;
        [SerializeField]  private bool _stateIsJumping;
        public event Action OnChangeJumpingToTrue;

        public bool StateEnableGravity => _stateEnableGravity;
        private void FixedUpdate()
        {
           // Debug.Log(_stateIsJumping);
           // Debug.Log(_IsJumping);
            UpdateStatements();
            UpdateStateEnableGravity();
        }

        private void UpdateStatements()
        {
            _stateAttacking = _player.AttackController.StateAttacking;
            _stateDashing = _player.SummonersDash.StateDashing;
            _stateShouldFalling = _player.GravityController._ShoudlFalling;
            _stateBarelyGrounded = _player.GroundController.StateBarelyGrounded;
            _stateIsFullyGrounded = _player.GroundController.StateIsFullyGrounded;
            _stateIsFalling = _player.GravityController._IsFalling;
            _IsJumping = _player.MovementController._Jumping;
        }
        
        


     
        
        private void UpdateStateEnableGravity()
        {
            if (!_stateAttacking && !_stateDashing)
            {
                _stateEnableGravity = true;
            }
            else
            {
                _stateEnableGravity = false;
            }
        }

    }
    
    
    
}