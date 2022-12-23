using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private string _hitTrigger;
    [SerializeField] private string _breakTrigger;
    [SerializeField] private bool _playHitSound = true;
    [SerializeField] private bool _playBreakSound = true;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _breakSound;
    [SerializeField] private UnityEvent _onHit;
    [SerializeField] private UnityEvent _onBreak;
    [SerializeField] private UnityEvent _onHitSound;
    [SerializeField] private UnityEvent _onBreakSound;
    private AudioSource _sfxSource;
    private Animator _animator;
    private VFXController _vfxController;

    public int Health { get => _health; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _vfxController = GetComponentInChildren<VFXController>();
        _sfxSource = GameObject.FindGameObjectWithTag("SfxSource").GetComponent<AudioSource>();
    }
    public void PlayHitSound()
    {
        if (_hitSound)
        {
            _sfxSource.PlayOneShot(_hitSound);
        }
        _onBreakSound.Invoke();
    }
    public void PlayBreakSound()
    {
        if (_breakSound)
        {
            _sfxSource.PlayOneShot(_breakSound);
        }
        _onHitSound.Invoke();
    }
    public void Hit(Collider2D collider)
    {
        _vfxController.TriggerPunch(collider.ClosestPoint(transform.position));
        if (_playHitSound)
        {
            PlayHitSound();
        }
        if (_health == 0)
        {
            return;
        }
        _health--;
        if (_health == 0)
        {
            _onHit.Invoke();
            Break();
        }
        else
        {
            if (!string.IsNullOrEmpty(_hitTrigger))
            {
                _animator.SetTrigger(_hitTrigger);
            }
            _onHit.Invoke();
        }
    }
    private void Break()
    {
        _onBreak.Invoke();
        if (!string.IsNullOrEmpty(_breakTrigger))
        {
            _animator.SetTrigger(_breakTrigger);
        }
        if (_playBreakSound)
        {
            PlayBreakSound();
        }
    }
}
