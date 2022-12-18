using UnityEngine;
using UnityEngine.Events;

public enum PlayerHealthState
{
    AWAKE,
    HURT,
    RESURRECTING,
    DEAD,
}

public class HealthStateMachine : MonoBehaviour
{
    [SerializeField] private PlayerHealthState _currentState;
    private Animator _animator;
    private SFXController _sfxController;
    private PlayerController _controller;
    private HitBox _hitbox;
    private GameHUD _hud;
    private CombatStateMachine _combatStateMachine;

    public PlayerHealthState CurrentState { get => _currentState; private set => _currentState = value; }
    public bool CanBeHurt
    {
        get
        {
            return _combatStateMachine.CurrentState != CombatState.GROUNDPOUND_DIVING
           && _combatStateMachine.CurrentState != CombatState.SPECIAL
           && _combatStateMachine.CurrentState != CombatState.GROUNDPOUNDING;
        }
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sfxController = GetComponentInChildren<SFXController>();
        _controller = GetComponent<PlayerController>();
        _hitbox = GetComponentInChildren<HitBox>();
        _hud = FindObjectOfType<GameHUD>();
        _combatStateMachine = GetComponent<CombatStateMachine>();
    }
    private void Start()
    {
        CurrentState = PlayerHealthState.AWAKE;
        OnEnterAwake();
    }
    public void Update()
    {
        OnStateUpdate(CurrentState);
    }
    public void FixedUpdate()
    {
        OnStateFixedUpdate(CurrentState);
    }

    private void OnStateEnter(PlayerHealthState state)
    {
        switch (state)
        {
            case PlayerHealthState.AWAKE:
                OnEnterAwake();
                break;
            case PlayerHealthState.HURT:
                OnEnterHurt();
                break;
            case PlayerHealthState.RESURRECTING:
                OnEnterResurrecting();
                break;
            case PlayerHealthState.DEAD:
                OnEnterDead();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(PlayerHealthState state)
    {
        switch (state)
        {
            case PlayerHealthState.AWAKE:
                OnUpdateAwake();
                break;
            case PlayerHealthState.HURT:
                OnUpdateHurt();
                break;
            case PlayerHealthState.RESURRECTING:
                OnUpdateResurrecting();
                break;
            case PlayerHealthState.DEAD:
                OnUpdateDead();
                break;
        }
    }
    private void OnStateFixedUpdate(PlayerHealthState state)
    {
        switch (state)
        {
            case PlayerHealthState.AWAKE:
                OnFixedUpdateAwake();
                break;
            case PlayerHealthState.HURT:
                OnFixedUpdateHurt();
                break;
            case PlayerHealthState.RESURRECTING:
                OnFixedUpdateResurrecting();
                break;
            case PlayerHealthState.DEAD:
                OnFixedUpdateDead();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(PlayerHealthState state)
    {
        switch (state)
        {
            case PlayerHealthState.AWAKE:
                OnExitAwake();
                break;
            case PlayerHealthState.HURT:
                OnExitHurt();
                break;
            case PlayerHealthState.RESURRECTING:
                OnExitResurrecting();
                break;
            case PlayerHealthState.DEAD:
                OnExitDead();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(PlayerHealthState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterAwake()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsAwake", true);
    }
    private void OnUpdateAwake()
    {
        // Si la Hitbox est touchée et que le personnage peut subir des dégats
        if (CanBeHurt && _hitbox.DetectHit())
        {
            AttackData atk = _hitbox.GetHit();
            if (_controller.HealthPoints - atk.Damages > 0)
            {
                TransitionToState(PlayerHealthState.HURT);
                return;
            }
            else
            {
                // S'il ne reste plus de vie
                if (_controller.Lifes - 1 == 0)
                {
                    TransitionToState(PlayerHealthState.DEAD);
                    return;
                }
                // Sinon
                else
                {
                    TransitionToState(PlayerHealthState.RESURRECTING);
                    return;
                }
            }
        }
    }
    private void OnFixedUpdateAwake()
    {
    }
    private void OnExitAwake()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsAwake", false);
    }

    private void OnEnterHurt()
    {
        //
        AttackData attackData = _hitbox.GetHit();
        // On démarre le recul
        _controller.StartKnockBack(attackData);
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsHurt", true);
        if (attackData.HitType == HitType.CAN)
        {
            // On démarre le SFX
            _sfxController.TriggerThrowHit();
        }
        else
        {
            // On démarre le SFX
            _sfxController.TriggerHurt();
        }
    }
    private void OnUpdateHurt()
    {
        // Si le recul est terminé
        if (_controller.IsKnockBackEnded)
        {
            TransitionToState(PlayerHealthState.AWAKE);
            return;
        }

        // On continue le recul vertical
        _controller.DoVerticalKnockback();
    }
    private void OnFixedUpdateHurt()
    {
        // On continue le recul horizontal
        _controller.DoHorizontalKnockBack();
    }
    private void OnExitHurt()
    {
        // On termine le recul
        _controller.EndKnockBack();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsHurt", false);
    }

    private void OnEnterResurrecting()
    {
        // On démarre la resurrection
        _controller.StartResurrection();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsDead", true);
        //
        _sfxController.TriggerDead();
    }
    private void OnUpdateResurrecting()
    {
        // Si la résurrection est terminée
        if (_controller.IsResurrectionEnded)
        {
            TransitionToState(PlayerHealthState.AWAKE);
        }
    }
    private void OnFixedUpdateResurrecting()
    {
    }
    private void OnExitResurrecting()
    {
        // On termine la résurrection
        _controller.EndResurrection();
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsDead", false);
    }

    private void OnEnterDead()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsDead", true);
        // On affiche le menu de Game Over
        _hud.ShowGameOverMenu();
        // On démarre le SFX
        _sfxController.TriggerDead();
    }
    private void OnUpdateDead()
    {
    }
    private void OnFixedUpdateDead()
    {
    }
    private void OnExitDead()
    {
        // On envoie les paramètres necessaires à l'Animator
        _animator.SetBool("IsDead", false);
    }

}
