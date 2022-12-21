using KevinCastejon.MoreAttributes;
using UnityEngine;
using UnityEngine.Events;

public class LevelStage : MonoBehaviour
{
    [SerializeField] private Vector2 _triggerArea;
    [SerializeField] private Vector2 _constraintArea;
    [SerializeField] private LevelEvent[] _events;
    [SerializeField] private UnityEvent _onStarted;
    [SerializeField] private UnityEvent _onCompleted;
    [SerializeField] private Transform _stageWallsContainer;
    [SerializeField] private BoxCollider2D _stageWallL;
    [SerializeField] private BoxCollider2D _stageWallR;
    [SerializeField] private BoxCollider2D _stageWallD;
    [SerializeField] private BoxCollider2D _stageWallU;
    private Bounds _constraintBackup;
    private CameraFollow _camera;
    private int _currentEvent;
    private bool _isStarted;
    private bool _isCompleted;

    public LevelEvent[] Events { get => _events; }

    public void GenerateWalls()
    {
        if (_stageWallsContainer == null)
        {
            _stageWallsContainer = new GameObject().transform;
            _stageWallsContainer.parent = transform;
            _stageWallsContainer.position = transform.position;
            _stageWallsContainer.name = "Walls";
        }

        if (_stageWallL == null)
        {
            _stageWallL = new GameObject().AddComponent<BoxCollider2D>();
            _stageWallL.gameObject.name = "WallL";
            _stageWallL.transform.parent = _stageWallsContainer;
        }
        if (_stageWallR == null)
        {
            _stageWallR = new GameObject().AddComponent<BoxCollider2D>();
            _stageWallR.gameObject.name = "WallR";
            _stageWallR.transform.parent = _stageWallsContainer;
        }
        if (_stageWallD == null)
        {
            _stageWallD = new GameObject().AddComponent<BoxCollider2D>();
            _stageWallD.gameObject.name = "WallD";
            _stageWallD.transform.parent = _stageWallsContainer;
        }
        if (_stageWallU == null)
        {
            _stageWallU = new GameObject().AddComponent<BoxCollider2D>();
            _stageWallU.gameObject.name = "WallU";
            _stageWallU.transform.parent = _stageWallsContainer;
        }
    }

    public void TransformWalls()
    {
        GenerateWalls();
        _stageWallL.size = new Vector2(1f, _constraintArea.y);
        _stageWallR.size = new Vector2(1f, _constraintArea.y);
        _stageWallD.size = new Vector2(_constraintArea.x, 1f);
        _stageWallU.size = new Vector2(_constraintArea.x, 1f);
        _stageWallL.transform.position = transform.position - new Vector3((_constraintArea.x + 1f) * 0.5f, 0f, 0f);
        _stageWallR.transform.position = transform.position + new Vector3((_constraintArea.x + 1f) * 0.5f, 0f, 0f);
        _stageWallD.transform.position = transform.position - new Vector3(0f, (_constraintArea.y + 1f) * 0.5f, 0f);
        _stageWallU.transform.position = transform.position + new Vector3(0f, (_constraintArea.y + 1f) * 0.5f, 0f);
    }

    private void Awake()
    {
        _camera = FindObjectOfType<CameraFollow>();
        // On positionne les murs
        TransformWalls();
        // On désactive les murs
        _stageWallsContainer.gameObject.SetActive(false);
    }

    private void StartEvent()
    {
        _events[_currentEvent].Event.Init();
        if (!_events[_currentEvent].Wait)
        {
            SwitchToNextEvent();
        }
    }
    public void Update()
    {
        if (!_isStarted && Physics2D.OverlapArea((Vector2)transform.position - _triggerArea * 0.5f, (Vector2)transform.position + _triggerArea * 0.5f, LayerMask.GetMask(new string[] { "StageDetector" })))
        {
            Init();
        }
        else if (_isStarted && !_isCompleted && _events[_currentEvent].Event.IsOver())
        {
            SwitchToNextEvent();
        }
    }

    private void SwitchToNextEvent()
    {
        if (_currentEvent < _events.Length - 1)
        {
            _currentEvent++;
            StartEvent();
        }
        else
        {
            // On ré-assigne les contraintes originales de la camera
            _stageWallsContainer.gameObject.SetActive(false);
            _camera.Constraint = _constraintBackup;
            _isCompleted = true;
            _onCompleted.Invoke();
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!_isStarted && collision.CompareTag("StageDetector"))
    //    {
    //        Init();
    //    }
    //}
    public void Init()
    {
        // On conserve une reference vers les contraintes par défaut de la camera
        _constraintBackup = _camera.Constraint;
        // On change les contraintes de la camera en lui assignant les contraintes du stage
        _camera.Constraint = new Bounds(transform.position, _constraintArea);
        // On active les murs
        _stageWallsContainer.gameObject.SetActive(true);
        _isStarted = true;
        _onStarted.Invoke();
        StartEvent();
    }
}
