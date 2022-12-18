using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject
{
    [SerializeField] private bool _value;
    private UnityEvent<bool> _onChange = new UnityEvent<bool>();
    public bool Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            _onChange.Invoke(_value);
        }
    }
    public UnityEvent<bool> OnChange { get => _onChange; }
}
