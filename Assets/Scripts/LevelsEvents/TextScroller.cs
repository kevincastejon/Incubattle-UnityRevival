using KevinCastejon.MoreAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextScroller : MonoBehaviour, ILevelEvent
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _delayBetweenChars = 0.05f;
    [SerializeField] private float _delayBetweenMessages = 2f;
    [SerializeField] private bool _autoRun = true;
    [SerializeField] private string[] _messages;
    [SerializeField] UnityEvent _onComplete;
    private TextScrollStateMachine _stateMachine = new TextScrollStateMachine();

    private int _currentMessage = 0;
    private int _currentChar;
    private bool _isInitialized;
    private bool _completed;
    private bool _isMessageDisplayed;

    private float _nextCharTime;
    private float _nextMessageTime;

    public bool IsInitialized { get => _isInitialized; }
    public bool IsMessageDisplayed { get => _isMessageDisplayed; }
    public bool IsWaitingEnded { get => Time.time > _nextMessageTime; }
    public bool IsMessageLeft { get => _currentMessage < _messages.Length; }

    private void Awake()
    {
        _stateMachine.Init(this);
    }
    private void Start()
    {
        if (_autoRun)
        {
            Init();
        }
    }
    private void Update()
    {
        _stateMachine.Update();
    }
    public void Init()
    {

        _isInitialized = true;
    }

    public void StartScrollMessage()
    {
        _currentChar = 1;
        _nextCharTime = Time.time;
        _isMessageDisplayed = false;
        _text.text = _messages[_currentMessage].Substring(0, _currentChar) + "<color=#00000000>" + _messages[_currentMessage].Substring(_currentChar, _messages[_currentMessage].Length - _currentChar) + "</color>";
        _currentChar++;
        _audioSource.Play();
    }
    public void DoScrollMessage()
    {
        if (Time.time > _nextCharTime)
        {
            _text.text = _messages[_currentMessage].Substring(0, _currentChar) + "<color=#00000000>" + _messages[_currentMessage].Substring(_currentChar, _messages[_currentMessage].Length - _currentChar) + "</color>";
            _nextCharTime = Time.time + _delayBetweenChars;
            _currentChar++;
            if (_currentChar > _messages[_currentMessage].Length)
            {
                _isMessageDisplayed = true;
            }
        }
    }
    public void StartWait()
    {
        _currentMessage++;
        _audioSource.Stop();
        _nextMessageTime = Time.time + _delayBetweenMessages;
    }

    public void Complete()
    {
        _completed = true;
        _onComplete.Invoke();
        _text.text = "";
    }

    public bool IsOver()
    {
        return _completed;
    }
}
public enum TextScrollStates
{
    OFF,
    SCROLLING,
    WAITING,
    COMPLETED,
}

public class TextScrollStateMachine
{
    private TextScrollStates _currentState;
    private TextScroller _textScroller;
    public TextScrollStates CurrentState { get => _currentState; private set => _currentState = value; }

    public void Init(TextScroller textScroller)
    {
        _textScroller = textScroller;
    }
    public void Update()
    {
        OnStateUpdate(CurrentState);
    }

    private void OnStateEnter(TextScrollStates state)
    {
        switch (state)
        {
            case TextScrollStates.OFF:
                OnEnterOff();
                break;
            case TextScrollStates.SCROLLING:
                OnEnterScrolling();
                break;
            case TextScrollStates.WAITING:
                OnEnterWaiting();
                break;
            case TextScrollStates.COMPLETED:
                OnEnterCompleted();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    private void OnStateUpdate(TextScrollStates state)
    {
        switch (state)
        {
            case TextScrollStates.OFF:
                OnUpdateOff();
                break;
            case TextScrollStates.SCROLLING:
                OnUpdateScrolling();
                break;
            case TextScrollStates.WAITING:
                OnUpdateWaiting();
                break;
            case TextScrollStates.COMPLETED:
                OnUpdateCompleted();
                break;
        }
    }
    private void OnStateExit(TextScrollStates state)
    {
        switch (state)
        {
            case TextScrollStates.OFF:
                OnExitOff();
                break;
            case TextScrollStates.SCROLLING:
                OnExitScrolling();
                break;
            case TextScrollStates.WAITING:
                OnExitWaiting();
                break;
            case TextScrollStates.COMPLETED:
                OnExitCompleted();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    private void TransitionToState(TextScrollStates toState)
    {
        OnStateExit(CurrentState);
        CurrentState = toState;
        OnStateEnter(toState);
    }

    private void OnEnterOff()
    {

    }
    private void OnUpdateOff()
    {
        if (_textScroller.IsInitialized)
        {
            TransitionToState(TextScrollStates.SCROLLING);
            return;
        }
    }
    private void OnExitOff()
    {
    }

    private void OnEnterScrolling()
    {

        _textScroller.StartScrollMessage();
    }
    private void OnUpdateScrolling()
    {
        if (_textScroller.IsMessageDisplayed)
        {
            TransitionToState(TextScrollStates.WAITING);
            return;
        }

        _textScroller.DoScrollMessage();
    }
    private void OnExitScrolling()
    {
    }

    private void OnEnterWaiting()
    {
        _textScroller.StartWait();
    }
    private void OnUpdateWaiting()
    {
        if (_textScroller.IsWaitingEnded)
        {
            if (_textScroller.IsMessageLeft)
            {
                TransitionToState(TextScrollStates.SCROLLING);
                return;
            }
            else
            {
                TransitionToState(TextScrollStates.COMPLETED);
                return;
            }
        }
    }
    private void OnExitWaiting()
    {
    }

    private void OnEnterCompleted()
    {
        _textScroller.Complete();
    }
    private void OnUpdateCompleted()
    {
    }
    private void OnExitCompleted()
    {
    }
}