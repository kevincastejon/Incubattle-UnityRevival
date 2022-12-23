using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private BoolVariable _isCoop;
    [SerializeField] private PrefabVariable _p1;
    [SerializeField] private PrefabVariable _p2;
    [SerializeField] private List<GameObject> _playersPrefabs;
    [SerializeField] private GameObject[] _playersDeathPoses;
    [SerializeField] private Transform _uniquePos;
    [SerializeField] private Transform _coopPosP1;
    [SerializeField] private Transform _coopPosP2;
    [SerializeField] private IntVariable _currentLevel;
    [SerializeField] private string[] _levels;

    private void Start()
    {
        if (!_isCoop.Value)
        {
            GameObject deathPose = _playersDeathPoses[_playersPrefabs.IndexOf(_p1.Value)];
            deathPose.SetActive(true);
            deathPose.transform.position = _uniquePos.position;
        }
        else
        {
            GameObject deathPoseP1 = _playersDeathPoses[_playersPrefabs.IndexOf(_p1.Value)];
            GameObject deathPoseP2 = _playersDeathPoses[_playersPrefabs.IndexOf(_p2.Value)];
            deathPoseP1.SetActive(true);
            deathPoseP1.transform.position = _coopPosP1.position;
            deathPoseP2.SetActive(true);
            deathPoseP2.transform.position = _coopPosP2.position;
        }
    }

    public void SelectMenuItem(int itemId)
    {
        if (itemId == 0)
        {
            SceneManager.LoadScene(_levels[_currentLevel.Value]);
        }
        else if (itemId == 1)
        {
            SceneManager.LoadScene("Intro");
        }
    }
}
