using KevinCastejon.MoreAttributes;
using UnityEngine;

public enum IAHealthState
{
    AWAKE,
    HURT,
    DEAD,
}

public class IAHealthStateMachine : MonoBehaviour
{
    [SerializeField] [ReadOnly] private IAHealthState _currentState;
    private Animator _animator;
    private EnemyController _enemyController;
    private HitBox _hitbox;
    private TimersController _timersController;
    private IACombatStateMachine _combatStateMachine;
    public IAHealthState CurrentState { get => _currentState; private set => _currentState = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();
        _hitbox = GetComponentInChildren<HitBox>();
        _timersController = GetComponentInChildren<TimersController>();
        _combatStateMachine = GetComponent<IACombatStateMachine>();
    }
    private void Start()
    {
        CurrentState = IAHealthState.AWAKE;
        OnEnterAwake();
    }
    private void Update()
    {
        OnStateUpdate(CurrentState);
    }
    private void FixedUpdate()
    {
        OnStateFixedUpdate(CurrentState);
    }

    private void OnStateEnter(IAHealthState state)
    {
        switch (state)
        {
            case IAHealthState.AWAKE:
                OnEnterAwake();
                break;
            case IAHealthState.HURT:
                OnEnterHurt();
                break;
            case IAHealthState.DEAD:
                OnEnterDead();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(IAHealthState state)
    {
        switch (state)
        {
            case IAHealthState.AWAKE:
                OnUpdateAwake();
                break;
            case IAHealthState.HURT:
                OnUpdateHurt();
                break;
            case IAHealthState.DEAD:
                OnUpdateDead();
                break;
        }
    }
    private void OnStateFixedUpdate(IAHealthState state)
    {
        switch (state)
        {
            case IAHealthState.AWAKE:
                OnFixedUpdateAwake();
                break;
            case IAHealthState.HURT:
                OnFixedUpdateHurt();
                break;
            case IAHealthState.DEAD:
                OnFixedUpdateDead();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(IAHealthState state)
    {
        switch (state)
        {
            case IAHealthState.AWAKE:
                OnExitAwake();
                break;
            case IAHealthState.HURT:
                OnExitHurt();
                break;
            case IAHealthState.DEAD:
                OnExitDead();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(IAHealthState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterAwake()
    {
        _animator.SetBool("IsAwake", true);
    }
    private void OnUpdateAwake()
    {
        if (_combatStateMachine.CurrentState == IACombatState.PEACEFULL && _hitbox.DetectHit())
        {
            AttackData atk = _hitbox.GetHit();
            if (_enemyController.HealthPoints - atk.Damages > 0)
            {
                TransitionToState(IAHealthState.HURT);
                return;
            }
            else
            {
                TransitionToState(IAHealthState.DEAD);
                return;
            }
        }
    }
    private void OnFixedUpdateAwake()
    {
    }
    private void OnExitAwake()
    {
        _animator.SetBool("IsAwake", false);
    }

    private void OnEnterHurt()
    {
        _animator.SetBool("IsHurt", true);
        AttackData atk = _hitbox.GetHit();
        _enemyController.StartKnockBack(atk);
    }
    private void OnUpdateHurt()
    {
        if (_enemyController.IsKnockBackEnded)
        {
            TransitionToState(IAHealthState.AWAKE);
            return;
        }
        _enemyController.DoVerticalKnockback();
    }
    private void OnFixedUpdateHurt()
    {
        _enemyController.DoHorizontalKnockback();
    }
    private void OnExitHurt()
    {
        _animator.SetBool("IsHurt", false);
        _enemyController.EndKnockBack();
    }

    private void OnEnterDead()
    {
        _animator.SetBool("IsDead", true);
        _enemyController.StartVanishing();
    }
    private void OnUpdateDead()
    {
        _enemyController.DoVanishing();
    }
    private void OnFixedUpdateDead()
    {
    }
    private void OnExitDead()
    {
        _animator.SetBool("IsDead", false);
    }

}
