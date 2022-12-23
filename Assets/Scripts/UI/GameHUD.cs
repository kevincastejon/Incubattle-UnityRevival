using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lifesTextP1;
    [SerializeField] private Image _healthBarMaskP1;
    [SerializeField] private Image _staminaBarMaskP1;
    [SerializeField] private Image _staminaBarP1;
    [SerializeField] private Image _avatarP1;
    [SerializeField] private ColorAnimator _colorAnimatorP1;
    [SerializeField] private GameObject _specialLabelP1;
    [SerializeField] private IntVariable _lifesP1;
    [SerializeField] private IntVariable _healthPointsP1;
    [SerializeField] private IntVariable _staminaP1;
    [SerializeField] private Image _avatarP2;
    [SerializeField] private GameObject _groupP2;
    [SerializeField] private IntVariable _lifesP2;
    [SerializeField] private IntVariable _healthPointsP2;
    [SerializeField] private IntVariable _staminaP2;
    [SerializeField] private TextMeshProUGUI _lifesTextP2;
    [SerializeField] private Image _healthBarMaskP2;
    [SerializeField] private Image _staminaBarMaskP2;
    [SerializeField] private Image _staminaBarP2;
    [SerializeField] private ColorAnimator _colorAnimatorP2;
    [SerializeField] private GameObject _specialLabelP2;
    [SerializeField] private Sprite[] _avatars;
    [SerializeField] private List<GameObject> _playersPrefab;
    [SerializeField] private PrefabVariable _P1;
    [SerializeField] private PrefabVariable _P2;
    [SerializeField] private BoolVariable _isCoop;
    [SerializeField] private IntVariable _score;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _victoryMenu;
    [SerializeField] private IntVariable _maxHealthPoints;
    [SerializeField] private IntVariable _maxStamina;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private TextMeshProUGUI _victoryScoreText;
    [SerializeField] private TextMeshProUGUI _timeText;


    private void Awake()
    {
        _score.OnChange.AddListener(OnScoreChange);
        _lifesP1.OnChange.AddListener(OnLifeChange);
        _healthPointsP1.OnChange.AddListener(OnHealthChange);
        _staminaP1.OnChange.AddListener(OnStaminaChange);
        _avatarP1.sprite = _avatars[_playersPrefab.FindIndex(x => x == _P1.Value)];
        if (_isCoop.Value)
        {
            _groupP2.SetActive(true);
            _avatarP2.sprite = _avatars[_playersPrefab.FindIndex(x => x == _P2.Value)];
            _lifesP2.OnChange.AddListener(OnLifeChangeP2);
            _healthPointsP2.OnChange.AddListener(OnHealthChangeP2);
            _staminaP2.OnChange.AddListener(OnStaminaChangeP2);
        }
    }

    private void OnHealthChange(int value)
    {
        float lifePercent = (float)value / _maxHealthPoints.Value;
        float x = (lifePercent * 51) - 4;
        _healthBarMaskP1.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, _healthBarMaskP1.GetComponent<RectTransform>().anchoredPosition.y);
    }

    private void OnStaminaChange(int value)
    {
        float staminaPercent = (float)value / _maxStamina.Value;
        float x = (staminaPercent * 31) - 2.5f;
        _staminaBarMaskP1.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, _staminaBarMaskP1.GetComponent<RectTransform>().anchoredPosition.y);
        bool staminaFull = value == _maxStamina.Value;
        _colorAnimatorP1.enabled = staminaFull;
        _specialLabelP1.SetActive(staminaFull);
        if (staminaFull)
        {
            _staminaBarP1.color = Color.cyan;
        }
    }

    private void OnLifeChange(int value)
    {
        _lifesTextP1.text = value.ToString();
    }

    private void OnHealthChangeP2(int value)
    {
        float lifePercent = (float)value / _maxHealthPoints.Value;
        float x = (lifePercent * 51) - 4;
        _healthBarMaskP2.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, _healthBarMaskP2.GetComponent<RectTransform>().anchoredPosition.y);
    }

    private void OnStaminaChangeP2(int value)
    {
        float staminaPercent = (float)value / _maxStamina.Value;
        float x = (staminaPercent * 31) - 2.5f;
        _staminaBarMaskP2.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, _staminaBarMaskP2.GetComponent<RectTransform>().anchoredPosition.y);
        bool staminaFull = value == _maxStamina.Value;
        _colorAnimatorP2.enabled = staminaFull;
        _specialLabelP2.SetActive(staminaFull);
        if (staminaFull)
        {
            _staminaBarP2.color = Color.cyan;
        }
    }

    private void OnLifeChangeP2(int value)
    {
        _lifesTextP2.text = value.ToString();
    }

    private void OnScoreChange(int value)
    {
        _scoreText.text = value.ToString().PadLeft(6, '0');
    }

    public void ShowVictoryMenu()
    {
        _victoryScoreText.text = "SCORE :" + _score.Value.ToString().PadLeft(6, '0');
        string mins = Mathf.FloorToInt(_levelManager.ElapsedTime / 60f).ToString();
        string secs = (_levelManager.ElapsedTime - Mathf.FloorToInt(_levelManager.ElapsedTime / 60f) / 60f).ToString("F2");
        _timeText.text = "TIME : " + mins + " : " + secs;
        _victoryMenu.SetActive(true);
        PlayerInput[] _players = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput player in _players)
        {
            player.GetComponent<Animator>().SetTrigger("Victory");
            player.enabled = false;
        }
    }
}
