using System;
using UnityEngine;

namespace Gachimaru.Gameplay
{
    public class FirstSummonersSkill : SkillBase
    {
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private SummonBase _summoningPrefab;
        [SerializeField] private Group _aggressiveGroup;

        private Vector3 Orientation => Caster.MovementController.ForwardCameraOrientation;
        public event Action OnSummon;

        protected override void Cast()
        {
            OnSummon?.Invoke();
            Vector3 inputDirection = Orientation;
            Vector3 aimDir = (inputDirection - _spawnPosition.position).normalized;
            var summon = Instantiate(_summoningPrefab, _spawnPosition.position, 
                Quaternion.LookRotation(aimDir.normalized, Vector3.zero));
            summon.Init(Caster.CharacterGroup, _aggressiveGroup);
        }
    }
}