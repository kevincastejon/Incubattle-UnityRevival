using KevinCastejon.MoreAttributes;
using UnityEngine;

public enum BossState
{
    THINKING,
    WALKING,
    HUNTING,
    RISING,
    ATTACKING,
}

public class BossStateMachine : MonoBehaviour
{
    [SerializeField] [ReadOnly]private BossState _currentState;
    private BossController _controller;
    [SerializeField] private BossHandStateMachine _leftHandStateMachine;
    [SerializeField] private BossHandStateMachine _rightHandStateMachine;
    [SerializeField] private WeightedRandomPicker _unrisedIA;
    private Animator _animator;
    private Rigidbody2D _rigidbody;

    public BossState CurrentState { get => _currentState; private set => _currentState = value; }

    private void Awake()
    {
        _controller = GetComponent<BossController>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

        OnStateUpdate(CurrentState);
    }
    private void FixedUpdate()
    {
        _rigidbody.bodyType = _leftHandStateMachine.CurrentState == BossHandState.HITTING || _rightHandStateMachine.CurrentState == BossHandState.HITTING ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        OnStateFixedUpdate(CurrentState);
    }

    private void OnStateEnter(BossState state)
    {
        switch (state)
        {
            case BossState.THINKING:
                OnEnterThinking();
                break;
            case BossState.WALKING:
                OnEnterWalking();
                break;
            case BossState.HUNTING:
                OnEnterHunting();
                break;
            case BossState.RISING:
                OnEnterRising();
                break;
            case BossState.ATTACKING:
                OnEnterAttacking();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(BossState state)
    {
        if (OnUpdateAnyState())
        {
            return;
        }
        switch (state)
        {
            case BossState.THINKING:
                OnUpdateThinking();
                break;
            case BossState.WALKING:
                OnUpdateWalking();
                break;
            case BossState.HUNTING:
                OnUpdateHunting();
                break;
            case BossState.RISING:
                OnUpdateRising();
                break;
            case BossState.ATTACKING:
                OnUpdateAttacking();
                break;
        }
    }
    private void OnStateFixedUpdate(BossState state)
    {
        switch (state)
        {
            case BossState.THINKING:
                OnFixedUpdateThinking();
                break;
            case BossState.WALKING:
                OnFixedUpdateWalking();
                break;
            case BossState.HUNTING:
                OnFixedUpdateHunting();
                break;
            case BossState.RISING:
                OnFixedUpdateRising();
                break;
            case BossState.ATTACKING:
                OnFixedUpdateAttacking();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateExit(BossState state)
    {
        switch (state)
        {
            case BossState.THINKING:
                OnExitThinking();
                break;
            case BossState.WALKING:
                OnExitWalking();
                break;
            case BossState.HUNTING:
                OnExitHunting();
                break;
            case BossState.RISING:
                OnExitRising();
                break;
            case BossState.ATTACKING:
                OnExitAttacking();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(BossState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private bool OnUpdateAnyState()
    {
        if (!_controller.IsRised && _leftHandStateMachine == null && _rightHandStateMachine == null)
        {
            TransitionToState(BossState.RISING);
            return true;
        }
        return false;
    }

    private void OnEnterThinking()
    {
        Debug.Log("THINK");
        _controller.StartThinking();
    }
    private void OnUpdateThinking()
    {
        if (_controller.IsThinkingEnded && (_leftHandStateMachine == null || _leftHandStateMachine.CurrentState == BossHandState.IDLE) && (_rightHandStateMachine == null || _rightHandStateMachine.CurrentState == BossHandState.IDLE))
        {
            string pickedName = _unrisedIA.PickName();
            Debug.Log(pickedName);
            if (pickedName == "ThinkAgain")
            {
                TransitionToState(BossState.THINKING);
                return;
            }
            else if (pickedName == "Walk")
            {
                TransitionToState(BossState.WALKING);
                return;
            }
            else if (pickedName == "Hunt")
            {
                TransitionToState(BossState.HUNTING);
                return;
            }
        }
    }
    private void OnFixedUpdateThinking()
    {
        _controller.DoIdle();
    }
    private void OnExitThinking()
    {
        _controller.EndThink();
    }

    private void OnEnterWalking()
    {
        Debug.Log("WALK");
    }
    private void OnUpdateWalking()
    {
        if ((_leftHandStateMachine != null && _leftHandStateMachine.CurrentState == BossHandState.HITTING) || (_rightHandStateMachine != null && _rightHandStateMachine.CurrentState == BossHandState.HITTING) || _controller.IsThinkingTime)
        {
            TransitionToState(BossState.THINKING);
            return;
        }
    }
    private void OnFixedUpdateWalking()
    {
        _controller.DoWalk();
    }
    private void OnExitWalking()
    {
    }

    private void OnEnterHunting()
    {
        Debug.Log("HUNT");
        _controller.FindTarget();
    }
    private void OnUpdateHunting()
    {
        if (_controller.IsRised && _controller.IsTargetReachable)
        {
            TransitionToState(BossState.ATTACKING);
            return;
        }
        if (!_controller.IsRised && ((_leftHandStateMachine != null && _leftHandStateMachine.CurrentState == BossHandState.HITTING) || (_rightHandStateMachine != null && _rightHandStateMachine.CurrentState == BossHandState.HITTING) || _controller.IsThinkingTime))
        {
            TransitionToState(BossState.THINKING);
            return;
        }

        _controller.DoHunt();
    }
    private void OnFixedUpdateHunting()
    {
    }
    private void OnExitHunting()
    {
    }

    private void OnEnterRising()
    {
        _controller.StartRise();
    }
    private void OnUpdateRising()
    {
        if (_controller.IsRisingEnded)
        {
            Debug.Log("HUNT");
            TransitionToState(BossState.HUNTING);
            return;
        }
        _controller.DoRise();
    }
    private void OnFixedUpdateRising()
    {
    }
    private void OnExitRising()
    {
    }

    private void OnEnterAttacking()
    {
        _animator.SetTrigger("Attack");
        _controller.DoIdle();
        _controller.StartHit();
    }
    private void OnUpdateAttacking()
    {
        if (_controller.IsAttackEnded)
        {
            TransitionToState(BossState.RISING);
            return;
        }

        _controller.DoHit();
    }
    private void OnFixedUpdateAttacking()
    {
    }
    private void OnExitAttacking()
    {
    }

}
