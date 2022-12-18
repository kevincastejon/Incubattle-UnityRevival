using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    HEALTH,
    SCORE
}

public class Collectible : MonoBehaviour
{
    [Header("Références Settings")]
    [SerializeField] private Transform _sprite;
    [SerializeField] private IntVariable _healthPoints;
    [SerializeField] private IntVariable _maxHealthPoints;
    [SerializeField] private IntVariable _score;
    [Header("Collectible Settings")]
    [SerializeField] private CollectibleType _type;
    [SerializeField] private int _value;
    [SerializeField] private AudioClip _sound;
    private Rigidbody2D _rigidbody;
    private AudioSource _audioSource;
    private float _spriteVerticalVelocity;
    private float _gravityValue = -15f;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        // On récupère l'AudioSource SFX sur la scène
        _audioSource = GameObject.FindGameObjectWithTag("SfxSource").GetComponent<AudioSource>();
    }

    private void Start()
    {
        _rigidbody.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode2D.Impulse);
        _spriteVerticalVelocity = Random.Range(6f,8f);
    }

    private void Update()
    {
        if (!_isGrounded)
        {
            _spriteVerticalVelocity += _gravityValue * Time.deltaTime;
            _sprite.Translate(new Vector2(0f, _spriteVerticalVelocity) * Time.deltaTime, Space.World);
            if (_sprite.localPosition.y <= 0f)
            {
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;
                _isGrounded = true;
                _rigidbody.velocity = Vector2.zero;
                _sprite.localPosition = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lorsque le joueur pénètre la zone
        if (_isGrounded && collision.CompareTag("Player"))
        {
            // Si le Collectible est de type HEALTH
            if (_type == CollectibleType.HEALTH)
            {
                // On calcule la nouvelle valeur des points de vie
                int newHealthPoints = _healthPoints.Value + _value;
                // On augmente les points de vie (sans dépasser la valeur de points de vie max)
                _healthPoints.Value = Mathf.Min(newHealthPoints, _maxHealthPoints.Value);
            }
            // Sinon le Collectible est de type SCORE
            else
            {
                // On augmente le score
                _score.Value += _value;
            }
            // On joue un son
            _audioSource.PlayOneShot(_sound);
            // Et on détruit le Collectible
            Destroy(gameObject);
        }
    }
}
