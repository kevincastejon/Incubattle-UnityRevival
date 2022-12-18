using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    [SerializeField] private int _value;
    private UnityEvent<int> _onChange = new UnityEvent<int>();
    public int Value
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
    public UnityEvent<int> OnChange { get => _onChange; }

}
