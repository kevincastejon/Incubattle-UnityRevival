using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorAnimator : MonoBehaviour
{
    [SerializeField] private ColorStage[] _colorStages;
    [SerializeField] private UnityEvent<Color> _onColorChange;
    [SerializeField] private Color _currentColor;

    private int _currentStage = -1;
    private int _nextStage = 0;
    private float _nextStageTime;

    public Color CurrentColor { get => _currentColor; }
    public UnityEvent<Color> OnColorChange { get => _onColorChange; }

    private void Start()
    {
        _currentColor = _colorStages[0].Color;
        SwitchToNextStage();
    }

    private void SwitchToNextStage()
    {
        _currentStage = _nextStage;
        _nextStage = _nextStage + 1 == _colorStages.Length ? 0 : _nextStage + 1;
        _nextStageTime = Time.time + _colorStages[_nextStage].Duration;
    }

    private void Update()
    {
        if (Time.time > _nextStageTime)
        {
            SwitchToNextStage();
        }
        _currentColor = Color.Lerp(_colorStages[_currentStage].Color, _colorStages[_nextStage].Color, GetProgress());
        _onColorChange.Invoke(_currentColor);
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
