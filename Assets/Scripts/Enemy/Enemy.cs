using UnityEngine;


namespace Gachimaru.Gameplay
{
    public class Enemy : CharacterBase
    {
        public EnemyGravityScript EnemyGravityScript => _enemyGravityScript;

        [SerializeField] private EnemyGravityScript _enemyGravityScript;
        [SerializeField] private CharacterAI _characterAI;

        protected override void OnAwake()
        {
            _characterAI.Init(Group.Enemy, Group.Player);
        }
    }
}