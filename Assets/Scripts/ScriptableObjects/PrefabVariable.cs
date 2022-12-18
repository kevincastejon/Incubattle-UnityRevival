using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PrefabVariable : ScriptableObject
{
    [SerializeField] private GameObject _value;
    public GameObject Value { get => _value; set => _value = value; }
}
