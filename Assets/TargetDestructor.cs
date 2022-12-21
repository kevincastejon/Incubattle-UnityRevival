using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDestructor : MonoBehaviour
{
    [SerializeField] private Object _target;
    [SerializeField] private bool _selfDestroyWithTarget;

    public Object Target { get => _target; set => _target = value; }
    public bool SelfDestroyWithTarget { get => _selfDestroyWithTarget; set => _selfDestroyWithTarget = value; }

    public void Destroy()
    {
        if (_target is Component)
        {
            Destroy(((Component)_target).gameObject);
        }
        else
        {
            Destroy(_target);
        }
        if (_selfDestroyWithTarget)
        {
            Destroy(gameObject);
        }
    }
}
