using KevinCastejon.MoreAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandController : MonoBehaviour
{
    [SerializeField] private float _attackDuration = 1f;
    [SerializeField] private AnimationCurve _attackEasing;
    [SerializeField] private Collider2D _attackBox;
    [SerializeField] private Collider2D _playerDetector;
    [SerializeField] private GameObject _shadow;
    [SerializeField] private LayerMask _playerLayer;
    private ScreenShaker _shaker;
    private ColorAnimator _colorAnimator;

    [SerializeField] [ReadOnly]private int _health = 10;
    private float _attackEndTime;

    public bool IsAttackEnded { get => Time.time >= _attackEndTime; }
    public bool IsPlayerDetected { get => _playerDetector.IsTouchingLayers(_playerLayer); }
    
    private void Awake()
    {
        _shaker = FindObjectOfType<ScreenShaker>();
        _colorAnimator = GetComponent<ColorAnimator>();
    }

    public void StartHit()
    {
        _attackEndTime = Time.time + _attackDuration;
    }
    public void DoHit()
    {
        float hitProgress = (_attackDuration - (_attackEndTime - Time.time)) / _attackDuration;
        if (!_attackBox.enabled && hitProgress > 0.24f && hitProgress < 0.26f)
        {
            _shaker.Shake();
        }
        _attackBox.enabled = hitProgress > 0.24f && hitProgress < 0.26f;
        transform.localPosition = new Vector2(transform.localPosition.x, _attackEasing.Evaluate(hitProgress));
    }
    public void StopHit()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, _attackEasing.Evaluate(0f));
    }
    public void Hurt()
    {
        _colorAnimator.Init(); 
        _health--;
        if (_health == 0)
        {
            Destroy(_shadow);
            Destroy(gameObject);
        }
    }
}
