using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selector : MonoBehaviour
{
    [SerializeField] private Transform _selector;
    [SerializeField] private Transform[] _selections;
    [SerializeField] private UnityEvent _onSelect;
    [SerializeField] private UnityEvent _onDeselect;
    [SerializeField] private string _axisName = "Horizontal";
    [SerializeField] private string _buttonName = "Attack";
    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioClip _selectSound;

    private int _selection;
    private bool _selected;
    private bool _axisInUse;

    private AudioSource _audioSource;

    public int Selection { get => _selection; }
    public bool Selected { get => _selected; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _selector.position = _selections[_selection].position;
    }

    private void Update()
    {
        _selections[_selection].gameObject.SetActive(true);
        if (_selected)
        {
            return;
        }
        float axisValue = Input.GetAxisRaw(_axisName);
        if (axisValue != 0f)
        {
            if (!_axisInUse)
            {
                _axisInUse = true;
                if (axisValue > 0f)
                {
                    SwitchRight();
                }
                else
                {
                    SwitchLeft();
                }
                _audioSource.PlayOneShot(_moveSound);
            }
        }
        else
        {
            _axisInUse = false;
        }
        if (Input.GetButtonDown(_buttonName))
        {
            _selected = true;
            _onSelect.Invoke();
            _audioSource.PlayOneShot(_selectSound);
        }
    }

    private void SwitchRight()
    {
        _selections[_selection].gameObject.SetActive(false);
        _selection = _selection + 1 == _selections.Length ? 0 : _selection + 1;
        _selector.position = _selections[_selection].position;
    }
    private void SwitchLeft()
    {
        _selections[_selection].gameObject.SetActive(false);
        _selection = _selection - 1 == -1 ? _selections.Length - 1 : _selection - 1;
        _selector.position = _selections[_selection].position;
    }
    public void Unselect()
    {
        _selected = false;
        _onDeselect.Invoke();
    }
}
