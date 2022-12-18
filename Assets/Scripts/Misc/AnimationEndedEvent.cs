using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEndedEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent _onAnimationEnd;

    public void AnimationEnded()
    {
        _onAnimationEnd.Invoke();
    }
}
