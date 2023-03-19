using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInput : MonoBehaviour
{
    [SerializeField] private string _horizontalAxisName = "Horizontal";
    [SerializeField] private string _verticalAxisName = "Vertical";
    [SerializeField] private string _attackButtonName = "Attack";
    [SerializeField] private string _jumpButtonName = "Jump";
    [SerializeField] private string _sprintButtonName = "Sprint";
    [SerializeField] private string _specialButtonName = "Special";
    // Axe deplacement
    private Vector2 _movement;

    // Bouton attaque
    private bool _attack;
    private bool _attackDown;

    // Bouton saut
    private bool _jump;
    private bool _jumpDown;

    // Bouton sprint
    private bool _sprint;
    private bool _sprintDown;

    // Bouton sprint
    private bool _special;
    private bool _specialDown;

    public Vector2 Movement { get => _movement; }
    public Vector2 NormalizedMovement { get => _movement.normalized; }
    public Vector2 ClampedMovement { get => Vector2.ClampMagnitude(_movement, 1f); }
    public bool HasMovement { get => _movement != Vector2.zero; }

    public bool Attack { get => _attack; set => _attack = value; }
    public bool AttackDown { get => _attackDown; set => _attackDown = value; }

    public bool Jump { get => _jump; set => _jump = value; }
    public bool JumpDown { get => _jumpDown; set => _jumpDown = value; }

    public bool Sprint { get => _sprint; set => _sprint = value; }
    public bool SprintDown { get => _sprintDown; set => _sprintDown = value; }

    public bool Special { get => _special; set => _special = value; }
    public bool SpecialDown { get => _specialDown; set => _specialDown = value; }

    public string HorizontalAxisName { get => _horizontalAxisName; set => _horizontalAxisName = value; }
    public string VerticalAxisName { get => _verticalAxisName; set => _verticalAxisName = value; }
    public string AttackButtonName { get => _attackButtonName; set => _attackButtonName = value; }
    public string JumpButtonName { get => _jumpButtonName; set => _jumpButtonName = value; }
    public string SprintButtonName { get => _sprintButtonName; set => _sprintButtonName = value; }
    public string SpecialButtonName { get => _specialButtonName; set => _specialButtonName = value; }

    private bool _axisSprintInUse;

    private Inputs _inputs;

    private void Awake()
    {
        _inputs = new Inputs();
        _inputs.Player.Enable();
    }

    private void Update()
    {
        //// On stocke la valeur de l'axe de déplacement
        _movement = _inputs.Player.Move.ReadValue<Vector2>();

        //// On stocke la valeur de l'input Attack
        _attack = _inputs.Player.Attack.ReadValue<float>()>0f;
        //// On stocke la valeur 'down' de l'input Attack
        _attackDown = _inputs.Player.Attack.triggered;

        // On stocke la valeur de l'input Jump
        _jump = _inputs.Player.Jump.ReadValue<float>() > 0f;
        // On stocke la valeur 'down' de l'input Jump
        _jumpDown = _inputs.Player.Jump.triggered;

        // On stocke la valeur de l'input Sprint
        _sprint = _inputs.Player.Sprint.ReadValue<float>() > 0f;
        // On stocke la valeur 'down' de l'input Sprint
        _sprintDown = _inputs.Player.Sprint.triggered;

        // On stocke la valeur de l'input Special
        _special = _inputs.Player.Special.ReadValue<float>() > 0f;
        // On stocke la valeur 'down' de l'input Special
        _specialDown = _inputs.Player.Special.triggered;
    }
}