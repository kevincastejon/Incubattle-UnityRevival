using UnityEngine;

public enum MovementState
{
    IDLE,
    WALKING,
    SPRINTING,
}

public class MovementStateMachine : MonoBehaviour
{
    [SerializeField] private MovementState _currentState;
    private Animator _animator;
    private VFXController _vfxController;
    private HeroInput _input;
    private PlayerController _controller;
    private HealthStateMachine _healthStateMachine;
    private CombatStateMachine _combatStateMachine;

    public MovementState CurrentState { get => _currentState; private set => _currentState = value; }
    public bool CanMove
    {
        get
        {
            return _combatStateMachine.CurrentState != CombatState.GROUNDPOUND_DIVING
           && _combatStateMachine.CurrentState != CombatState.GROUNDPOUNDING
           && _combatStateMachine.CurrentState != CombatState.SPECIAL
           && _healthStateMachine.CurrentState == PlayerHealthState.AWAKE
           && _controller.IsAttackFreezeEnded;
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _vfxController = GetComponentInChildren<VFXController>();
        _input = GetComponent<HeroInput>();
        _controller = GetComponent<PlayerController>();
        _healthStateMachine = GetComponent<HealthStateMachine>();
        _combatStateMachine = GetComponent<CombatStateMachine>();
    }
    private void Start()
    {
        CurrentState = MovementState.IDLE;
        OnEnterIdle();
    }
    public void Update()
    {
        OnStateUpdate(CurrentState);
    }
    public void FixedUpdate()
    {
        OnStateFixedUpdate(CurrentState);
    }

    private void OnStateEnter(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnEnterIdle();
                break;
            case MovementState.WALKING:
                OnEnterWalking();
                break;
            case MovementState.SPRINTING:
                OnEnterSprinting();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnUpdateIdle();
                break;
            case MovementState.WALKING:
                OnUpdateWalking();
                break;
            case MovementState.SPRINTING:
                OnUpdateSprinting();
                break;
        }
    }
    private void OnStateFixedUpdate(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnFixedUpdateIdle();
                break;
            case MovementState.WALKING:
                OnFixedUpdateWalking();
                break;
            case MovementState.SPRINTING:
                OnFixedUpdateSprinting();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnExitIdle();
                break;
            case MovementState.WALKING:
                OnExitWalking();
                break;
            case MovementState.SPRINTING:
                OnExitSprinting();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(MovementState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterIdle()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsIdle", true);
    }
    private void OnUpdateIdle()
    {
        // Si il y a du mouvement, que le personnage peut se déplacer
        if (_input.HasMovement && CanMove)
        {
            // Si la touche de sprint est enfoncée
            if (_input.SprintDown) 
            {
                TransitionToState(MovementState.SPRINTING);
            }
            // Sinon
            else
            {
                TransitionToState(MovementState.WALKING);
            }
        }
    }
    private void OnFixedUpdateIdle()
    {
        // On fige le personnage
        _controller.DoIdle();
    }
    private void OnExitIdle()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsIdle", false);
    }

    private void OnEnterWalking()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsWalking", true);
    }
    private void OnUpdateWalking()
    {
        // Si il n'y a pas de mouvement ou que le personnage ne peut pas se déplacer
        if (!_input.HasMovement || !CanMove)
        {
            TransitionToState(MovementState.IDLE);
            return;
        }
        // Si il y a mouvement et que la touche Sprint est enfoncée
        if (_input.HasMovement && _input.SprintDown)
        {
            TransitionToState(MovementState.SPRINTING);
            return;
        }

        // On fait se retourner le personnage
        _controller.DoTurnCharacter();
    }
    private void OnFixedUpdateWalking()
    {
        // On fait marcher le personnage
        _controller.DoWalk();
    }
    private void OnExitWalking()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsWalking", false);
    }

    private void OnEnterSprinting()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsSprinting", true);
        if (_input.Movement.x > 0f)
        {
            // On démarre le VFX
            _vfxController.TriggerRunLeft();
        }
        else
        {
            // On démarre le VFX
            _vfxController.TriggerRunRight();
        }
    }
    private void OnUpdateSprinting()
    {
        // Si il n'y a pas de mouvement ou que le personnage ne peut pas se déplacer
        if (!_input.HasMovement || !CanMove)
        {
            TransitionToState(MovementState.IDLE);
            return;
        }
        // Si il y a mouvement et que la touche Sprint n'est pas enfoncée
        if (_input.HasMovement && !_input.Sprint)
        {
            TransitionToState(MovementState.WALKING);
            return;
        }

        // On fait se retourner le personnage
        _controller.DoTurnCharacter();
    }
    private void OnFixedUpdateSprinting()
    {
        // On fait courir le personnage
        _controller.DoSprint();
    }
    private void OnExitSprinting()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsSprinting", false);
    }

}
