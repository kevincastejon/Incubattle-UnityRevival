using KevinCastejon.TimerEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimersController : MonoBehaviour
{
    [SerializeField] private Vector2 _movementThinkingDuration;
    [SerializeField] private Vector2 _delayBetweenThoughtsDuration;
    [SerializeField] private Vector2 _airThinkingDuration;
    [SerializeField] private Vector2 _combatThinkingDuration;
    [SerializeField] private Vector2 _holdThinkingDuration;
    [SerializeField] private Vector2 _throwThinkingDuration;

    private Timer _movementThinkingTimer = new Timer(1);
    private Timer _delayBetweenThoughtsTimer = new Timer(1);
    private Timer _airThinkingTimer = new Timer(1);
    private Timer _combatThinkingTimer = new Timer(1);
    private Timer _holdThinkingTimer = new Timer(1);
    private Timer _throwThinkingTimer = new Timer(1);

    private void Update()
    {
        _movementThinkingTimer.Update(Time.deltaTime);
        _delayBetweenThoughtsTimer.Update(Time.deltaTime);
        _airThinkingTimer.Update(Time.deltaTime);
        _combatThinkingTimer.Update(Time.deltaTime);
        _holdThinkingTimer.Update(Time.deltaTime);
        _throwThinkingTimer.Update(Time.deltaTime);
    }

    public void StartMovementThinking()
    {
        _movementThinkingTimer.Duration = Random.Range(_movementThinkingDuration.x, _movementThinkingDuration.y);
        _movementThinkingTimer.Play();
    }

    public void StopMovementThinking()
    {
        _movementThinkingTimer.Stop();
        _movementThinkingTimer.Clear();
    }

    public bool MovementThinkingEnded { get => _movementThinkingTimer.IsCompleted; }

    public void StartDelayBetweenThoughts()
    {
        _delayBetweenThoughtsTimer.Duration = Random.Range(_delayBetweenThoughtsDuration.x, _delayBetweenThoughtsDuration.y);
        _delayBetweenThoughtsTimer.Play();
    }
    public void StopDelayBetweenThoughts()
    {
        _delayBetweenThoughtsTimer.Stop();
        _delayBetweenThoughtsTimer.Clear();
    }
    public bool DelayBetweenThoughtsEnded { get => _delayBetweenThoughtsTimer.IsCompleted; }

    public void StartAirThinking()
    {
        _airThinkingTimer.Duration = Random.Range(_airThinkingDuration.x, _airThinkingDuration.y);
        _airThinkingTimer.Play();
    }
    public void StopAirThinking()
    {
        _airThinkingTimer.Stop();
        _airThinkingTimer.Clear();
    }
    public bool AirThinkingEnded { get => _airThinkingTimer.IsCompleted; }

    public void StartCombatThinking()
    {
        _combatThinkingTimer.Duration = Random.Range(_combatThinkingDuration.x, _combatThinkingDuration.y);
        _combatThinkingTimer.Play();
    }
    public void StopCombatThinking()
    {
        _combatThinkingTimer.Stop();
        _combatThinkingTimer.Clear();
    }
    public bool CombatThinkingEnded { get => _combatThinkingTimer.IsCompleted; }

    public void StartHoldThinking()
    {
        _holdThinkingTimer.Duration = Random.Range(_holdThinkingDuration.x, _holdThinkingDuration.y);
        _holdThinkingTimer.Play();
    }
    public void StopHoldThinking()
    {
        _holdThinkingTimer.Stop();
        _holdThinkingTimer.Clear();
    }
    public bool HoldThinkingEnded { get => _holdThinkingTimer.IsCompleted; }
    public void StartThrowThinking()
    {
        _throwThinkingTimer.Duration = Random.Range(_throwThinkingDuration.x, _throwThinkingDuration.y);
        _throwThinkingTimer.Play();
    }
    public void StopThrowThinking()
    {
        _throwThinkingTimer.Stop();
        _throwThinkingTimer.Clear();
    }
    public bool ThrowThinkingEnded { get => _throwThinkingTimer.IsCompleted; }
}
