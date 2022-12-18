using System;
using UnityEngine;

[Serializable]
public struct ColorStage
{
    [SerializeField] private float _duration;
    [SerializeField] private Color _color;

    public ColorStage(float duration, Color color)
    {
        _duration = duration;
        _color = color;
    }

    public float Duration { get => _duration; set => _duration = value; }
    public Color Color { get => _color; set => _color = value; }
}
