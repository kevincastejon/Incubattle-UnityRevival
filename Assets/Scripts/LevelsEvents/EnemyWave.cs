using KevinCastejon.MoreAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWave : MonoBehaviour, ILevelEvent
{
    
    [HideOnPrefab(true)] [SerializeField] private Transform _enemiesContainer;
    [HideOnPrefab(true)] [SerializeField] private GameObject _enemy1;
    [HideOnPrefab(true)] [SerializeField] private GameObject _enemy2;
    [HideOnPrefab(true)] [SerializeField] private GameObject _enemy3;
    [HideOnPrefab(true)] [SerializeField] private GameObject _dp1;
    [HideOnPrefab(true)] [SerializeField] private GameObject _dp2;
    [HideOnPrefab(true)] [SerializeField] private GameObject _dp3;
    [HideOnPrefab(true)] [SerializeField] private GameObject _dp4;
    [HideOnPrefab(true)] [SerializeField] private GameObject _dp5;
    [HideOnPrefab(true)] [SerializeField] private GameObject _boss;

    [SerializeField] private int _enemy1Count;
    [SerializeField] private int _enemy2Count;
    [SerializeField] private int _enemy3Count;
    [SerializeField] private int _dp1Count;
    [SerializeField] private int _dp2Count;
    [SerializeField] private int _dp3Count;
    [SerializeField] private int _dp4Count;
    [SerializeField] private int _dp5Count;
    [SerializeField] private int _bossCount;
    [SerializeField] private UnityEvent _onComplete;

    
    private CameraFollow _camera;
    private bool _isStarted;
    private bool _isCompleted;

    private void Awake()
    {
        _camera = FindObjectOfType<CameraFollow>();
    }

    public void Init()
    {
        // On note que la vague a demarré
        _isStarted = true;
        // On active tous les ennemis de la vague
        for (int i = 0; i < _enemy1Count; i++) { Instantiate(_enemy1, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
        for (int i = 0; i < _enemy2Count; i++) { Instantiate(_enemy2, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
        for (int i = 0; i < _enemy3Count; i++) { Instantiate(_enemy3, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
        for (int i = 0; i < _dp1Count; i++) { Instantiate(_dp1, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
        for (int i = 0; i < _dp2Count; i++) { Instantiate(_dp2, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
        for (int i = 0; i < _dp3Count; i++) { Instantiate(_dp3, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
        for (int i = 0; i < _dp4Count; i++) { Instantiate(_dp4, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
        for (int i = 0; i < _dp5Count; i++) { Instantiate(_dp5, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
        for (int i = 0; i < _bossCount; i++) { Instantiate(_boss, GetRandomPosition(), Quaternion.identity, _enemiesContainer); }
    }

    public bool IsOver()
    {
        return _isCompleted;
    }
    
    private void Update()
    {
        // Si tous les ennemis sont morts
        if (!_isCompleted && _isStarted && _enemiesContainer.childCount == 0)
        {
            // On déclenche l'evenement (Unityevent) de fin de vague
            _onComplete.Invoke();
            //
            _isCompleted = true;
            // On détruit l'objet Wave
            //Destroy(gameObject);
        }
    }

    //// Si la vague n'est pas démarrée et que le joueur pénètre la zone on démarre la vague
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!_isStarted && collision.CompareTag("WaveDetector"))
    //    {
    //        StartWave();
    //    }
    //}

    private Vector2 GetRandomPosition()
    {
        bool left = Random.value > 0.5f;
        Bounds bounds = _camera.GetCameraBoundsFromPosition(transform.position);
        float h = left ? bounds.min.x : bounds.max.x;
        float v = Random.Range(0.5f , - 3.75f);
        return new Vector2(h, v);
    }
}
