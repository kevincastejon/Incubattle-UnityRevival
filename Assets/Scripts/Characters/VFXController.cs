using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] private Animator _groundPound;
    [SerializeField] private Animator _special;
    [SerializeField] private Animator _jump;
    [SerializeField] private Animator _runLeft;
    [SerializeField] private Animator _runRight;
    [SerializeField] private Animator _land;
    [SerializeField] private Animator _throw;
    [SerializeField] private GameObject _punchPrefab;
    [SerializeField] private Transform _punchZAxisSorterTranform;

    private Vector3 _groundPoundPosBackup;
    private Vector3 _specialPosBackup;
    private Vector3 _jumpPosBackup;
    private Vector3 _runLeftPosBackup;
    private Vector3 _runRightPosBackup;
    private Vector3 _landPosBackup;
    private Vector3 _throwPosBackup;

    private void Start()
    {
        if (_groundPound)
        {
            _groundPoundPosBackup = _groundPound.transform.localPosition;
        }
        if (_special)
        {
            _specialPosBackup = _special.transform.localPosition;
        }
        if (_jump)
        {
            _jumpPosBackup = _jump.transform.localPosition;
        }
        if (_runLeft)
        {
            _runLeftPosBackup = _runLeft.transform.localPosition;
        }
        if (_runRight)
        {
            _runRightPosBackup = _runRight.transform.localPosition;
        }
        if (_land)
        {
            _landPosBackup = _land.transform.localPosition;
        }
        if (_throw)
        {
            _throwPosBackup = _throw.transform.localPosition;
        }
    }

    public void TriggerSlam()
    {
        _groundPound.transform.parent = transform.parent;
        _groundPound.transform.localPosition = _groundPoundPosBackup;
        _groundPound.SetTrigger("Show");
        _groundPound.transform.parent = null;
    }
    public void TriggerSpecial()
    {
        //_special.transform.parent = transform.parent;
        _special.transform.localPosition = _specialPosBackup;
        _special.SetTrigger("Show");
        //_special.transform.parent = null;
    }
    public void TriggerJump()
    {
        _jump.transform.parent = transform.parent;
        _jump.transform.localPosition = _jumpPosBackup;
        _jump.SetTrigger("Show");
        _jump.transform.parent = null;
    }
    public void TriggerRunLeft()
    {
        _runLeft.transform.parent = transform.parent;
        _runLeft.transform.localPosition = _runLeftPosBackup;
        _runLeft.SetTrigger("Show");
        _runLeft.transform.parent = null;
    }
    public void TriggerRunRight()
    {
        _runRight.transform.parent = transform.parent;
        _runRight.transform.localPosition = _runRightPosBackup;
        _runRight.transform.right = Vector2.left;
        _runRight.SetTrigger("Show");
        _runRight.transform.parent = null;
    }
    public void TriggerLand()
    {
        _land.transform.parent = transform.parent;
        _land.transform.localPosition = _landPosBackup;
        _land.SetTrigger("Show");
        _land.transform.parent = null;
    }
    public void TriggerThrow()
    {
        _throw.transform.parent = transform.parent;
        _throw.transform.localPosition = _throwPosBackup;
        _throw.SetTrigger("Show");
        _throw.transform.parent = null;
    }
    public void TriggerPunch(Vector3 position)
    {
        Instantiate(_punchPrefab, position, Quaternion.identity).GetComponent<ZAxisSorter>().MainTransform = _punchZAxisSorterTranform;
    }
}
