using KevinCastejon.MoreAttributes;
using UnityEngine;
using UnityEngine.Events;

public enum MenuStates
{
    MODE_SELECTION,
    SOLO_PICKING,
    COOP_PICKING,
    COOP_P1PICKED,
    COOP_P2PICKED,
    OUTRO,
}

public class MenuStateMachine : MonoBehaviour
{
    [ReadOnly] [SerializeField] private MenuStates _currentState;
    [SerializeField] private IntVariable _score;
    [SerializeField] private IntVariable _currentLevel;
    [SerializeField] private Selector _modeSelector;
    [SerializeField] private Selector _p1Selector;
    [SerializeField] private Selector _p2Selector;
    [SerializeField] private BoolVariable _isCoop;
    [SerializeField] private PrefabVariable _p1Character;
    [SerializeField] private PrefabVariable _p2Character;
    [SerializeField] private GameObject[] _charactersPrefabs;
    [SerializeField] private Animator[] _charactersPreviews;

    [SerializeField] private UnityEvent _onEnterModeSelection;
    [SerializeField] private UnityEvent _onExitModeSelection;
    [SerializeField] private UnityEvent _onEnterSoloPicking;
    [SerializeField] private UnityEvent _onExitSoloPicking;
    [SerializeField] private UnityEvent _onEnterCoopPicking;
    [SerializeField] private UnityEvent _onExitCoopPicking;
    [SerializeField] private UnityEvent _onEnterCoopP1Picking;
    [SerializeField] private UnityEvent _onExitCoopP1Picking;
    [SerializeField] private UnityEvent _onEnterCoopP2Picking;
    [SerializeField] private UnityEvent _onExitCoopP2Picking;
    [SerializeField] private UnityEvent _onEnterOutro;
    [SerializeField] private UnityEvent _onExitOutro;


    public MenuStates CurrentState { get => _currentState; private set => _currentState = value; }
    private Inputs _inputs;
    private void Awake()
    {
        _inputs = new Inputs();
        _inputs.UI.Enable();
    }
    private void Start()
    {
        _score.Value = 0;
        _currentLevel.Value = 0;
    }
    private void Update()
    {
        OnStateUpdate(CurrentState);
    }

