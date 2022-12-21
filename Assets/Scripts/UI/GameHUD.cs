using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _lifesText;
    [SerializeField] private Image _healthBarMask;
    [SerializeField] private Image _staminaBarMask;
    [SerializeField] private Image _staminaBar;
    [SerializeField] private Animator _victoryMenu;
    [SerializeField] private Animator _gameOverMenu;
    [SerializeField] private ColorAnimator _colorAnimator;
    [SerializeField] private GameObject _specialLabel;

    [SerializeField] private IntVariable _score;
    [SerializeField] private IntVariable _lifes;
    [SerializeField] private IntVariable _healthPoints;
    [SerializeField] private IntVariable _maxHealthPoints;
    [SerializeField] private IntVariable _stamina;
    [SerializeField] private IntVariable _maxStamina;


    private void Awake()
    {
        _score.OnChange.AddListener(OnScoreChange);
        _lifes.OnChange.AddListener(OnLifeChange);
        _healthPoints.OnChange.AddListener(OnHealthChange);
        _stamina.OnChange.AddListener(OnStaminaChange);
    }

    private void OnHealthChange(int value)
    {
        float lifePercent = (float)value / _maxHealthPoints.Value;
        float x = (lifePercent * 51) - 4;
        _healthBarMask.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, _healthBarMask.GetComponent<RectTransform>().anchoredPosition.y);
    }

    private void OnStaminaChange(int value)
    {
        float staminaPercent = (float)value / _maxStamina.Value;
        float x = (staminaPercent * 31) - 2.5f;
        _staminaBarMask.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, _staminaBarMask.GetComponent<RectTransform>().anchoredPosition.y);
        bool staminaFull = value == _maxStamina.Value;
        _colorAnimator.enabled = staminaFull;
        _specialLabel.SetActive(staminaFull);
        if (staminaFull)
        {
            _staminaBar.color = Color.cyan;
        }
    }

    private void OnLifeChange(int value)
    {
        _lifesText.text = value.ToString();
    }

    private void OnScoreChange(int value)
    {
        _scoreText.text = value.ToString().PadLeft(6, '0');
    }

    public void ShowVictoryMenu()
    {
        _victoryMenu.enabled = true;
    }
}
