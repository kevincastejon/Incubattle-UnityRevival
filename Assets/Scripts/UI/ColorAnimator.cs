using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorAnimator : MonoBehaviour
{
    [SerializeField] private ColorStage[] _colorStages;
    [SerializeField] private UnityEvent<Color> _onColorChange;
    [SerializeField] private UnityEvent<Color> _onComplete;
    [SerializeField] private Color _currentColor;
    [SerializeField] private int _cycleCount;
    [SerializeField] private bool _autoRun;
    [SerializeField] private bool _stopAtNextCycle;

    private int _currentCount;
    private int _currentStage = -1;
    private int _nextStage = 0;
    private float _nextStageTime;
    private bool _isStarted;
    private bool _isCompleted;

    public Color CurrentColor { get => _currentColor; }
    public UnityEvent<Color> OnColorChange { get => _onColorChange; }

    private void Start()
    {
        if (_autoRun)
        {
            Init();
        }
    }
    public void Init()
    {
        _isStarted = true;
        _isCompleted = false;
        _currentCount = 0;
        _currentColor = _colorStages[0].Color;
        SwitchToNextStage();
    }

    private void SwitchToNextStage()
    {
        _currentStage = _nextStage;
        _nextStage++;
        if (_nextStage >= _colorStages.Length)
        {
            _nextStage = 0;
            _currentCount++;
            if (_cycleCount > 0 && _currentCount >= _cycleCount)
            {
                _isCompleted = true;
                _onComplete.Invoke(_currentColor);
                return;
            }
        }
        _nextStageTime = Time.time + _colorStages[_currentStage].Duration;
    }

    private void Update()
    {
        if (!_isStarted || _isCompleted)
        {
            return;
        }
        if (Time.time > _nextStageTime)
        {
            
            SwitchToNextStage();
        }
        else if (!_stopAtNextCycle)
        {
            _currentColor = Color.Lerp(_colorStages[_currentStage].Color, _colorStages[_nextStage].Color, GetProgress());
            _onColorChange.Invoke(_currentColor);
        }
        if (_stopAtNextCycle)
        {
            _currentColor = Color.Lerp(_colorStages[_currentStage].Color, _colorStages[_nextStage].Color, GetProgress());
            _onColorChange.Invoke(_currentColor);
        }
    }
    private float GetProgress()
    {
        float elapsedTime = Time.time - (_nextStageTime - _colorStages[_nextStage].Duration);
        float progress = elapsedTime / _colorStages[_nextStage].Duration;
        return progress;
    }

    public void Reset()
    {
        _colorStages = new ColorStage[] { new ColorStage(1f, Color.white), new ColorStage(1f, Color.black) };
    }
}
