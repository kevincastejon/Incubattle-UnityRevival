using System;
using UnityEngine;
public enum FollowType
{
    SNAP,
    LERP,
    SMOOTHDAMP
}

public enum UpdateType
{
    UPDATE,
    FIXED_UPDATE,
    LATE_UPDATE
}
public class CameraFollow : MonoBehaviour
{

    [SerializeField] private FollowType _followType = FollowType.SNAP;
    [SerializeField] private UpdateType _updateType = UpdateType.UPDATE;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _minDistance = 0.1f;
    [SerializeField] private BoxCollider2D _worldConstraint;

    //Lerp parameters
    [SerializeField] private float _speed = 2f;

    //SmoothDamp parameters
    [SerializeField] private float _smoothTime = 0.5f;
    [SerializeField] private float _maxSpeed = 10f;

    private Transform _target1;
    private Transform _target2;

    private Bounds _constraint;
    private Vector3 _velocity;

    public Bounds Constraint { get => _constraint; set => _constraint = value; }
    public Transform Target1 { get => _target1; set => _target1 = value; }
    public Transform Target2 { get => _target2; set => _target2 = value; }
    private Vector3 TargetPosition { get => _target2 ? (_target1.position + _target2.position) * 0.5f : _target1.position; }

    private void Start()
    {
        _constraint = new Bounds((Vector2)_worldConstraint.transform.position + _worldConstraint.offset, _worldConstraint.size);
    }

    private void Update()
    {
        if (_updateType == UpdateType.UPDATE)
        {
            DoUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (_updateType == UpdateType.FIXED_UPDATE)
        {
            DoUpdate();
        }
    }

    private void LateUpdate()
    {
        if (_updateType == UpdateType.LATE_UPDATE)
        {
            DoUpdate();
        }
    }

    private void DoUpdate()
    {
        if (!_target1)
        {
            return;
        }
        switch (_followType)
        {
            case FollowType.SNAP:
                Snap();
                break;
            case FollowType.LERP:
                Lerp();
                break;
            case FollowType.SMOOTHDAMP:
                SmoothDamp();
                break;
            default:
                break;
        }
    }

    private void Snap()
    {
        Vector3 newPosition = new Vector3(TargetPosition.x, 0f, _camera.transform.position.z);
        MoveConstraint(newPosition);
    }

    private void Lerp()
    {
        Vector3 currentPosition = new Vector3(_camera.transform.position.x, 0f, _camera.transform.position.z);
        Vector3 targetPosition = new Vector3(TargetPosition.x, 0f, _camera.transform.position.z);
        if (Vector2.Distance(currentPosition, targetPosition) < _minDistance)
        {
            return;
        }
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, _speed * Time.deltaTime);
        MoveConstraint(newPosition);
    }

    private void SmoothDamp()
    {
        Vector3 currentPosition = new Vector3(_camera.transform.position.x, 0f, _camera.transform.position.z);
        Vector3 targetPosition = new Vector3(TargetPosition.x, 0f, _camera.transform.position.z);
        if (Vector2.Distance(currentPosition, targetPosition) < _minDistance)
        {
            return;
        }
        Vector3 newPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref _velocity, _smoothTime, _maxSpeed);
        MoveConstraint(newPosition);
    }

    private void MoveConstraint(Vector3 newPosition)
    {
        float limitLeft = _constraint.min.x;
        float limitRight = _constraint.max.x;

        Bounds cameraBounds = GetCameraBoundsFromPosition(newPosition);

        float cameraLeft = cameraBounds.min.x;
        float cameraRight = cameraBounds.max.x;

        bool isIntoLimits = cameraLeft > limitLeft && cameraRight < limitRight;
        bool isMovingTowardLimit = false;
        if (!isIntoLimits)
        {
            bool leftSide = cameraLeft < limitLeft;
            if (leftSide)
            {
                isMovingTowardLimit = transform.position.x < newPosition.x;
            }
            else
            {
                isMovingTowardLimit = transform.position.x > newPosition.x;
            }
        }

        if (isIntoLimits || isMovingTowardLimit)
        {
            _camera.transform.position = newPosition;
        }
    }

    public Bounds GetCameraBoundsFromPosition(Vector2 position)
    {
        float cameraHeight = _camera.orthographicSize * 2;
        Bounds bounds = new Bounds(position, new Vector3(cameraHeight * _camera.aspect, cameraHeight, 0));
        return bounds;
    }
}