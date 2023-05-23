using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerSwitcher : MonoBehaviour
{
    [SerializeField] private PrefabVariable _p1;
    [SerializeField] private RuntimeAnimatorController[] _controllers;
    [SerializeField] private List<GameObject> _players;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.runtimeAnimatorController = _controllers[_players.IndexOf(_p1.Value)];
    }
}
