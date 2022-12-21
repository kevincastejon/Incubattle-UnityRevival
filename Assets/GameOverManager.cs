using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private IntVariable _currentLevel;
    [SerializeField] private string[] _levels;

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
