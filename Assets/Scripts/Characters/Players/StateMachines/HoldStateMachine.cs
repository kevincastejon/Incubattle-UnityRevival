using UnityEngine;

public enum HoldState
{
    HANDS_FREE,
    PICKING,
    HOLDING,
    THROWING,
}

public class HoldStateMachine : MonoBehaviour
{
    [SerializeField] private HoldState _currentState;
    private Animator _animator;
    private VFXController _vfxController;
    private SFXController _sfxController;
    private HeroInput _input;
    private PlayerController _controller;
    private PickBox _pickBox;
    private MovementStateMachine _movementStateMachine;
    private HealthStateMachine _healthStateMachine;
    private AirStateMachine _airStateMachine;
    public HoldState CurrentState { get => _currentState; private set => _currentState = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _vfxController = GetComponentInChildren<VFXController>();
        _sfxController = GetComponentInChildren<SFXController>();
        _input = GetComponent<HeroInput>();
        _controller = GetComponent<PlayerController>();
        _pickBox = GetComponentInChildren<PickBox>();
        _healthStateMachine = GetComponent<HealthStateMachine>();
        _airStateMachine = GetComponent<AirStateMachine>();
        _movementStateMachine = GetComponent<MovementStateMachine>();
    }
    private void Start()
    {
        CurrentState = HoldState.HANDS_FREE;
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

    private void OnStateEnter(HoldState state)
    {
        switch (state)
        {
            case HoldState.HANDS_FREE:
                OnEnterHandsFree();
                break;
            case HoldState.PICKING:
                OnEnterPicking();
                break;
            case HoldState.HOLDING:
                OnEnterHolding();
                break;
            case HoldState.THROWING:
                OnEnterThrowing();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(HoldState state)
    {
        switch (state)
        {
            case HoldState.HANDS_FREE:
                OnUpdateHandsFree();
                break;
            case HoldState.PICKING:
                OnUpdatePicking();
                break;
            case HoldState.HOLDING:
                OnUpdateHolding();
                break;
            case HoldState.THROWING:
                OnUpdateThrowing();
                break;
        }
    }
    private void OnStateFixedUpdate(HoldState state)
    {
        switch (state)
        {
            case HoldState.HANDS_FREE:
                OnFixedUpdateHandsFree();
                break;
            case HoldState.PICKING:
                OnFixedUpdatePicking();
                break;
            case HoldState.HOLDING:
                OnFixedUpdateHolding();
                break;
            case HoldState.THROWING:
                OnFixedUpdateThrowing();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(HoldState state)
    {
        switch (state)
        {
            case HoldState.HANDS_FREE:
                OnExitHandsFree();
                break;
            case HoldState.PICKING:
                OnExitPicking();
                break;
            case HoldState.HOLDING:
                OnExitHolding();
                break;
            case HoldState.THROWING:
                OnExitThrowing();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(HoldState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterHandsFree()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsHandsFree", true);
    }
    private void OnUpdateHandsFree()
    {
        // Si le personnage ne subit pas de recul, que l'input Attack vient d'être enfoncée et que la PickBox detecte un Throwable
        if (_movementStateMachine.CurrentState != MovementState.SPRINTING && _airStateMachine.CurrentState == AirState.GROUNDED && _healthStateMachine.CurrentState == PlayerHealthState.AWAKE && _input.AttackDown && _pickBox.GetThrowable() != null)
        {
            TransitionToState(HoldState.PICKING);
            return;
        }
    }
    private void OnFixedUpdateHandsFree()
    {
    }
    private void OnExitHandsFree()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsHandsFree", false);
    }

    private void OnEnterPicking()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsPicking", true);
        // On démarre le ramassage
        _controller.StartPicking(_pickBox.GetThrowable());
    }
    private void OnUpdatePicking()
    {
        if (_controller.IsPickingEnded)
        {
            TransitionToState(HoldState.HOLDING);
            return;
        }

        // On continue le ramassage
        _controller.DoPicking();
    }
    private void OnFixedUpdatePicking()
    {
    }
    private void OnExitPicking()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsPicking", false);
    }

    private void OnEnterHolding()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsHolding", true);
    }
    private void OnUpdateHolding()
    {
        // Si le personnage ne subit pas de recul et que l'input Attack vient d'être enfoncée
        if (_healthStateMachine.CurrentState == PlayerHealthState.AWAKE && _input.AttackDown)
        {
            TransitionToState(HoldState.THROWING);
            return;
        }
    }
    private void OnFixedUpdateHolding()
    {
    }
    private void OnExitHolding()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsHolding", false);
    }

    private void OnEnterThrowing()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsThrowing", true);
        // On démarre le lancer
        _controller.StartThrowing();
        // On démarre le VFX
        _vfxController.TriggerThrow();
        // On démarre le SFX
        _sfxController.TriggerThrow();
    }
    private void OnUpdateThrowing()
    {
        if (_controller.IsThrowingEnded)
        {
            TransitionToState(HoldState.HANDS_FREE);
            return;
        }

        // On continue le lancer
        _controller.DoThrowing();
    }
    private void OnFixedUpdateThrowing()
    {
    }
    private void OnExitThrowing()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsThrowing", false);
    }
}
