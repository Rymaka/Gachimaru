using System;
using Gachimaru.InputSystem;
using UnityEngine;
using Zenject;

namespace Gachimaru.Gameplay
{
    public class Player : CharacterBase
    {
        [Inject] private CharactersContainer _charactersContainer;
        
        public PlayerGravityController GravityController => _gravityController;
        public InputController InputController => _inputController;
        public PlayerAttackController AttackController => _attackController;
        public PlayerGroundController GroundController => _groundController;
        public FirstSummonersSkill FirstSummonersSkill => _firstSummonersSkill;
        public SummonersDash SummonersDash => _summonersDash;
        public ReadPlayerState ReadPlayerState => playerState;
        public BlinkShaderSkript BlinkShaderSkript => _blinkShaderSkript;

        [SerializeField] private PlayerGravityController _gravityController;
        [SerializeField] private PlayerAttackController _attackController;
        [SerializeField] private PlayerGroundController _groundController;
        [SerializeField] private InputController _inputController;
        [SerializeField] private FirstSummonersSkill _firstSummonersSkill;
        [SerializeField] private SummonersDash _summonersDash;
        [SerializeField] private ReadPlayerState playerState;
        [SerializeField] private BlinkShaderSkript _blinkShaderSkript;

        protected override void OnAwake()
        {
            _charactersContainer.AddCharacter(this);
        }
    }
}

