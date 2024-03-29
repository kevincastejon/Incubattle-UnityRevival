using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private AnimationCurve _horizontalShake;
    [SerializeField] private AnimationCurve _verticalShake;
    [SerializeField] private AnimationCurve _depthShake;
    [Min(0.001f)] [SerializeField] private float _duration = 0.5f;
    [SerializeField] private bool _useXAxis;
    [SerializeField] private bool _useYAxis;
    [SerializeField] private bool _useZAxis;
    [SerializeField] private bool _autoRun;
    [SerializeField] private Transform _target;

    private bool _running;
    private float _endTime;
    private Vector3 _backupPosition;

    private void Start()
    {
        if (_autoRun)
        {
            Shake();
        }
    }

    public void Shake()
    {
        if (_running)
        {
            return;
        }
        _running = true;
        _endTime = Time.time + _duration;
        _backupPosition = _target.position;
    }

    private void Update()
    {
        if (!_running)
        {
            return;
        }
        float progress = GetProgress();
        Vector3 motion = new Vector3(_useXAxis ? _horizontalShake.Evaluate(progress) : 0f, _useYAxis ? _verticalShake.Evaluate(progress) : 0f, _useZAxis ? _depthShake.Evaluate(progress) : 0f);
        _target.Translate(motion, Space.Self);
        if (Time.time >= _endTime)
        {
            _target.localPosition = _backupPosition;
            _running = false;
        }
    }
    private float GetProgress()
    {
        float elapsedTime = Time.time - (_endTime - _duration);
        float progress = elapsedTime / _duration;
        return progress;
    }
}
