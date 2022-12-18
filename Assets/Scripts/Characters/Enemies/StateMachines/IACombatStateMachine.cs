using KevinCastejon.MoreAttributes;
using UnityEngine;

public enum IACombatState
{
    PEACEFULL,
    ATTACKING,
    SLAM,
    TAUNTING,
    DASHING
}

public class IACombatStateMachine : MonoBehaviour
{
    [SerializeField] [ReadOnly] private IACombatState _currentState;
    [SerializeField] private WeightedRandomPicker _attack;
    private Animator _animator;
    private ScreenShaker _screenShaker;
    private EnemyController _enemyController;
    private TimersController _timersController;
    private IAHoldStateMachine _holdStateMachine;
    private IAHealthStateMachine _healthStateMachine;
    private IAAirStateMachine _airStateMachine;

    public IACombatState CurrentState { get => _currentState; private set => _currentState = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _screenShaker = FindObjectOfType<ScreenShaker>();
        _enemyController = GetComponent<EnemyController>();
        _timersController = GetComponentInChildren<TimersController>();
        _holdStateMachine = GetComponent<IAHoldStateMachine>();
        _healthStateMachine = GetComponent<IAHealthStateMachine>();
        _airStateMachine = GetComponent<IAAirStateMachine>();
    }
    private void Start()
    {
        CurrentState = IACombatState.PEACEFULL;
        OnEnterPeacefull();
    }
    private void Update()
    {
        OnStateUpdate(CurrentState);
    }
    private void FixedUpdate()
    {
        OnStateFixedUpdate(CurrentState);
    }