    private void OnStateEnter(MenuStates state)
    {
        switch (state)
        {
            case MenuStates.MODE_SELECTION:
                OnEnterModeSelection();
                break;
            case MenuStates.SOLO_PICKING:
                OnEnterSoloPicking();
                break;
            case MenuStates.COOP_PICKING:
                OnEnterCoopPicking();
                break;
            case MenuStates.COOP_P1PICKED:
                OnEnterCoopP1Picked();
                break;
            case MenuStates.COOP_P2PICKED:
                OnEnterCoopP2Picked();
                break;
            case MenuStates.OUTRO:
                OnEnterOutro();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(MenuStates state)
    {
        switch (state)
        {
            case MenuStates.MODE_SELECTION:
                OnUpdateModeSelection();
                break;
            case MenuStates.SOLO_PICKING:
                OnUpdateSoloPicking();
                break;
            case MenuStates.COOP_PICKING:
                OnUpdateCoopPicking();
                break;
            case MenuStates.COOP_P1PICKED:
                OnUpdateCoopP1Picked();
                break;
            case MenuStates.COOP_P2PICKED:
                OnUpdateCoopP2Picked();
                break;
            case MenuStates.OUTRO:
                OnUpdateOutro();
                break;
        }
    }
    private void OnStateExit(MenuStates state)
    {
        switch (state)
        {
            case MenuStates.MODE_SELECTION:
                OnExitModeSelection();
                break;
            case MenuStates.SOLO_PICKING:
                OnExitSoloPicking();
                break;
            case MenuStates.COOP_PICKING:
                OnExitCoopPicking();
                break;
            case MenuStates.COOP_P1PICKED:
                OnExitCoopP1Picked();
                break;
            case MenuStates.COOP_P2PICKED:
                OnExitCoopP2Picked();
                break;
            case MenuStates.OUTRO:
                OnExitOutro();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(MenuStates toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterModeSelection()
    {
        _onEnterModeSelection.Invoke();
    }
    private void OnUpdateModeSelection()
    {
        if (_modeSelector.Selected)
        {
            _isCoop.Value = _modeSelector.Selection == 1;
            if (_modeSelector.Selection == 0)
            {
                TransitionToState(MenuStates.SOLO_PICKING);
                return;
            }
            if (_modeSelector.Selection == 1)
            {
                TransitionToState(MenuStates.COOP_PICKING);
                return;
            }
        }
    }
    private void OnExitModeSelection()
    {
        _onExitModeSelection.Invoke();
    }

    private void OnEnterSoloPicking()
    {
        _onEnterSoloPicking.Invoke();
    }
    private void OnUpdateSoloPicking()
    {
        if (_inputs.UI.Cancel.triggered)
        {
            _modeSelector.Unselect();
            TransitionToState(MenuStates.MODE_SELECTION);
            return;
        }
        if (_p1Selector.Selected)
        {
            _p1Character.Value = _charactersPrefabs[_p1Selector.Selection];
            TransitionToState(MenuStates.OUTRO);
            return;
        }
    }
    private void OnExitSoloPicking()
    {
        _onExitSoloPicking.Invoke();
    }

    private void OnEnterCoopPicking()
    {
        _onEnterCoopPicking.Invoke();
    }
    private void OnUpdateCoopPicking()
    {
        if (_inputs.UI.Cancel.triggered)
        {
            _modeSelector.Unselect();
            TransitionToState(MenuStates.MODE_SELECTION);
            return;
        }
        if (_p1Selector.Selected)
        {
            _p1Character.Value = _charactersPrefabs[_p1Selector.Selection];
            TransitionToState(MenuStates.COOP_P1PICKED);
            return;
        }
        if (_p2Selector.Selected)
        {
            _p2Character.Value = _charactersPrefabs[_p1Selector.Selection];
            TransitionToState(MenuStates.COOP_P2PICKED);
            return;
        }
    }
    private void OnExitCoopPicking()
    {
        _onExitCoopPicking.Invoke();
    }

    private void OnEnterCoopP1Picked()
    {
        _onEnterCoopP1Picking.Invoke();
    }
    private void OnUpdateCoopP1Picked()
    {
        if (_inputs.UI.Cancel.triggered)
        {
            _p1Selector.Unselect();
            TransitionToState(MenuStates.COOP_PICKING);
            return;
        }
        if (_p2Selector.Selected)
        {
            if (_p2Selector.Selection == _p1Selector.Selection)
            {
                _p2Selector.Unselect();
                return;
            }
            _p2Character.Value = _charactersPrefabs[_p2Selector.Selection];
            TransitionToState(MenuStates.OUTRO);
            return;

        }
    }
    private void OnExitCoopP1Picked()
    {
        _onExitCoopP1Picking.Invoke();
    }

    private void OnEnterCoopP2Picked()
    {
        _onEnterCoopP2Picking.Invoke();
    }
    private void OnUpdateCoopP2Picked()
    {
        if (_inputs.UI.Cancel.triggered)
        {
            _p2Selector.Unselect();
            TransitionToState(MenuStates.COOP_PICKING);
            return;
        }
        if (_p1Selector.Selected)
        {
            if (_p2Selector.Selection == _p1Selector.Selection)
            {
                _p1Selector.Unselect();
                return;
            }
            _p1Character.Value = _charactersPrefabs[_p1Selector.Selection];
            TransitionToState(MenuStates.OUTRO);
            return;
        }
    }
    private void OnExitCoopP2Picked()
    {
        _onExitCoopP2Picking.Invoke();
    }

    private void OnEnterOutro()
    {
        _onEnterOutro.Invoke();
        _charactersPreviews[_p1Selector.Selection].SetTrigger("Intro");
        if (_isCoop.Value)
        {
            _charactersPreviews[_p2Selector.Selection].SetTrigger("Intro");
        }
    }
    private void OnUpdateOutro()
    {
    }
    private void OnExitOutro()
    {
        _onExitOutro.Invoke();
    }

}
