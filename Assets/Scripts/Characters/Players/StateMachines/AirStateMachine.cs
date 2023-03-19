using UnityEngine;

public enum AirState
{
    GROUNDED,
    JUMPING,
    DIVING,
    GROUNDPOUND_LANDING,
    LANDING,
}

public class AirStateMachine : MonoBehaviour
{
    [SerializeField] private AirState _currentState;
    private Animator _animator;
    private VFXController _vfxController;
    private SFXController _sfxController;
    private HeroInput _input;
    private PlayerController _controller;
    private HealthStateMachine _healthStateMachine;
    private CombatStateMachine _combatStateMachine;
    private HoldStateMachine _holdStateMachine;

    public AirState CurrentState { get => _currentState; private set => _currentState = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _vfxController = GetComponentInChildren<VFXController>();
        _sfxController = GetComponentInChildren<SFXController>();
        _input = GetComponent<HeroInput>();
        _controller = GetComponent<PlayerController>();
        _healthStateMachine = GetComponent<HealthStateMachine>();
        _combatStateMachine = GetComponent<CombatStateMachine>();
        _holdStateMachine = GetComponent<HoldStateMachine>();
    }
    private void Start()
    {
        CurrentState = AirState.GROUNDED;
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

    private void OnStateEnter(AirState state)
    {
        switch (state)
        {
            case AirState.GROUNDED:
                OnEnterGrounded();
                break;
            case AirState.JUMPING:
                OnEnterJumping();
                break;
            case AirState.DIVING:
                OnEnterDiving();
                break;
            case AirState.GROUNDPOUND_LANDING:
                OnEnterGroundPoundLanding();
                break;
            case AirState.LANDING:
                OnEnterLanding();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(AirState state)
    {
        switch (state)
        {
            case AirState.GROUNDED:
                OnUpdateGrounded();
                break;
            case AirState.JUMPING:
                OnUpdateJumping();
                break;
            case AirState.DIVING:
                OnUpdateDiving();
                break;
            case AirState.GROUNDPOUND_LANDING:
                OnUpdateGroundPoundLanding();
                break;
            case AirState.LANDING:
                OnUpdateLanding();
                break;
        }
    }
    private void OnStateFixedUpdate(AirState state)
    {
        switch (state)
        {
            case AirState.GROUNDED:
                OnFixedUpdateGrounded();
                break;
            case AirState.JUMPING:
                OnFixedUpdateJumping();
                break;
            case AirState.DIVING:
                OnFixedUpdateDiving();
                break;
            case AirState.GROUNDPOUND_LANDING:
                OnFixedUpdateGroundPoundLanding();
                break;
            case AirState.LANDING:
                OnFixedUpdateLanding();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(AirState state)
    {
        switch (state)
        {
            case AirState.GROUNDED:
                OnExitGrounded();
                break;
            case AirState.JUMPING:
                OnExitJumping();
                break;
            case AirState.DIVING:
                OnExitDiving();
                break;
            case AirState.GROUNDPOUND_LANDING:
                OnExitGroundPoundLanding();
                break;
            case AirState.LANDING:
                OnExitLanding();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(AirState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterGrounded()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsGrounded", true);
    }
    private void OnUpdateGrounded()
    {
        // Si la touche de saut vient d'être enfoncée et que le personnage n'est ni mort ni blessé et que le freeze d'attaque est terminé
        if (_input.JumpDown
            && _combatStateMachine.CurrentState != CombatState.SPECIAL
            && _healthStateMachine.CurrentState == PlayerHealthState.AWAKE
            && _controller.IsAttackFreezeEnded)
        {
            TransitionToState(AirState.JUMPING);
            return;
        }
    }
    private void OnFixedUpdateGrounded()
    {
    }
    private void OnExitGrounded()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsGrounded", false);
    }

    private void OnEnterJumping()
    {
        // On démarre le saut
        _controller.StartJump();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsJumping", true);
        // On démarre le VFX
        _vfxController.TriggerJump();
        // On démarre le SFX
        _sfxController.TriggerJump();
    }
    private void OnUpdateJumping()
    {
        // Si le personnage ne se déplace pas, n'est pas blessé, ne porte rien dans les mains et effectue une attaque
        if (!_input.HasMovement && _combatStateMachine.CurrentState == CombatState.PEACEFULL && _healthStateMachine.CurrentState == PlayerHealthState.AWAKE && _holdStateMachine.CurrentState == HoldState.HANDS_FREE && _input.AttackDown)
        {
            TransitionToState(AirState.DIVING);
            return;
        }
        // Si le saut est terminé
        if (_controller.IsJumpEnded)
        {
            TransitionToState(AirState.LANDING);
            return;
        }

        // On continue le saut
        _controller.DoJump();
    }
    private void OnFixedUpdateJumping()
    {
    }
    private void OnExitJumping()
    {
        // On termine le saut
        _controller.EndJump();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsJumping", false);
    }

    private void OnEnterDiving()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsDiving", true);
    }
    private void OnUpdateDiving()
    {
        // Si la plongée est terminée
        if (_controller.IsDiveEnded)
        {
            TransitionToState(AirState.GROUNDPOUND_LANDING);
            return;
        }

        // On continue la plongée
        _controller.DoDiving();
    }
    private void OnFixedUpdateDiving()
    {
    }
    private void OnExitDiving()
    {
        // On termine la plongée
        _controller.EndDiving();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsDiving", false);
    }

    private void OnEnterGroundPoundLanding()
    {

    }
    private void OnUpdateGroundPoundLanding()
    {
        // Si le groundpound est terminé
        if (_controller.IsGroundPoundEnded)
        {
            TransitionToState(AirState.GROUNDED);
            return;
        }
    }
    private void OnFixedUpdateGroundPoundLanding()
    {
    }
    private void OnExitGroundPoundLanding()
    {

    }

    private void OnEnterLanding()
    {
        // On démarre l'atterissage
        _controller.StartLand();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsLanding", true);
        // On démarre le VFX
        _vfxController.TriggerLand();
    }
    private void OnUpdateLanding()
    {
        // Si la touche de saut vient d'être enfoncée et que le personnage n'est ni mort ni blessé et que le freeze d'attaque est terminé
        if (_input.JumpDown
            && _healthStateMachine.CurrentState == PlayerHealthState.AWAKE
            && _controller.IsAttackFreezeEnded)
        {
            TransitionToState(AirState.JUMPING);
            return;
        }
        // Si l'atterissage est terminé
        if (_controller.IsLandingEnded)
        {
            TransitionToState(AirState.GROUNDED);
            return;
        }

        // On continue l'atterissage
        _controller.DoLand();
    }
    private void OnFixedUpdateLanding()
    {
    }
    private void OnExitLanding()
    {
        // On termine l'atterissage
        _controller.EndLand();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsLanding", false);
    }

}
