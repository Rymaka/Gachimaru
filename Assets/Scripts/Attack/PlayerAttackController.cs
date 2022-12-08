using System;
using Gachimaru.InputSystem;
using UnityEngine;
using System.Collections;


namespace Gachimaru.Gameplay
{
    public class PlayerAttackController : SkillBase
    {
        [Header("Attack Settings")]
        [SerializeField] private float _attackDuration;
        [SerializeField] private float _attackTime;
        [SerializeField] private float _headshotMultiplier;
        [SerializeField] private float _damage;
        [SerializeField] private float _timeBetweenCombos;
        [SerializeField] private int _maxAttacks;
        [SerializeField] private int _currentAttacks;

        [SerializeField] private bool _onAttack;
        [SerializeField] private bool _coroutineIsStarted;


        [Header("Objects")]
        public Collider[] attackHitboxes;
        [SerializeField] private Player _player;


        //Events
        public event Action OnAttack;
        public event Action OnUnAttack;
        public event Action OnComboCD;
        public event Action OnCanCombo;

        //refer to player state
        public bool StateAttacking => _onAttack;

        private void Start()
        {
            ResetAttacks();
        }

        private void Update()
        {
            if (_currentAttacks >= _maxAttacks) //TODO: add logic to make sure that timer starts after attack + time between attacks
            {
                //to make sure that it starts only once
                if (!_coroutineIsStarted)
                {
                    StartCoroutine(AttackComboCooldown());
                }
            }
        }

        protected override void Cast()
        {
            PickAttackType();
        }



        private void ResetAttacks()
        {
            _currentAttacks = 0;
            OnCanCombo?.Invoke();
        }

        private void PickAttackType()
        {
            if (_player.GroundController.IsBarelyGrounded)
            {
                GroundCombo();
            }
            else
            {
                AirCombo();
            }
        }
        private void AirCombo()
        {
            //TODO: Make AirCombo logic
            if (_currentAttacks < _maxAttacks)
            {
                _currentAttacks++;
            }
            else
            {
                Debug.Log("КД атаки дурик");
            }
        }
        private void GroundCombo()
        {
            if (_currentAttacks < _maxAttacks)
            {
                Attack();
                _currentAttacks++;
            }
            else
            {
                Debug.Log("КД атаки дурик");
            }
        }



        private void Attack()
        {
            DealDamage(attackHitboxes[0]);
        }

        private void DealDamage(Collider col)
        {
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
                foreach (Collider c in cols)
                {
                    if (c.transform.parent.parent == transform)
                    {
                        continue;
                    }

                    switch (c.name)
                    {
                        case "Head":
                            _damage *= _headshotMultiplier;
                            Debug.Log("HeadDamage");
                            break;
                        default:
                            _damage = _damage;
                            Debug.Log("DefaultDamage");
                            break;
                    }

                    c.SendMessageUpwards("TakeDamage", _damage);
                }
            
        }
        
        
        private IEnumerator AttackComboCooldown()
        {
            _coroutineIsStarted = true;
            OnComboCD?.Invoke();
            yield return new WaitForSeconds(_timeBetweenCombos);
            ResetAttacks();
            _coroutineIsStarted = false;
        }
    }
}

    


