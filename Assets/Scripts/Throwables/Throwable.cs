using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private Transform _canSprite;
    [SerializeField] private Transform _canShadow;
    [SerializeField] private ZAxisSorter _canZAxisSorter;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _usingCount = 5;
    private Rigidbody2D _rigidbody;
    private Transform _feet;
    private bool _isGrounded = true;
    private bool _isPicked = false;
    private float _spriteVerticalVelocity;
    private float _gravityValue = -15f;
    private float _fallHeight;
    private int _idleLayer;
    public bool IsPicked { get => _isPicked; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _idleLayer = gameObject.layer;
    }
    public void Pick(Transform feet, Transform hands)
    {
        _feet = feet;
        _canShadow.gameObject.SetActive(false);
        //
        _canZAxisSorter.MainTransform = feet;
        //
        transform.parent = hands;
        //
        transform.position = hands.position;
    }
    public void Throw(Vector2 direction, int _attackingLayer)
    {
        Vector3 handsPos = transform.position;
        transform.position = _feet.position;
        _canSprite.position = handsPos;
        _usingCount--;
        gameObject.layer = _attackingLayer;
        _canSprite.gameObject.layer = _attackingLayer;
        _spriteVerticalVelocity = 0f;
        transform.parent = null;
        _isGrounded = false;
        _isPicked = false;
        _canShadow.gameObject.SetActive(true);
        _canZAxisSorter.MainTransform = transform;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.AddForce(direction * _speed, ForceMode2D.Impulse);
        _fallHeight = Vector2.Distance(new Vector2(0f, _canSprite.position.y), new Vector2(0f, transform.position.y));
    }

    private void Update()
    {
        if (!_isGrounded)
        {
            _spriteVerticalVelocity += _gravityValue * Time.deltaTime;
            _canSprite.Translate(new Vector2(0f, _spriteVerticalVelocity) * Time.deltaTime, Space.World);
            if (_canSprite.localPosition.y <= 0f)
            {
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;
                _isGrounded = true;
                _rigidbody.velocity = Vector2.zero;
                _canSprite.localPosition = Vector2.zero;
                gameObject.layer = _idleLayer;
                _canSprite.gameObject.layer = _idleLayer;
                if (_usingCount == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private float GetFallProgress()
    {
        return 1f - (_canSprite.localPosition.y / _fallHeight);
    }
}
