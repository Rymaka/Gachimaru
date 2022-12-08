using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gachimaru.Gameplay
{
    public class CharacterAI : MonoBehaviour
    {
        [Inject] private CharactersContainer _charactersContainer;
        
        public LayerMask WhatIsGround;
        private NavMeshAgent _agent;
        private Group _aggressiveGroup;
        private Group _friendGroup;
        
        [SerializeField] private float _sightRange;
        [SerializeField] private float stopRange;

        [SerializeField] private bool _chasing;

        [SerializeField] private Color color;
        [SerializeField] private Color color2;

        private CharacterBase _chasingCharacter;

        public void Init(Group friendGroup, Group aggressiveGroup)
        {
            _aggressiveGroup = aggressiveGroup;
            _friendGroup = friendGroup;
        }
        
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        //TODO: Сделать отдельный базовый класс MelleAttack, в котором будет логика нанесения урона CharacterBase'у
        
        private void Update()
        {
            //TODO: обработать кейс, когда не найден никто
            if (!TryFindTarget(_aggressiveGroup))
            {
                TryFindTarget(_friendGroup);
            }
            
            LookAtTarget();
            
            StayIdleIfPlayerNearbyAndNoEnemy();
        }

        private bool TryFindTarget(Group group)
        {
            foreach (var character in _charactersContainer.Characters)
            {
                if (!character.CharacterGroup.Equals(group)) continue;
                if (!IsVisible(character)) continue;

                _chasingCharacter = character;
                _agent.SetDestination(_chasingCharacter.transform.position);
                return true;
            }

            return false;
        }

        private bool IsDestinationReached()
        {
            return (_chasingCharacter.transform.position - transform.position).magnitude < stopRange;
        }
        
        private bool IsVisible(CharacterBase lookingCharacter)
        {
            return (lookingCharacter.transform.position - transform.position).magnitude < _sightRange;
        }

        private void StayIdleIfPlayerNearbyAndNoEnemy()
        {
            if (IsDestinationReached())
            {
                StayIdle();
                _chasing = false;
            }
            else
            {
                _agent.speed = 3.5f;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = color2;
            //Gizmos.DrawSphere(transform.position, attackRange);
        }

        private void Disapear()
        {
            _chasing = false;
            Destroy(gameObject);
        }

        private void LookAtTarget()
        {
            transform.LookAt(_chasingCharacter.transform.position);
        }

        private void StayIdle()
        {
            _chasing = false;
            _agent.velocity = Vector3.zero;
            _agent.ResetPath();
        }
    }
}

