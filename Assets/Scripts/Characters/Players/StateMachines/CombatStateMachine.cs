using UnityEngine;

public enum CombatState
{
    PEACEFULL,
    ATTACKING,
    GROUNDPOUND_DIVING,
    GROUNDPOUNDING,
    SPECIAL
}

public class CombatStateMachine : MonoBehaviour
{
    [SerializeField] private CombatState _currentState;
    private Animator _animator;
    private VFXController _vfxController;
    private SFXController _sfxController;
    private PlayerInput _input;
    private PlayerController _controller;
    private HealthStateMachine _healthStateMachine;
    private AirStateMachine _airStateMachine;
    private HoldStateMachine _holdStateMachine;

    public CombatState CurrentState { get => _currentState; private set => _currentState = value; }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _vfxController = GetComponentInChildren<VFXController>();
        _sfxController = GetComponentInChildren<SFXController>();
        _input = GetComponent<PlayerInput>();
        _controller = GetComponent<PlayerController>();
        _healthStateMachine = GetComponent<HealthStateMachine>();
        _airStateMachine = GetComponent<AirStateMachine>();
        _holdStateMachine = GetComponent<HoldStateMachine>();
    }
    private void Start()
    {
        CurrentState = CombatState.PEACEFULL;
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

    private void OnStateEnter(CombatState state)
    {
        switch (state)
        {
            case CombatState.PEACEFULL:
                OnEnterPeacefull();
                break;
            case CombatState.ATTACKING:
                OnEnterAttacking();
                break;
            case CombatState.GROUNDPOUND_DIVING:
                OnEnterGroundPoundDiving();
                break;
            case CombatState.GROUNDPOUNDING:
                OnEnterGroundPounding();
                break;
            case CombatState.SPECIAL:
                OnEnterSpecial();
                break;
        }
    }
    private void OnStateUpdate(CombatState state)
    {
        switch (state)
        {
            case CombatState.PEACEFULL:
                OnUpdatePeacefull();
                break;
            case CombatState.ATTACKING:
                OnUpdateAttacking();
                break;
            case CombatState.GROUNDPOUND_DIVING:
                OnUpdateGroundPoundDiving();
                break;
            case CombatState.GROUNDPOUNDING:
                OnUpdateGroundPounding();
                break;
            case CombatState.SPECIAL:
                OnUpdateSpecial();
                break;
        }
    }
    private void OnStateFixedUpdate(CombatState state)
    {
        switch (state)
        {
            case CombatState.PEACEFULL:
                OnFixedUpdatePeacefull();
                break;
            case CombatState.ATTACKING:
                OnFixedUpdateAttacking();
                break;
            case CombatState.GROUNDPOUND_DIVING:
                OnFixedUpdateGroundPoundDiving();
                break;
            case CombatState.GROUNDPOUNDING:
                OnFixedUpdateGroundPounding();
                break;
            case CombatState.SPECIAL:
                OnFixedUpdateSpecial();
                break;
        }
    }
    private void OnStateExit(CombatState state)
    {
        switch (state)
        {
            case CombatState.PEACEFULL:
                OnExitPeacefull();
                break;
            case CombatState.ATTACKING:
                OnExitAttacking();
                break;
            case CombatState.GROUNDPOUND_DIVING:
                OnExitGroundPoundDiving();
                break;
            case CombatState.GROUNDPOUNDING:
                OnExitGroundPounding();
                break;
            case CombatState.SPECIAL:
                OnExitSpecial();
                break;
        }
    }
    private void TransitionToState(CombatState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterPeacefull()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsPeacefull", true);
    }
    private void OnUpdatePeacefull()
    {
        // Si la touche Special vient d'être enfoncée et que la stamina est au maximum
        if (_input.SpecialDown && _controller.Stamina == _controller.MaxStamina && _holdStateMachine.CurrentState == HoldState.HANDS_FREE)
        {
            TransitionToState(CombatState.SPECIAL);
            return;
        }
        // Si le personnage a démarré une plongée
        if (_airStateMachine.CurrentState == AirState.DIVING)
        {
            TransitionToState(CombatState.GROUNDPOUND_DIVING);
            return;
        }
        // Si la touche Attack vient d'être enfoncée et que le personnage n'est ni mort ni blessé
        if (_input.AttackDown && _healthStateMachine.CurrentState == PlayerHealthState.AWAKE && _holdStateMachine.CurrentState == HoldState.HANDS_FREE)
        {
            TransitionToState(CombatState.ATTACKING);
            return;
        }
    }
    private void OnFixedUpdatePeacefull()
    {
    }
    private void OnExitPeacefull()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsPeacefull", false);
    }

    private void OnEnterAttacking()
    {
        // On démarre l'attaque
        _controller.StartAttack();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsAttacking", true);
        // On démarre le SFX
        _sfxController.TriggerHit();
    }
    private void OnUpdateAttacking()
    {
        // Si l'attaque est terminée ou que le personnage est blessé ou mort ou qu'il est en train d'atterir
        if (_controller.IsAttackEnded 
            || _healthStateMachine.CurrentState != PlayerHealthState.AWAKE
            || _airStateMachine.CurrentState == AirState.LANDING)
        {
            TransitionToState(CombatState.PEACEFULL);
            return;
        }

        // On continue l'attaque
        _controller.DoAttack();
    }
    private void OnFixedUpdateAttacking()
    {
    }
    private void OnExitAttacking()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsAttacking", false);
        _controller.EndAttack();
    }

    private void OnEnterGroundPoundDiving()
    {
    }
    private void OnUpdateGroundPoundDiving()
    {
        // Si la plongée est terminée
        if (_controller.IsDiveEnded)
        {
            TransitionToState(CombatState.GROUNDPOUNDING);
            return;
        }
    }
    private void OnFixedUpdateGroundPoundDiving()
    {
    }
    private void OnExitGroundPoundDiving()
    {
    }

    private void OnEnterGroundPounding()
    {
        // On démarre le groundpound
        _controller.StartGroundPounding();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsGroundPounding", true);
        // On démarre le VFX
        _vfxController.TriggerSlam();
        // On démarre le SFX
        _sfxController.TriggerSlam();
    }
    private void OnUpdateGroundPounding()
    {
        // Si le groundpound est terminé
        if (_controller.IsGroundPoundEnded)
        {
            TransitionToState(CombatState.PEACEFULL);
            return;
        }

        // On continue le groundpound
        _controller.DoGroundPounding();
    }
    private void OnFixedUpdateGroundPounding()
    {
    }
    private void OnExitGroundPounding()
    {
        // On termine le groundpound
        _controller.EndGroundPounding();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsGroundPounding", false);
    }

    private void OnEnterSpecial()
    {
        // On démarre l'attaque speciale
        _controller.StartSpecial();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsSpecial", true);
        // On démarre le SFX
        _sfxController.TriggerSpecial();
    }
    private void OnUpdateSpecial()
    {
        // Si l'attaque speciale est terminée
        if (_controller.IsSpecialEnded)
        {
            TransitionToState(CombatState.PEACEFULL);
            return;
        }

        // On continue l'attaque speciale
        _controller.DoSpecial();
    }
    private void OnFixedUpdateSpecial()
    {
    }
    private void OnExitSpecial()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsSpecial", false);
        // On termine l'attaque speciale
        _controller.EndSpecial();
    }

}
