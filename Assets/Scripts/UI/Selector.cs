using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selector : MonoBehaviour
{
    [SerializeField] private Transform _selector;
    [SerializeField] private Transform[] _selections;
    [SerializeField] private UnityEvent<int> _onSelect;
    [SerializeField] private UnityEvent<int> _onDeselect;
    [SerializeField] private string _axisName = "Horizontal";
    [SerializeField] private string _buttonName = "Attack";
    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioClip _selectSound;
    [SerializeField] private bool _activationSync = true;

    private int _selection;
    private bool _selected;
    private bool _axisInUse;

    private AudioSource _audioSource;

    public int Selection { get => _selection; }
    public bool Selected { get => _selected; }
    private Inputs _inputs;
    private void Awake()
    {
        _inputs = new Inputs();
        _inputs.UI.Enable();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _selector.position = _selections[_selection].position;
    }

    private void Update()
    {
        if (_activationSync)
        {
            _selections[_selection].gameObject.SetActive(true);
        }
        if (_selected)
        {
            return;
        }
        float axisValue = _inputs.UI.Navigate.ReadValue<Vector2>().x;
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
        if (_inputs.UI.Submit.triggered)
        {
            _selected = true;
            _onSelect.Invoke(_selection);
            _audioSource.PlayOneShot(_selectSound);
        }
    }

    private void SwitchRight()
    {
        if (_activationSync)
        {
            _selections[_selection].gameObject.SetActive(false);
        }
        _selection = _selection + 1 == _selections.Length ? 0 : _selection + 1;
        _selector.position = _selections[_selection].position;
    }
    private void SwitchLeft()
    {
        if (_activationSync)
        {
            _selections[_selection].gameObject.SetActive(false);
        }
        _selection = _selection - 1 == -1 ? _selections.Length - 1 : _selection - 1;
        _selector.position = _selections[_selection].position;
    }
    public void Unselect()
    {
        _selected = false;
        _onDeselect.Invoke(_selection);
    }
}
