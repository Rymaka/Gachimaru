using Gachimaru.Gameplay;
using UnityEditor.Animations;
using UnityEngine;

public class CastAnimation : MonoBehaviour
{
    [SerializeField] private SkillBase _skill;
    [SerializeField] private Animator _animator;

    [SerializeField] private string _startCastTrigger;
    [SerializeField] private string _finishCastTrigger;
    [SerializeField] private string _interruptCastTrigger;
    
    private int StartCast;
    private int FinishCast;
    private int InterruptCast;

    private void Awake()
    {
        _skill.OnStartCast += OnStartCast;
        _skill.OnFinishCast += OnFinishCast;
        _skill.OnCastInterrupted += OnCastInterrupted;

        StartCast = Animator.StringToHash(_startCastTrigger);
        FinishCast = Animator.StringToHash(_finishCastTrigger);
        InterruptCast = Animator.StringToHash(_interruptCastTrigger);
    }

    private void OnStartCast()
    {
        _animator.SetTrigger(StartCast);
    }
    
    private void OnFinishCast()
    {
        _animator.SetTrigger(FinishCast);
    }

    private void OnCastInterrupted()
    {
        _animator.SetTrigger(InterruptCast);
    }
}
