using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [Min(0.001f)] [SerializeField] private float _duration = 0.5f;
    [SerializeField] private SpriteRenderer _mask;
    [SerializeField] private bool _autoRun;
    private float _endTime;
    private bool _running;

    private void Awake()
    {
        _mask.enabled = true;
    }

    private void Start()
    {
        if (_autoRun)
        {
            FadeIn();
        }
    }
    public void FadeIn()
    {
        _endTime = Time.time + _duration;
        _running = true;
    }

    private void Update()
    {
        if (!_running)
        {
            return;
        }
        _mask.color = new Color(_mask.color.r, _mask.color.g, _mask.color.b, 1f - GetProgress());
        if (Time.time > _endTime)
        {
            _mask.color = new Color(_mask.color.r, _mask.color.g, _mask.color.b, 0f);
            _running = true;
        }
    }
    private float GetProgress()
    {
        float elapsedTime = Time.time - (_endTime - _duration);
        float progress = elapsedTime / _duration;
        return progress;
    }
}
