using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [ExecuteAlways]
public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform _far;
    [SerializeField] private Transform _mid;
    [SerializeField] private Transform _close;
    [SerializeField] private float _speed = 0.5f;

    private bool _isMoving;

    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    void Update()
    {
        if (_isMoving)
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
        }

        _close.position = new Vector3(_close.position.x, transform.position.y * 2f, _close.position.z);
        _mid.position = new Vector3(_mid.position.x, transform.position.y, _mid.position.z);
        _far.position = new Vector3(_far.position.x, transform.position.y * 0.5f, _far.position.z);
    }
}
