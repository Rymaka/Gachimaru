using System;
using System.Collections;
using System.Collections.Generic;
using Gachimaru.InputSystem;
using UnityEngine;

namespace Gachimaru.Gameplay
{
    public class SkillBase : MonoBehaviour
    {
        [SerializeField] private KeyCode _key;
        [SerializeField] protected float Cooldown;
        [field:SerializeField] public CharacterBase Caster { get; protected set; }
        [SerializeField] private List<SkillBase> _interruptibleSkills;
        [SerializeField] protected float CastDuration;

        public event Action OnStartCast;
        public event Action OnFinishCast;
        public event Action OnCastInterrupted;
        
        private bool _isCooldown = false;
        private bool _isCasting = false;

        private IEnumerator _castingRoutine;

        private void Awake()
        {
            InputController.AddActionOnKey(_key, TryCast);
            OnAwake();
        }

        protected virtual void OnAwake() { }

        private void TryCast()
        {
            if (IsCanCast() && !_isCooldown)
            {
                foreach (var interruptibleSkill in _interruptibleSkills)
                {
                    interruptibleSkill.TryInterruptCast();
                }
                
                OnStartCast?.Invoke();

                _castingRoutine = Casting();
                StartCoroutine(_castingRoutine);
                StartCoroutine(CooldownRoutine());
            }
        }

        private void TryInterruptCast()
        {
            if (!_isCasting) return;
            
            StopCoroutine(_castingRoutine);
            InterruptCast();
            OnCastInterrupted?.Invoke();
        }

        private IEnumerator Casting()
        {
            _isCasting = true;
            yield return new WaitForSeconds(CastDuration);
            
            Cast();
            OnFinishCast?.Invoke();
            
            _isCasting = false;
        }

        private IEnumerator CooldownRoutine()
        {
            _isCooldown = true;
            yield return new WaitForSeconds(Cooldown);
            _isCooldown = false;
        }

        protected virtual bool IsCanCast()
        {
            return true;
        }
        protected virtual void Cast(){}
        
        protected virtual void InterruptCast(){}
    }
}
