using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulsor : MonoBehaviour
{
    [SerializeField] private Vector3 _force;
    [SerializeField] private Vector3 _variations;
    [SerializeField] private bool _impulseOnStart;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (_impulseOnStart)
        {
            Impulse();
        }
    }
    public void Impulse()
    {
        Vector3 force = new Vector3(_force.x + Random.Range(-_variations.x, _variations.x), _force.y + Random.Range(-_variations.y, _variations.y), _force.z + Random.Range(-_variations.z, _variations.z));
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}
