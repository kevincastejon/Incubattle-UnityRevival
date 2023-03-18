using UnityEngine;

public enum BossHandState
{
    IDLE,
    HITTING,
}

public class BossHandStateMachine : MonoBehaviour
{
    private BossHandState _currentState;
    private BossHandController _controller;

    public BossHandState CurrentState { get => _currentState; private set => _currentState = value; }

    private void Awake()
    {
        _controller = GetComponent<BossHandController>();
    }
    private void Update()
    {
        OnStateUpdate(CurrentState);
    }

    private void OnStateEnter(BossHandState state)
    {
        switch (state)
        {
            case BossHandState.IDLE:
                OnEnterIdle();
                break;
            case BossHandState.HITTING:
                OnEnterHitting();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(BossHandState state)
    {
        switch (state)
        {
            case BossHandState.IDLE:
                OnUpdateIdle();
                break;
            case BossHandState.HITTING:
                OnUpdateHitting();
                break;
        }
    }
    private void OnStateExit(BossHandState state)
    {
        switch (state)
        {
            case BossHandState.IDLE:
                OnExitIdle();
                break;
            case BossHandState.HITTING:
                OnExitHitting();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(BossHandState toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterIdle()
    {
    }
    private void OnUpdateIdle()
    {
        if (_controller.IsPlayerDetected)
        {
            TransitionToState(BossHandState.HITTING);
        }
    }
    private void OnExitIdle()
    {
    }

    private void OnEnterHitting()
    {
        _controller.StartHit();
    }
    private void OnUpdateHitting()
    {
        if (_controller.IsAttackEnded)
        {
            TransitionToState(BossHandState.IDLE);
            return;
        }

        _controller.DoHit();
    }
    private void OnExitHitting()
    {
        _controller.StopHit();
    }

}
