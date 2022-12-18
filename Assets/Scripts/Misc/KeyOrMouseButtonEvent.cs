using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InputEventType
{
    KEYCODE,
    VIRTUAL_BUTTON
}

[Serializable]
public class InputEvent
{
    [SerializeField] private InputEventType _type;
    [SerializeField] private KeyCode _key;
    [SerializeField] private string _virtualButtonName;
    [SerializeField] private UnityEvent _downEvent;
    [SerializeField] private UnityEvent _upEvent;

    public InputEventType Type { get => _type; }
    public KeyCode Key { get => _key; }
    public string VirtualButtonName { get => _virtualButtonName; }
    public UnityEvent DownEvent { get => _downEvent; }
    public UnityEvent UpEvent { get => _upEvent; }
}

public class KeyOrMouseButtonEvent : MonoBehaviour
{
    [SerializeField] private InputEvent[] _events;

    private void Update()
    {
        for (int i = 0; i < _events.Length; i++)
        {
            if (_events[i].Type == InputEventType.KEYCODE)
            {
                if (_events[i].Key == KeyCode.None)
                {
                    if (Input.anyKeyDown)
                    {
                        _events[i].DownEvent.Invoke();
                    }
                }
                else 
                {
                    if (Input.GetKeyDown(_events[i].Key))
                    {
                        _events[i].DownEvent.Invoke();
                    }
                    else if (Input.GetKeyUp(_events[i].Key))
                    {
                        _events[i].UpEvent.Invoke();
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown(_events[i].VirtualButtonName))
                {
                    _events[i].DownEvent.Invoke();
                }
                else if (Input.GetButtonUp(_events[i].VirtualButtonName))
                {
                    _events[i].UpEvent.Invoke();
                }
            }
        }
    }
}
