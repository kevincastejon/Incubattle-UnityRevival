using KevinCastejon.MoreAttributes;
using UnityEngine;

public enum IAHoldState
{
    HANDS_FREE,
    PICKING,
    HOLDING,
    THROWING,
}

public class IAHoldStateMachine : MonoBehaviour
{
    [SerializeField] [ReadOnly] private IAHoldState _currentState;
    [SerializeField] private WeightedRandomPicker _pick;
    [SerializeField] private WeightedRandomPicker _throw;
    private Animator _animator;
    private EnemyController _enemyController;
    private TimersController _timersController;
    private IACombatStateMachine _combatStateMachine;
    private IAHealthStateMachine _healthStateMachine;
    public IAHoldState CurrentState { get => _currentState; private set => _currentState = value; }

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
        CurrentState = IAHoldState.HANDS_FREE;
        OnEnterHandsFree();
    }
    private void Update()
    {
        OnStateUpdate(CurrentState);
    }
    private void FixedUpdate()
    {
        OnStateFixedUpdate(CurrentState);
    }

    private void OnStateEnter(IAHoldState state)
    {
        switch (state)
        {
            case IAHoldState.HANDS_FREE:
                OnEnterHandsFree();
                break;
            case IAHoldState.PICKING:
                OnEnterPicking();
                break;
            case IAHoldState.HOLDING:
                OnEnterHolding();
                break;
            case IAHoldState.THROWING:
                OnEnterThrowing();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(IAHoldState state)
    {
        switch (state)
        {
            case IAHoldState.HANDS_FREE:
                OnUpdateHandsFree();
                break;
            case IAHoldState.PICKING:
                OnUpdatePicking();
                break;
            case IAHoldState.HOLDING:
                OnUpdateHolding();
                break;
            case IAHoldState.THROWING:
                OnUpdateThrowing();
                break;
        }
    }
    private void OnStateFixedUpdate(IAHoldState state)
    {
        switch (state)
        {
            case IAHoldState.HANDS_FREE:
                OnFixedUpdateHandsFree();
                break;
            case IAHoldState.PICKING:
                OnFixedUpdatePicking();
                break;
            case IAHoldState.HOLDING:
                OnFixedUpdateHolding();
                break;
            case IAHoldState.THROWING:
                OnFixedUpdateThrowing();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(IAHoldState state)
    {
        switch (state)
        {
            case IAHoldState.HANDS_FREE:
                OnExitHandsFree();
                break;
            case IAHoldState.PICKING:
                OnExitPicking();
                break;
            case IAHoldState.HOLDING:
                OnExitHolding();
                break;
            case IAHoldState.THROWING:
                OnExitThrowing();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(IAHoldState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterHandsFree()
    {
        _animator.SetBool("IsHandsFree", true);
        _timersController.StartHoldThinking();
    }
    private void OnUpdateHandsFree()
    {
        if (_timersController.HoldThinkingEnded)
        {
            string pickedName = _pick.PickName();
            if (pickedName=="TRYPICK")
            {
                if (_enemyController.CanThrow && _healthStateMachine.CurrentState == IAHealthState.AWAKE && _combatStateMachine.CurrentState == IACombatState.PEACEFULL)
                {
                    TransitionToState(IAHoldState.PICKING);
                    return;
                }
                else
                {
                    _timersController.StopHoldThinking();
                    _timersController.StartHoldThinking();
                }
            }
            else
            {
                _timersController.StopHoldThinking();
                _timersController.StartHoldThinking();
            }
        }
    }
    private void OnFixedUpdateHandsFree()
    {
    }
    private void OnExitHandsFree()
    {
        _timersController.StopHoldThinking();
        _animator.SetBool("IsHandsFree", false);
    }

    private void OnEnterPicking()
    {
        _animator.SetBool("IsPicking", true);
        _enemyController.StartPicking();
    }
    private void OnUpdatePicking()
    {
        if (_enemyController.IsPickingEnded)
        {
            TransitionToState(IAHoldState.HOLDING);
            return;
        }
        _enemyController.DoPicking();
    }
    private void OnFixedUpdatePicking()
    {
    }
    private void OnExitPicking()
    {
        _animator.SetBool("IsPicking", false);
    }

    private void OnEnterHolding()
    {
        _animator.SetBool("IsHolding", true);
        _timersController.StartThrowThinking();
    }
    private void OnUpdateHolding()
    {
        if (_timersController.ThrowThinkingEnded)
        {
            string pickedName = _throw.PickName();
            if (pickedName=="TRYTHROW")
            {
                if (_enemyController.IsTargetThrowable)
                {
                    if (_healthStateMachine.CurrentState == IAHealthState.AWAKE)
                    {
                        TransitionToState(IAHoldState.THROWING);
                        return;
                    }
                }
                else
                {
                    _timersController.StopThrowThinking();
                    _timersController.StartThrowThinking();
                }
            }
            else
            {
                _timersController.StopThrowThinking();
                _timersController.StartThrowThinking();
            }
        }
    }
    private void OnFixedUpdateHolding()
    {
    }
    private void OnExitHolding()
    {
        _animator.SetBool("IsHolding", false);
        _timersController.StopThrowThinking();
    }

    private void OnEnterThrowing()
    {
        _animator.SetBool("IsThrowing", true);
        _enemyController.StartThrowing();
    }
    private void OnUpdateThrowing()
    {
        if (_enemyController.IsThrowingEnded)
        {
            TransitionToState(IAHoldState.HANDS_FREE);
            return;
        }
        _enemyController.DoThrowing();
    }
    private void OnFixedUpdateThrowing()
    {
    }
    private void OnExitThrowing()
    {
        _animator.SetBool("IsThrowing", false);
    }

}
