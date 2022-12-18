using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour, ILevelEvent
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerParameter;
    private bool _animationEnded;
    public void Init()
    {
        _animator.SetTrigger(_triggerParameter);
    }

    public bool IsOver()
    {
        return _animationEnded;
    }

    public void AnimationOver()
    {
        _animationEnded = true;
    }
}
