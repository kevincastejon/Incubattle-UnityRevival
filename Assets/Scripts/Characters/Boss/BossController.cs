using KevinCastejon.MoreAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Transform _body;
    [SerializeField] Transform _leftHand;
    [SerializeField] Transform _rightHand;

    [SerializeField] float _speed = 0.5f;
    [SerializeField] float _attackDuration = 1.5f;
    [SerializeField] float _thinkingDuration = 2f;
    [SerializeField] float _risingDuration = 2f;
    [SerializeField] float _delayBetweenThoughtsDuration = 5f;
    [SerializeField] [ReadOnly] private int _health = 10;
    [SerializeField] AnimationCurve _riseCurve;
    [SerializeField] AnimationCurve _hitCurve;

    private float _attackEndTime;
    private float _thinkingEndTime;
    private float _risingEndTime;
    private float _nextThoughtTime;
    private Rigidbody2D _rigidbody;
    private Transform _target;
    private GameHUD _gameHUD;

    public bool IsRised { get; private set; }
    public bool IsThinkingEnded { get => Time.time >= _thinkingEndTime; }
    public bool IsRisingEnded { get => Time.time >= _risingEndTime; }
    public bool IsThinkingTime { get => Time.time >= _nextThoughtTime; }
    public bool IsAttackEnded { get => Time.time >= _attackEndTime; }
    public bool IsTargetReachable { get => Vector2.Distance(_target.position, transform.position) < 1f; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _gameHUD = FindObjectOfType<GameHUD>();
    }

    public void FindTarget()
    {
        _target = FindObjectOfType<PlayerController>().transform;
    }

    public void StartThinking()
    {
        _thinkingEndTime = _thinkingDuration + Time.time;
    }
    public void EndThink()
    {
        _nextThoughtTime = Time.time + _delayBetweenThoughtsDuration;
    }

    public void DoIdle()
    {
        _rigidbody.velocity = Vector2.zero;
    }
    public void DoWalk()
    {
        _rigidbody.velocity = new Vector2(Random.value, Random.value).normalized * _speed;
    }
    public void DoHunt()
    {
        _rigidbody.velocity = (_target.position - transform.position).normalized * _speed;
    }
    public void StartRise()
    {
        IsRised = true;
        _risingEndTime = _risingDuration + Time.time;
    }
    public void DoRise()
    {
        // On calcule la progression de la montée
        float risingProgress = (_risingDuration - (_risingEndTime - Time.time)) / _risingDuration;
        _body.localPosition = new Vector2(0f, _riseCurve.Evaluate(risingProgress));
    }

    public void StartHit()
    {
        _attackEndTime = _attackDuration + Time.time;
    }
    public void DoHit()
    {
        // On calcule la progression de l'attaque
        float hitProgress = (_attackDuration - (_attackEndTime - Time.time)) / _attackDuration;
        _body.localPosition = new Vector2(0f, _hitCurve.Evaluate(hitProgress));
    }
    public void Hurt()
    {
        _health--;
        if (_health == 0)
        {
            Destroy(gameObject);
            _gameHUD.ShowVictoryMenu();
        }
    }
}
