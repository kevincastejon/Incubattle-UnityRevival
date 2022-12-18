using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundCompleteEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent _onComplete;
    private AudioSource _audioSource;
    private bool _isEnded;

    public bool IsEnded { get => _isEnded; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isEnded && !_audioSource.isPlaying)
        {
            _isEnded = true;
            _onComplete.Invoke();
        }
    }
}
