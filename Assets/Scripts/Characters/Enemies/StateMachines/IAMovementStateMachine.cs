using KevinCastejon.MoreAttributes;
using System.Collections.Generic;
using UnityEngine;

public enum IAMovementState
{
    IDLE,
    HUNTING,
    FLEEING,
}

public class IAMovementStateMachine : MonoBehaviour
{
    [SerializeField] [ReadOnly] private IAMovementState _currentState;
    [SerializeField] private WeightedRandomPicker _near;
    [SerializeField] private WeightedRandomPicker _far;
    private Animator _animator;
    private EnemyController _enemyController;
    private TimersController _timersController;
    private IACombatStateMachine _combatStateMachine;
    private IAHealthStateMachine _healthStateMachine;

    public IAMovementState CurrentState { get => _currentState; private set => _currentState = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();
        _timersController = GetComponentInChildren<TimersController>();
        _combatStateMachine = GetComponent<IACombatStateMachine>();
        _healthStateMachine = GetComponent<IAHealthStateMachine>();
    }
    private void Start()
    {
        CurrentState = IAMovementState.IDLE;
        OnEnterIdle();
    }
    private void Update()
    {
        OnStateUpdate(CurrentState);
    }
    private void FixedUpdate()
    {
        OnStateFixedUpdate(CurrentState);
    }

    private void OnStateEnter(IAMovementState state)
    {
        switch (state)
        {
            case IAMovementState.IDLE:
                OnEnterIdle();
                break;
            case IAMovementState.HUNTING:
                OnEnterHunting();
                break;
            case IAMovementState.FLEEING:
                OnEnterFleeing();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(IAMovementState state)
    {
        switch (state)
        {
            case IAMovementState.IDLE:
                OnUpdateIdle();
                break;
            case IAMovementState.HUNTING:
                OnUpdateHunting();
                break;
            case IAMovementState.FLEEING:
                OnUpdateFleeing();
                break;
        }
    }
    private void OnStateFixedUpdate(IAMovementState state)
    {
        switch (state)
        {
            case IAMovementState.IDLE:
                OnFixedUpdateIdle();
                break;
            case IAMovementState.HUNTING:
                OnFixedUpdateHunting();
                break;
            case IAMovementState.FLEEING:
                OnFixedUpdateFleeing();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(IAMovementState state)
    {
        switch (state)
        {
            case IAMovementState.IDLE:
                OnExitIdle();
                break;
            case IAMovementState.HUNTING:
                OnExitHunting();
                break;
            case IAMovementState.FLEEING:
                OnExitFleeing();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(IAMovementState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterIdle()
    {
        _animator.SetBool("IsIdle", true);
        _timersController.StartMovementThinking();
    }
    private void OnUpdateIdle()
    {
        if (_timersController.MovementThinkingEnded)
        {
            if (_combatStateMachine.CurrentState == IACombatState.PEACEFULL && _healthStateMachine.CurrentState == IAHealthState.AWAKE)
            {
                if (_enemyController.IsTargetReachable)
                {
                    string pickedName = _near.PickName();
                    if (pickedName == "FLEE")
                    {
                        TransitionToState(IAMovementState.FLEEING);
                        return;
                    }
                    else
                    {
                        _timersController.StopMovementThinking();
                        _timersController.StartMovementThinking();
                        return;
                    }
                }
                else
                {
                    string pickedName = _far.PickName();
                    if (pickedName == "FLEE")
                    {
                        TransitionToState(IAMovementState.FLEEING);
                        return;
                    }
                    else if (pickedName == "IDLE")
                    {
                        _timersController.StopMovementThinking();
                        _timersController.StartMovementThinking();
                        return;
                    }
                    else
                    {
                        TransitionToState(IAMovementState.HUNTING);
                        return;
                    }
                }
            }
            else
            {
                _timersController.StopMovementThinking();
                _timersController.StartMovementThinking();
                return;
            }
        }
    }
    private void OnFixedUpdateIdle()
    {
        _enemyController.DoIdle();
    }
    private void OnExitIdle()
    {
        _timersController.StopMovementThinking();
        _animator.SetBool("IsIdle", false);
    }

    private void OnEnterHunting()
    {
        _enemyController.PickTarget();
        _timersController.StartDelayBetweenThoughts();
        _animator.SetBool("IsWalking", true);
    }
    private void OnUpdateHunting()
    {
        if (_enemyController.IsTargetReachable || _timersController.DelayBetweenThoughtsEnded || _combatStateMachine.CurrentState != IACombatState.PEACEFULL || _healthStateMachine.CurrentState != IAHealthState.AWAKE)
        {
            TransitionToState(IAMovementState.IDLE);
            return;
        }
        _enemyController.DoTurnCharacter();
    }
    private void OnFixedUpdateHunting()
    {
        _enemyController.DoHunt();
    }
    private void OnExitHunting()
    {
        _timersController.StopDelayBetweenThoughts();
        _animator.SetBool("IsWalking", false);
    }

    private void OnEnterFleeing()
    {
        _timersController.StartDelayBetweenThoughts();
        _animator.SetBool("IsWalking", true);
    }
    private void OnUpdateFleeing()
    {
        if (_timersController.DelayBetweenThoughtsEnded || _combatStateMachine.CurrentState != IACombatState.PEACEFULL || _healthStateMachine.CurrentState != IAHealthState.AWAKE)
        {
            TransitionToState(IAMovementState.IDLE);
            return;
        }
        _enemyController.DoTurnCharacter();
    }
    private void OnFixedUpdateFleeing()
    {
        _enemyController.DoFlee();
    }
    private void OnExitFleeing()
    {
        _timersController.StopDelayBetweenThoughts();
        _animator.SetBool("IsWalking", false);
    }
}
