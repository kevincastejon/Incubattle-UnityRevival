using KevinCastejon.MoreAttributes;
using UnityEngine;

public enum IAAirState
{
    GROUNDED,
    JUMPING,
    LANDING,
}

public class IAAirStateMachine : MonoBehaviour
{
    [SerializeField] [ReadOnly] private IAAirState _currentState;
    [SerializeField] private WeightedRandomPicker _jump;
    private Animator _animator;
    private EnemyController _enemyController;
    private TimersController _timersController;
    private IACombatStateMachine _combatStateMachine;
    private IAHealthStateMachine _healthStateMachine;
    public IAAirState CurrentState { get => _currentState; private set => _currentState = value; }
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
        CurrentState = IAAirState.GROUNDED;
        OnEnterGrounded();
    }
    private void Update()
    {
        OnStateUpdate(CurrentState);
    }
    private void FixedUpdate()
    {
        OnStateFixedUpdate(CurrentState);
    }

    private void OnStateEnter(IAAirState state)
    {
        switch (state)
        {
            case IAAirState.GROUNDED:
                OnEnterGrounded();
                break;
            case IAAirState.JUMPING:
                OnEnterJumping();
                break;
            case IAAirState.LANDING:
                OnEnterLanding();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(IAAirState state)
    {
        switch (state)
        {
            case IAAirState.GROUNDED:
                OnUpdateGrounded();
                break;
            case IAAirState.JUMPING:
                OnUpdateJumping();
                break;
            case IAAirState.LANDING:
                OnUpdateLanding();
                break;
        }
    }
    private void OnStateFixedUpdate(IAAirState state)
    {
        switch (state)
        {
            case IAAirState.GROUNDED:
                OnFixedUpdateGrounded();
                break;
            case IAAirState.JUMPING:
                OnFixedUpdateJumping();
                break;
            case IAAirState.LANDING:
                OnFixedUpdateLanding();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(IAAirState state)
    {
        switch (state)
        {
            case IAAirState.GROUNDED:
                OnExitGrounded();
                break;
            case IAAirState.JUMPING:
                OnExitJumping();
                break;
            case IAAirState.LANDING:
                OnExitLanding();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    public void TransitionToState(IAAirState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterGrounded()
    {
        _animator.SetBool("IsGrounded", true);
        _timersController.StartAirThinking();
    }
    private void OnUpdateGrounded()
    {
        if (_timersController.AirThinkingEnded)
        {
            string pickedName = _jump.PickName();
            if (pickedName == "TRYJUMP")
            {
                if (_enemyController.CanJump && _healthStateMachine.CurrentState == IAHealthState.AWAKE && _combatStateMachine.CurrentState == IACombatState.PEACEFULL && _enemyController.IsTargetReachable)
                {
                    TransitionToState(IAAirState.JUMPING);
                    return;
                }
                else
                {
                    _timersController.StopAirThinking();
                    _timersController.StartAirThinking();
                    return;
                }
            }
            else
            {
                _timersController.StopAirThinking();
                _timersController.StartAirThinking();
                return;
            }
        }
    }
    private void OnFixedUpdateGrounded()
    {
    }
    private void OnExitGrounded()
    {
        _animator.SetBool("IsGrounded", false);
        _timersController.StopAirThinking();
    }

    private void OnEnterJumping()
    {
        _animator.SetBool("IsJumping", true);
        _enemyController.StartJump();
    }
    private void OnUpdateJumping()
    {
        if (_enemyController.IsJumpEnded)
        {
            TransitionToState(IAAirState.LANDING);
            return;
        }
        _enemyController.DoJump();
    }
    private void OnFixedUpdateJumping()
    {
    }
    private void OnExitJumping()
    {
        _animator.SetBool("IsJumping", false);
        _enemyController.EndJump();
    }

    private void OnEnterLanding()
    {
        _animator.SetBool("IsLanding", true);
        _enemyController.StartLand();
    }
    private void OnUpdateLanding()
    {
        if (_enemyController.IsLandingEnded)
        {
            TransitionToState(IAAirState.GROUNDED);
            return;
        }
        _enemyController.DoLand();
    }
    private void OnFixedUpdateLanding()
    {
    }
    private void OnExitLanding()
    {
        _animator.SetBool("IsLanding", false);
        _enemyController.EndLand();
    }

}
