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
    [SerializeField] private GameObject _shadow;
    [SerializeField] private IntVariable _healthPoints;
    [SerializeField] private IntVariable _maxHealthPoints;
    [SerializeField] private IntVariable _score;
    [Header("Collectible Settings")]
    [SerializeField] private CollectibleType _type;
    [SerializeField] private int _value;
    [SerializeField] private AudioClip _sound;
    [SerializeField] private Vector2 _initialDirection;
    [SerializeField] private Vector2 _initialVariation;
    private Rigidbody2D _rigidbody;
    private Collider2D _trigger;
    private AudioSource _audioSource;
    private ColorAnimator _fader;
    private float _spriteVerticalVelocity;
    private float _gravityValue = -15f;
    private bool _isGrounded;
    private Vector2 _lootPosition;
    private float _lootDuration = 0.5f;
    private float _lootEndTime;
    private bool _isLooted;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        // On récupère l'AudioSource SFX sur la scène
        _audioSource = GameObject.FindGameObjectWithTag("SfxSource").GetComponent<AudioSource>();
        // 
        _trigger = _sprite.GetComponent<Collider2D>();
        _fader = _sprite.GetComponent<ColorAnimator>();
    }

    private void Start()
    {
        _rigidbody.AddForce(new Vector2(_initialDirection.x + Random.Range(-_initialVariation.x, _initialVariation.x), _initialDirection.y + Random.Range(-_initialVariation.y, _initialVariation.y)), ForceMode2D.Impulse);
        _spriteVerticalVelocity = Random.Range(6f, 8f);
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
                _trigger.enabled = true;
            }
        }
        else if (_isLooted)
        {
            if (Time.time >= _lootEndTime)
            {
                Destroy(gameObject);
            }
            else
            {
                float t = (_lootDuration - (_lootEndTime - Time.time)) / _lootDuration;
                transform.position = Vector2.Lerp(_lootPosition, (Vector2)Camera.main.transform.position + Vector2.up * Camera.main.orthographicSize, t);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lorsque le joueur pénètre la zone
        if (_isGrounded && collision.CompareTag("Player"))
        {
            // On joue un son
            _audioSource.PlayOneShot(_sound);
            // Si le Collectible est de type HEALTH
            if (_type == CollectibleType.HEALTH)
            {
                // On calcule la nouvelle valeur des points de vie
                int newHealthPoints = _healthPoints.Value + _value;
                // On augmente les points de vie (sans dépasser la valeur de points de vie max)
                _healthPoints.Value = Mathf.Min(newHealthPoints, _maxHealthPoints.Value);
                // Et on détruit le Collectible
                Destroy(gameObject);
            }
            // Sinon le Collectible est de type SCORE
            else
            {
                // On augmente le score
                _score.Value += _value;
                _isLooted = true;
                _lootPosition = transform.position;
                _lootEndTime = Time.time + _lootDuration;
                _trigger.enabled = false;
                _fader.Init();
                _shadow.SetActive(false);
            }
        }
    }
}
