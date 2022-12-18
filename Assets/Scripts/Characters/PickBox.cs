using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBox : MonoBehaviour
{
    [SerializeField] private LayerMask _throwablesLayer;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    public Throwable GetThrowable()
    {
        Collider2D coll = Physics2D.OverlapArea(transform.position - _collider.bounds.extents, transform.position + _collider.bounds.extents, _throwablesLayer);
        if (coll == null)
        {
            return null;
        }
        Throwable throwable = coll.GetComponentInParent<Throwable>();
        return !throwable.IsPicked ? throwable : null;
    }
}
