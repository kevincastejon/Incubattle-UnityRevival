using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private LayerMask _attackBoxLayer;
    private BoxCollider2D _collider;
    private AttackData _attackData;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    public bool DetectHit()
    {
        Collider2D collider = Physics2D.OverlapArea(_collider.bounds.min, _collider.bounds.max, _attackBoxLayer);
        if (collider != null)
        {
            PlayerController playerController = collider.GetComponentInParent<PlayerController>();
            _attackData = new AttackData();
            _attackData.AttackerType = playerController != null ? AttackerType.PLAYER : AttackerType.ENEMY;
            _attackData.AttackBox = collider;
            _attackData.HitBox = _collider;
            if (collider.CompareTag("AttackBox"))
            {
                _attackData.HitType = HitType.ATTACK;
                _attackData.Damages = 1;
            }
            else if (collider.CompareTag("CanBox"))
            {
                _attackData.HitType = HitType.CAN;
                _attackData.Damages = 3;
            }
            else if (collider.CompareTag("GroundPoundBox"))
            {
                _attackData.HitType = HitType.GROUNDPOUND;
                _attackData.Damages = 1;
            }
            else if (collider.CompareTag("SpecialBox"))
            {
                _attackData.HitType = HitType.SPECIAL;
                _attackData.Damages = 10;
            }
            _attackData.HitDirection = collider.transform.position.x < _collider.transform.position.x ? Vector2.right : Vector2.left;
            return true;
        }
        else
        {
            _attackData = null;
            return false;
        }
    }
    public AttackData GetHit()
    {
        return _attackData;
    }
}
