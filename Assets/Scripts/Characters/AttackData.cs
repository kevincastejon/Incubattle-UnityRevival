using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackerType
{
    PLAYER,
    ENEMY
}
public enum HitType
{
    ATTACK,
    GROUNDPOUND,
    SPECIAL,
    CAN
}
public class AttackData
{
    private AttackerType _attackerType;
    private Collider2D _attackBox;
    private Collider2D _hitBox;
    private Vector2 _hitDirection;
    private HitType _hitType;
    private int _damages;

    public AttackerType AttackerType { get => _attackerType; set => _attackerType = value; }
    public Collider2D AttackBox { get => _attackBox; set => _attackBox = value; }
    public Collider2D HitBox { get => _hitBox; set => _hitBox = value; }
    public Vector2 HitDirection { get => _hitDirection; set => _hitDirection = value; }
    public HitType HitType { get => _hitType; set => _hitType = value; }
    public int Damages { get => _damages; set => _damages = value; }
}
