using UnityEngine;

namespace Gachimaru.Gameplay
{
    public class AttackAnimationScript : SkillBase
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameObject _sword;
        [SerializeField] private bool _canAttack = true;
        private Animator _anim;

        private void Awake()
        {
            //_anim = _sword.GetComponent<Animator>();
            _player.AttackController.OnComboCD += AttackAnimationCD;
            _player.AttackController.OnCanCombo += AttackAnimationIsReady;
        }
        
        private void AttackAnimationCD()
        {
            _canAttack = false;
        }
        
        private void AttackAnimationIsReady()
        {
            Debug.Log("CanAttack");
            _canAttack = true;
        }
        protected override void Cast()
        {
            Debug.Log("WORK PLEASE");
            SwordAttack();
        }

        private void SwordAttack()
        {
            if (_canAttack)
            {
                Debug.LogError("Attacked");
            }
        }
    }
}