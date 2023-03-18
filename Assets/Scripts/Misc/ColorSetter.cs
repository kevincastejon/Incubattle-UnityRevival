using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color[] _colors;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(int colorId)
    {
        _spriteRenderer.color = _colors[colorId];
    }
    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
