using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [SerializeField] private AudioClip _hit;
    [SerializeField] private AudioClip _hurt;
    [SerializeField] private AudioClip _throw;
    [SerializeField] private AudioClip _throwHit;
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _slam;
    [SerializeField] private AudioClip _special;
    [SerializeField] private AudioClip _specialBoom;
    [SerializeField] private AudioClip _dead;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void TriggerHit()
    {
        _audioSource.PlayOneShot(_hit);
    }
    
    public void TriggerHurt()
    {
        _audioSource.PlayOneShot(_hurt);
    }

    public void TriggerThrow()
    {
        _audioSource.PlayOneShot(_throw);
    }

    public void TriggerThrowHit()
    {
        _audioSource.PlayOneShot(_throwHit);
    }

    public void TriggerJump()
    {
        _audioSource.PlayOneShot(_jump);
    }

    public void TriggerSlam()
    {
        _audioSource.PlayOneShot(_slam);
    }

    public void TriggerSpecial()
    {
        _audioSource.PlayOneShot(_special);
    }

    public void TriggerSpecialBoom()
    {
        _audioSource.PlayOneShot(_specialBoom);
    }

    public void TriggerDead()
    {
        _audioSource.PlayOneShot(_dead);
    }
}