    private void OnStateEnter(IACombatState state)
    {
        switch (state)
        {
            case IACombatState.PEACEFULL:
                OnEnterPeacefull();
                break;
            case IACombatState.ATTACKING:
                OnEnterAttacking();
                break;
            case IACombatState.SLAM:
                OnEnterSlam();
                break;
            case IACombatState.TAUNTING:
                OnEnterTaunting();
                break;
            case IACombatState.DASHING:
                OnEnterDashing();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(IACombatState state)
    {
        switch (state)
        {
            case IACombatState.PEACEFULL:
                OnUpdatePeacefull();
                break;
            case IACombatState.ATTACKING:
                OnUpdateAttacking();
                break;
            case IACombatState.SLAM:
                OnUpdateSlam();
                break;
            case IACombatState.TAUNTING:
                OnUpdateTaunting();
                break;
            case IACombatState.DASHING:
                OnUpdateDashing();
                break;
            default:
                Debug.LogError("OnStateUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateFixedUpdate(IACombatState state)
    {
        switch (state)
        {
            case IACombatState.PEACEFULL:
                OnFixedUpdatePeacefull();
                break;
            case IACombatState.ATTACKING:
                OnFixedUpdateAttacking();
                break;
            case IACombatState.SLAM:
                OnFixedUpdateSlam();
                break;
            case IACombatState.TAUNTING:
                OnFixedUpdateTaunting();
                break;
            case IACombatState.DASHING:
                OnFixedUpdateDashing();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(IACombatState state)
    {
        switch (state)
        {
            case IACombatState.PEACEFULL:
                OnExitPeacefull();
                break;
            case IACombatState.ATTACKING:
                OnExitAttacking();
                break;
            case IACombatState.SLAM:
                OnExitSlam();
                break;
            case IACombatState.TAUNTING:
                OnExitTaunting();
                break;
            case IACombatState.DASHING:
                OnExitDashing();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(IACombatState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterPeacefull()
    {
        _animator.SetBool("IsPeacefull", true);
        _timersController.StartCombatThinking();
        _enemyController.ResetComboCount();
    }
    private void OnUpdatePeacefull()
    {
        if (_timersController.CombatThinkingEnded)
        {
            string pickedName = _attack.PickName();
            if (pickedName == "TRYSLAM")
            {
                if (_enemyController.CanSlam && _enemyController.IsTargetSlammable && _holdStateMachine.CurrentState == IAHoldState.HANDS_FREE && _healthStateMachine.CurrentState == IAHealthState.AWAKE && _airStateMachine.CurrentState == IAAirState.GROUNDED)
                {
                    TransitionToState(IACombatState.SLAM);
                    return;
                }
                else if (_enemyController.IsTargetReachable && _holdStateMachine.CurrentState == IAHoldState.HANDS_FREE && _healthStateMachine.CurrentState == IAHealthState.AWAKE)
                {
                    TransitionToState(IACombatState.ATTACKING);
                    return;
                }
                else
                {
                    _timersController.StopCombatThinking();
                    _timersController.StartCombatThinking();
                    return;
                }
            }
            else if (pickedName == "TRYDASH")
            {
                if (_enemyController.CanDash && _holdStateMachine.CurrentState == IAHoldState.HANDS_FREE && _healthStateMachine.CurrentState == IAHealthState.AWAKE && _airStateMachine.CurrentState == IAAirState.GROUNDED)
                {
                    TransitionToState(IACombatState.TAUNTING);
                    return;
                }
                else if (_enemyController.IsTargetReachable && _holdStateMachine.CurrentState == IAHoldState.HANDS_FREE && _healthStateMachine.CurrentState == IAHealthState.AWAKE)
                {
                    TransitionToState(IACombatState.ATTACKING);
                    return;
                }
                else
                {
                    _timersController.StopCombatThinking();
                    _timersController.StartCombatThinking();
                    return;
                }
            }
            else if (pickedName == "TRYATTACK")
            {
                if (_enemyController.IsTargetReachable && _holdStateMachine.CurrentState == IAHoldState.HANDS_FREE && _healthStateMachine.CurrentState == IAHealthState.AWAKE)
                {
                    TransitionToState(IACombatState.ATTACKING);
                    return;
                }
                else
                {
                    _timersController.StopCombatThinking();
                    _timersController.StartCombatThinking();
                    return;
                }
            }
            else
            {
                _timersController.StopCombatThinking();
                _timersController.StartCombatThinking();
                return;
            }
        }
    }
    private void OnFixedUpdatePeacefull()
    {
    }
    private void OnExitPeacefull()
    {
        _animator.SetBool("IsPeacefull", false);
    }

    private void OnEnterAttacking()
    {
        _animator.SetBool("IsAttacking", true);
        _enemyController.StartAttack();
        _enemyController.PickComboCount();
    }
    private void OnUpdateAttacking()
    {
        if (_enemyController.IsAttackEnded)
        {
            if (_enemyController.Combo == _enemyController.CurrentAttackMaxCombo - 1)
            {
                TransitionToState(IACombatState.PEACEFULL);
                return;
            }
            else
            {
                TransitionToState(IACombatState.ATTACKING);
                return;
            }
        }

        _enemyController.DoAttack();
    }
    private void OnFixedUpdateAttacking()
    {
    }
    private void OnExitAttacking()
    {
        _animator.SetBool("IsAttacking", false);
        _animator.Update(Time.deltaTime);
        _enemyController.EndAttack();
    }


    private void OnEnterSlam()
    {
        _animator.SetBool("IsSlaming", true);
        _enemyController.StartSlaming();
    }
    private void OnUpdateSlam()
    {
        if (_enemyController.IsSlamEnded)
        {
            TransitionToState(IACombatState.PEACEFULL);
            return;
        }

        _enemyController.DoSlaming();
    }
    private void OnFixedUpdateSlam()
    {
    }
    private void OnExitSlam()
    {
        _animator.SetBool("IsSlaming", false);
        _enemyController.EndSlaming();
    }


    private void OnEnterTaunting()
    {
        _animator.SetBool("IsTaunting", true);
        _enemyController.StartTaunting();
    }
    private void OnUpdateTaunting()
    {
        if (_enemyController.IsTauntEnded)
        {
            TransitionToState(IACombatState.DASHING);
            return;
        }

        _enemyController.DoTaunting();
    }
    private void OnFixedUpdateTaunting()
    {
    }
    private void OnExitTaunting()
    {
        _animator.SetBool("IsTaunting", false);
        _enemyController.EndTaunting();
    }

    private void OnEnterDashing()
    {
        _animator.SetBool("IsDashing", true);
        _enemyController.StartDashing();
    }
    private void OnUpdateDashing()
    {
        if (_enemyController.IsDashCoolDownEnded)
        {
            if (_enemyController.IsTargetReachable)
            {
                TransitionToState(IACombatState.ATTACKING);
                return;
            }
            else if (_enemyController.IsDashEnded)
            {
                TransitionToState(IACombatState.PEACEFULL);
                _airStateMachine.TransitionToState(IAAirState.JUMPING);
                return;
            }
        }

        _enemyController.DoTurnCharacter();
    }
    private void OnFixedUpdateDashing()
    {
        _enemyController.DoDashing();
    }
    private void OnExitDashing()
    {
        _animator.SetBool("IsDashing", false);
        _enemyController.EndDashing();
        _screenShaker.Shake();
    }

}
