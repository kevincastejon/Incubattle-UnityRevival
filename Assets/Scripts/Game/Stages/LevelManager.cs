using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private IntVariable _currentLevel;
    [SerializeField] private BoolVariable _isCoop;
    [SerializeField] private PrefabVariable _p1Prefab;
    [SerializeField] private PrefabVariable _p2Prefab;
    [SerializeField] private IntVariable _p2HealthSO;
    [SerializeField] private IntVariable _p2StaminaSO;
    [SerializeField] private IntVariable _p2LifesSO;
    [SerializeField] private Transform _p1Starter;
    [SerializeField] private Transform _p2Starter;
    [SerializeField] private string[] _levels;
    private PlayerController _p1;
    private PlayerController _p2;

    private CameraFollow _cameraFollow;

    private void Awake()
    {
        _cameraFollow = FindObjectOfType<CameraFollow>();
    }

    private void Update()
    {
        if ((!_isCoop.Value && _p1 == null) || (_isCoop.Value && _p1 == null && _p2 == null))
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    private void Start()
    {
        _p1 = Instantiate(_p1Prefab.Value, _p1Starter.position, Quaternion.identity).GetComponent<PlayerController>();
        _cameraFollow.Target1 = _p1.transform;
        if (_isCoop.Value)
        {
            _p2 = Instantiate(_p2Prefab.Value, _p2Starter.position, Quaternion.identity).GetComponent<PlayerController>();
            _cameraFollow.Target2 = _p2.transform;
            PlayerInput p2Input = _cameraFollow.Target2.GetComponent<PlayerInput>();
            p2Input.HorizontalAxisName += "P2";
            p2Input.VerticalAxisName += "P2";
            p2Input.JumpButtonName += "P2";
            p2Input.SprintButtonName += "P2";
            p2Input.AttackButtonName += "P2";
            p2Input.SpecialButtonName += "P2";
            PlayerController p2Controller = _cameraFollow.Target2.GetComponent<PlayerController>();
            p2Controller.HealthPointsSO = _p2HealthSO;
            p2Controller.StaminaSO = _p2StaminaSO;
            p2Controller.LifesSO = _p2LifesSO;
        }
    }

    public void StartNextLevel()
    {
        // On incrémente l'index du niveau actuel
        _currentLevel.Value++;
        // Puis on charge la scène
        SceneManager.LoadScene(_levels[_currentLevel.Value]);
    }
    public void RestartLevel()
    {
        // on recharge la scène
        SceneManager.LoadScene(_levels[_currentLevel.Value]);
    }
}
