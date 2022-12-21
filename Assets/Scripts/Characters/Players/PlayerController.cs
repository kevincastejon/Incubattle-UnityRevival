using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    //Reference settings
    [Header("Références Settings")]
    [SerializeField]
    [Tooltip("Référence à l'enfant contenant le corps du personnage")]
    private Transform _body;
    [SerializeField]
    [Tooltip("Référence à l'AttackBox")]
    private GameObject _attackBox;
    [SerializeField]
    [Tooltip("Référence à la HitBox")]
    private Collider2D _hitBox;
    [SerializeField]
    [Tooltip("Référence à la GroundPoundBox")]
    private GameObject _groundPoundBox;
    [SerializeField]
    [Tooltip("Référence à la SpecialBox")]
    private BoxCollider2D _specialBox;
    [SerializeField]
    [Tooltip("Référence à la position de la canette portée")]
    private Transform _holdCanPosition;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Référence au ScriptableObject PlayerHealth contenant le nombre actuel de points de vie")]
    private IntVariable _healthPoints;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Référence au ScriptableObject PlayerMaxHealth contenant le nombre max de points de vie")]
    private IntVariable _maxHealthPoints;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Référence au ScriptableObject PlayerStamina contenant le nombre actuel de stamina")]
    private IntVariable _stamina;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Référence au ScriptableObject PlayerMaxStamina contenant le nombre max de stamina")]
    private IntVariable _maxStamina;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Nombre de vies")]
    private IntVariable _lifes;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Nombre de vies de départ")]
    private IntVariable _startingLifes;

    //Health settings
    [Header("Health Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Delai avant résurrection")]
    private float _resurrectionDuration = 3f;
    [SerializeField]
    [Tooltip("Delai avant disparition du cadavre")]
    private float _vanishingDuration = 3f;

    //Speed settings
    [Header("Speed Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Vitesse de marche")]
    private float _speed = 2.5f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Vitesse de course")]
    private float _sprintSpeed = 5f;

    //Jump settings
    [Header("Jump Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du saut")]
    private float _jumpDuration = 1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Hauteur du saut")]
    private float _jumpHeight = 1f;
    [SerializeField]
    [Tooltip("Easing du saut")]
    private AnimationCurve _jumpEasing;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée de l'atterissage")]
    private float _landingDuration = 0.2f;

    //Attack settings
    [Header("Attack Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée de l'attaque")]
    private float _attackDuration = 0.1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du freeze de l'attaque")]
    private float _attackFreezeDuration = 0.01f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Délai minimum entre deux coups pour les combos")]
    private float _attackComboDuration = 0.5f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Vitesse de la plongée")]
    private float _diveSpeed = 2f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du groundpound")]
    private float _groundPoundDuration = 1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du special")]
    private float _specialDuration = 2.5f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du ramassage de cannette")]
    private float _pickingDuration = 0.25f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du lancer de cannette")]
    private float _throwingDuration = 0.25f;

    //KnockBack settings
    [Header("Knockback Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du knockback")]
    private float _knockBackDuration = 0.5f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Puissance horizontale du knockback")]
    private float _knockBackHorizontalForce = 1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Puissance verticale du knockback")]
    private float _knockBackVerticalForce = 0.5f;
    [SerializeField]
    [Tooltip("Easing horizontal du knockback")]
    private AnimationCurve _knockBackHorizontalEasing;
    [SerializeField]
    [Tooltip("Easing vertical du knockback")]
    private AnimationCurve _knockBackVerticalEasing;

    [Header("GroundPound Knockback Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du GroundPound knockback")]
    private float _groundPoundKnockBackDuration = 0.5f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Puissance horizontale du GroundPound knockback")]
    private float _groundPoundKnockBackHorizontalForce = 1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Puissance verticale du GroundPound knockback")]
    private float _groundPoundKnockBackVerticalForce = 0.5f;
    [SerializeField]
    [Tooltip("Easing horizontal du GroundPound knockback")]
    private AnimationCurve _groundPoundKnockBackHorizontalEasing;
    [SerializeField]
    [Tooltip("Easing vertical du GroundPound knockback")]
    private AnimationCurve _groundPoundKnockBackVerticalEasing;

    [Header("Can Knockback Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du Can knockback")]
    private float _canKnockBackDuration = 0.5f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Puissance horizontale du Can knockback")]
    private float _canKnockBackHorizontalForce = 1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Puissance verticale du Can knockback")]
    private float _canKnockBackVerticalForce = 0.5f;
    [SerializeField]
    [Tooltip("Easing horizontal du Can knockback")]
    private AnimationCurve _canKnockBackHorizontalEasing;
    [SerializeField]
    [Tooltip("Easing vertical du Can knockback")]
    private AnimationCurve _canKnockBackVerticalEasing;

    // Références de composants
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private PlayerInput _playerInput;
    private Camera _camera;
    private VFXController _vfxController;
    private SFXController _sfxController;

    // Variables de timer
    private float _jumpEndTime;                                 // heure de fin de saut
    private float _groundPoundEndTime;                          // heure de fin de groundpound
    private float _attackEndTime;                               // heure de fin d'attaque coup de poing
    private float _pickingEndTime;                              // heure de fin de ramassage de canette
    private float _throwingEndTime;                             // heure de fin de lancer de canette
    private float _specialEndTime;                              // heure de fin de special
    private float _attackFreezeEndTime;                         // heure de fin du freeze d'attaque
    private float _attackComboEndTime;                          // heure de fin du combo d'attaque
    private float _knockBackEndTime;                            // heure de fin de knockBack
    private float _resurrectionEndTime;                         // heure de fin de resurrection
    private float _vanishingEndTime;                            // heure de fin de disparition de cadavre
    private float _landingEndTime;                              // heure de fin d'atterissage

    // Variables d'attaques
    private Vector2 _lastMoveDirection = Vector2.right;            // direction du dernier déplacement
    private Vector2 _knockBackDirection;                                            // direction du knockBack en cours
    private HitType _hitType;                                                       // type du knockBack en cours
    private int _combo;                                                             // combo actuel
    private Throwable _throwable;                                                   // reference à la cannette portée

    // Propriétés
    public int HealthPoints { get => _healthPoints.Value; }
    public int Lifes { get => _lifes.Value; }
    public int Stamina { get => _stamina.Value; set => _stamina.Value = Mathf.Min(value, _maxStamina.Value); }
    public int MaxStamina { get => _maxStamina.Value; }
    public bool IsJumpEnded { get => Time.time >= _jumpEndTime; }
    public bool IsDiveEnded { get => _body.localPosition.y < 0f; }
    public bool IsGroundPoundEnded { get => Time.time >= _groundPoundEndTime; }
    public bool IsSpecialEnded { get => Time.time >= _specialEndTime; }
    public bool IsLandingEnded { get => Time.time >= _landingEndTime; }
    public bool IsAttackEnded { get => Time.time >= _attackEndTime; }
    public bool IsPickingEnded { get => Time.time >= _pickingEndTime; }
    public bool IsThrowingEnded { get => Time.time >= _throwingEndTime; }
    public bool IsKnockBackEnded { get => Time.time >= _knockBackEndTime; }
    public bool IsAttackFreezeEnded { get => Time.time >= _attackFreezeEndTime; }
    public bool IsResurrectionEnded { get => Time.time >= _resurrectionEndTime; }
    public Collider2D HitBox { get => _hitBox; }
    public IntVariable HealthPointsSO { set => _healthPoints = value; }
    public IntVariable StaminaSO { set => _stamina = value; }
    public IntVariable LifesSO { set => _lifes = value; }

    private void Awake()
    {
        // On récupère les composants
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _vfxController = GetComponentInChildren<VFXController>();
        _sfxController = GetComponentInChildren<SFXController>();

        _camera = Camera.main;
    }
    private void Start()
    {
        // On définit les points de vie au maximum
        _healthPoints.Value = _maxHealthPoints.Value;
        // On remet les vies du personnage au max
        _lifes.Value = _startingLifes.Value;
        // On remet la stamina à 0
        _stamina.Value = 10;
    }

    public void DoTurnCharacter()
    {
        // On tourne le personnage dans le sens de son déplacement
        _transform.right = _playerInput.ClampedMovement.x < 0 ? Vector2.left : Vector2.right;
    }

    public void DoIdle()
    {
        // On fige le personnage
        _rigidbody.velocity = Vector2.zero;
    }
    public void DoWalk()
    {
        // On détermine la vitesse selon l'état du personnage
        float speed = _speed;
        // On applique le déplacement
        _rigidbody.velocity = _playerInput.ClampedMovement * speed;
        // On stocke la dernière direction empruntée
        _lastMoveDirection = _rigidbody.velocity.x < 0 ? Vector2.left : Vector2.right;
    }
    public void DoSprint()
    {
        // On détermine la vitesse selon l'état du personnage
        float speed = _sprintSpeed;
        // On applique le déplacement
        _rigidbody.velocity = _playerInput.ClampedMovement * speed;
        // On stocke la dernière direction empruntée
        _lastMoveDirection = _rigidbody.velocity.x < 0 ? Vector2.left : Vector2.right;
    }

    public void StartJump()
    {
        // On modifie le paramètre de l'animator
        _animator.SetFloat("JumpProgress", 0f);
        // On définit l'heure où le saut sera terminé
        _jumpEndTime = Time.time + _jumpDuration;
    }
    public void DoJump()
    {
        // On calcule la progression du saut
        float jumpProgress = (_jumpDuration - (_jumpEndTime - Time.time)) / _jumpDuration;
        // On récupère de la courbe la valeur de hauteur normalisée
        float jumpNormalizedHeight = _jumpEasing.Evaluate(jumpProgress);
        // On applique le mouvement vertical du sprite grâce à la courbe, la progression et la force
        _body.localPosition = new Vector3(0, Mathf.Max(0f, jumpNormalizedHeight * _jumpHeight), _body.localPosition.z);
        // On définit le paramètre d'animator concernant la progression du saut
        _animator.SetFloat("JumpProgress", jumpProgress);
    }
    public void EndJump()
    {
        // On définit le paramètre d'animator concernant la progression du saut à 0
        _animator.SetFloat("JumpProgress", 0f);
    }

    public void DoDiving()
    {
        // On fait plonger le personnage
        _body.Translate(Vector2.down * _diveSpeed * Time.deltaTime, Space.Self);
        // On calcule le pourcentage de hauteur du personnage par rapport à la hauteur maximum du saut
        float percent = 1 - (_body.localPosition.y / _jumpHeight);
    }
    public void EndDiving()
    {
        // On snap la position Y du sprite à 0
        _body.localPosition = new Vector3(0, 0, _body.localPosition.z);
    }

    public void StartLand()
    {
        // On définit l'heure où l'atterrissage sera terminé
        _landingEndTime = Time.time + _landingDuration;
    }
    public void DoLand()
    {
        // On calcule la progression de l'atterrissage
        float landProgress = (_landingDuration - (_landingEndTime - Time.time)) / _landingDuration;
        // On définit le paramètre d'animator concernant la progression de l'atterrissage
        _animator.SetFloat("LandProgress", landProgress);
    }
    public void EndLand()
    {
        // On définit l'atterissage comme étant terminé
        _landingEndTime = Time.time;
        // On définit le paramètre d'animator concernant la progression de l'atterrissage
        _animator.SetFloat("LandProgress", 0f);
    }

    public void StartGroundPounding()
    {
        // On active la GroundPoundBox
        _groundPoundBox.SetActive(true);
        // On définit l'heure où le groundpound sera terminé
        _groundPoundEndTime = Time.time + _groundPoundDuration;
    }
    public void DoGroundPounding()
    {
        // On calcule la progression du groundpound
        float groundPoundProgress = (_landingDuration - (_landingEndTime - Time.time)) / _landingDuration;
        // On définit le paramètre d'animator concernant la progression du groundpound
        _animator.SetFloat("GroundPoundingProgress", groundPoundProgress);
    }
    public void EndGroundPounding()
    {
        // On active la GroundPoundBox
        _groundPoundBox.SetActive(false);
        // On définit le groundpound comme étant terminé
        _groundPoundEndTime = Time.time;
        // On définit le paramètre d'animator concernant la progression du groundpound
        _animator.SetFloat("GroundPoundingProgress", 0f);
    }

    public void StartSpecial()
    {
        // On définit l'heure où le special sera terminé
        _specialEndTime = Time.time + _specialDuration;
        // On définit le paramètre d'animator concernant la progression du special
        //_animator.SetFloat("SpecialProgress", 0f);
    }
    public void StartSpecialAttack()
    {
        if (Mathf.Approximately(0f, _animator.GetFloat("SpecialProgress")))
        {
            return;
        }

        // On active la SpecialBox
        //_specialBox.gameObject.SetActive(true);
        // On démarre le VFX
        _vfxController.TriggerSpecial();
        // On démarre le SFX
        _sfxController.TriggerSpecialBoom();
        // On synchronise la taille et la position de la SpecialBox avec la caméra pour qu'elle couvre tout l'écran
        //_specialBox.offset = transform.InverseTransformPoint(_camera.transform.position);
        //_specialBox.size = new Vector2((_camera.orthographicSize * 2) * _camera.aspect, _camera.orthographicSize * 2);
    }
    public void DoSpecial()
    {
        // On calcule la progression du special
        float specialProgress = (_specialDuration - (_specialEndTime - Time.time)) / _specialDuration;
        // On définit le paramètre d'animator concernant la progression du special
        _animator.SetFloat("SpecialProgress", Mathf.Clamp01(specialProgress));
        // On synchronise la taille et la position de la SpecialBox avec la caméra pour qu'elle couvre tout l'écran
        //_specialBox.offset = transform.InverseTransformPoint(_camera.transform.position);
        //_specialBox.size = new Vector2((_camera.orthographicSize * 2) * _camera.aspect, _camera.orthographicSize * 2);
    }
    public void EndSpecial()
    {
        // On définit le paramètre d'animator concernant la progression du special
        _animator.SetFloat("SpecialProgress", 0f);
        // On désactive la SpecialBox
        //_specialBox.gameObject.SetActive(false);
        // On réinitialise la stamina
        //_stamina.Value = 0;
    }

    public void StartAttack()
    {
        // On active l'AttackBox
        _attackBox.SetActive(true);
        // On détermine l'heure où l'attaque prendra fin
        _attackEndTime = Time.time + _attackDuration;
        // On détermine l'heure où le freeze d'attaque prendra fin
        _attackFreezeEndTime = Time.time + _attackFreezeDuration;

        // Si la dernière attaque a eu lieu après le délai imparti pour déclencher un combo
        if (Time.time > _attackComboEndTime)
        {
            // On réinitialise le compteur de combo
            _combo = 0;
        }
        // Sinon
        else
        {
            // On incrémente le compteur de combo
            _combo = _combo + 1 == 4 ? 0 : _combo + 1;
        }

        // On determine l'heure avant laquelle doit avoir lieu la prochaine attaque pour déclencher un combo
        _attackComboEndTime = Time.time + _attackComboDuration;
        // On envoie le numéro de combo à l'Animator
        _animator.SetInteger("AttackCombo", _combo);
    }
    public void DoAttack()
    {
        // On calcule la progression de l'attaque
        float attackProgress = (_attackDuration - (_attackEndTime - Time.time)) / _attackDuration;
        // On définit le paramètre d'animator concernant la progression de l'attaque
        _animator.SetFloat("AttackProgress", attackProgress);
    }
    public void EndAttack()
    {
        // On désactive l'AttackBox
        _attackBox.SetActive(false);
        // On détermine l'heure de fin de cooldown d'attaque
        _attackFreezeEndTime = Time.time + _attackFreezeDuration;
        // On définit le paramètre d'animator concernant la progression de l'attaque à 0
        _animator.SetFloat("AttackProgress", 0f);
    }

    public void StartPicking(Throwable throwable)
    {
        //
        _throwable = throwable;
        //
        _throwable.Pick(_transform, _holdCanPosition);
        // On détermine le ramassage prendra fin
        _pickingEndTime = Time.time + _pickingDuration;
        // On définit le paramètre d'animator concernant la progression du ramassage à 0
        _animator.SetFloat("PickingProgress", 0f);
    }
    public void DoPicking()
    {
        // On calcule la progression du ramassage
        float pickingProgress = (_pickingDuration - (_pickingEndTime - Time.time)) / _pickingDuration;
        // On définit le paramètre d'animator concernant la progression du ramassage
        _animator.SetFloat("PickingProgress", pickingProgress);
    }

    public void StartThrowing()
    {
        // On détermine quand le lancer prendra fin
        _throwingEndTime = Time.time + _throwingDuration;
        // On définit le paramètre d'animator concernant la progression du lancer à 0
        _animator.SetFloat("ThrowingProgress", 0f);
        // On déclenche le lancer
        _throwable.Throw(_lastMoveDirection, _attackBox.layer);
    }
    public void DoThrowing()
    {
        // On calcule la progression du lancer
        float throwingProgress = (_throwingDuration - (_throwingEndTime - Time.time)) / _throwingDuration;
        // On définit le paramètre d'animator concernant la progression du lancer
        _animator.SetFloat("ThrowingProgress", throwingProgress);
    }

    public void StartResurrection()
    {
        // On détermine l'heure où la resurection sera terminé
        _resurrectionEndTime = Time.time + _resurrectionDuration;
    }
    public void EndResurrection()
    {
        // On retire une vie
        _lifes.Value--;
        // On remet les points de vie au maximum
        _healthPoints.Value = _maxHealthPoints.Value;
    }

    public void StartVanishing()
    {
        // On détermine l'heure où la resurection sera terminé
        _vanishingEndTime = Time.time + _vanishingDuration;
    }
    public void DoVanishing()
    {
        // Si l'heure de disparition du cadavre est atteinte
        if (Time.time > _vanishingEndTime)
        {
            // On détruit l'ennemi
            Destroy(gameObject);
        }
    }

    public void StartKnockBack(AttackData attackData)
    {
        // On stocke le type d'attaque reçue
        _hitType = attackData.HitType;
        // On stocke la direction du knockback en cours
        _knockBackDirection = attackData.HitDirection;
        // On détermine l'heure où le knockback sera terminé et on retranche des points de vie en fonction du type d'attaque
        // On retranche des points de vie au personnage et on détermine l'heure où le knockback sera terminé
        switch (_hitType)
        {
            case HitType.ATTACK:
                _knockBackEndTime = Time.time + (_knockBackDuration);
                _healthPoints.Value = Mathf.Max(0, _healthPoints.Value - 1);
                break;
            case HitType.GROUNDPOUND:
                _knockBackEndTime = Time.time + (_groundPoundKnockBackDuration);
                _healthPoints.Value = Mathf.Max(0, _healthPoints.Value - 4);
                break;
            case HitType.CAN:
                _knockBackEndTime = Time.time + (_canKnockBackDuration);
                _healthPoints.Value = Mathf.Max(0, _healthPoints.Value - 2);
                break;
            default:
                break;
        }

        // Si le nombre de points de vie est tombé à 0
        if (_healthPoints.Value <= 0)
        {
            // On lance le timer de resurrection
            _resurrectionEndTime = Time.time + _resurrectionDuration;
        }
    }
    public void DoVerticalKnockback()
    {
        switch (_hitType)
        {
            case HitType.ATTACK:
                DoVerticalAttackKnockBack();
                break;
            case HitType.GROUNDPOUND:
                DoVerticalGroundPoundKnockBack();
                break;
            case HitType.CAN:
                DoVerticalCanKnockBack();
                break;
            default:
                break;
        }
    }
    private void DoVerticalAttackKnockBack()
    {
        // On calcule la progression du knockback
        float knockBackFreezeProgress = (_knockBackDuration - (_knockBackEndTime - Time.time)) / _knockBackDuration;
        // On calcule le easing grâce à la courbe, la progression et la force
        float curvedAmount = _knockBackVerticalEasing.Evaluate(knockBackFreezeProgress) * _knockBackVerticalForce;
        // On calcule la nouvelle position Y du sprite
        float newYPos = curvedAmount * _knockBackVerticalForce;
        // On applique le mouvement
        _body.localPosition = new Vector3(0, newYPos, _body.localPosition.z);
    }
    private void DoVerticalGroundPoundKnockBack()
    {
        // On calcule la progression du knockback
        float knockBackFreezeProgress = (_groundPoundKnockBackDuration - (_knockBackEndTime - Time.time)) / _groundPoundKnockBackDuration;
        // On calcule le easing grâce à la courbe, la progression et la force
        float curvedAmount = _groundPoundKnockBackVerticalEasing.Evaluate(knockBackFreezeProgress) * _groundPoundKnockBackVerticalForce;
        // On calcule la nouvelle position Y du sprite
        float newYPos = curvedAmount * _groundPoundKnockBackVerticalForce;
        // On applique le mouvement
        _body.localPosition = new Vector3(0, newYPos, _body.localPosition.z);
    }
    private void DoVerticalCanKnockBack()
    {
        // On calcule la progression du knockback
        float knockBackFreezeProgress = (_canKnockBackDuration - (_knockBackEndTime - Time.time)) / _canKnockBackDuration;
        // On calcule le easing grâce à la courbe, la progression et la force
        float curvedAmount = _canKnockBackVerticalEasing.Evaluate(knockBackFreezeProgress) * _canKnockBackVerticalForce;
        // On calcule la nouvelle position Y du sprite
        float newYPos = curvedAmount * _canKnockBackVerticalForce;
        // On applique le mouvement
        _body.localPosition = new Vector3(0, newYPos, _body.localPosition.z);
    }
    public void DoHorizontalKnockBack()
    {
        switch (_hitType)
        {
            case HitType.ATTACK:
                DoHorizontalAttackKnockBack();
                break;
            case HitType.GROUNDPOUND:
                DoHorizontalGroundPoundKnockBack();
                break;
            case HitType.CAN:
                DoHorizontalCanKnockBack();
                break;
            default:
                break;
        }
    }
    private void DoHorizontalAttackKnockBack()
    {
        // On calcule la progression du knockback
        float knockBackFreezeProgress = (_knockBackDuration - (_knockBackEndTime - Time.time)) / _knockBackDuration;
        // On calcule le easing grâce à la courbe, la progression et la force
        float curvedAmount = _knockBackHorizontalEasing.Evaluate(knockBackFreezeProgress) * _knockBackHorizontalForce;
        // On crée un vecteur de mouvement grâce à la direction et au easing
        Vector2 motion = _knockBackDirection * curvedAmount;
        // On applique le mouvement
        _rigidbody.velocity = motion;
    }
    private void DoHorizontalGroundPoundKnockBack()
    {
        // On calcule la progression du knockback
        float knockBackFreezeProgress = (_groundPoundKnockBackDuration - (_knockBackEndTime - Time.time)) / _groundPoundKnockBackDuration;
        // On calcule le easing grâce à la courbe, la progression et la force
        float curvedAmount = _groundPoundKnockBackHorizontalEasing.Evaluate(knockBackFreezeProgress) * _groundPoundKnockBackHorizontalForce;
        // On crée un vecteur de mouvement grâce à la direction et au easing
        Vector2 motion = _knockBackDirection * curvedAmount;
        // On applique le mouvement
        _rigidbody.velocity = motion;
    }
    private void DoHorizontalCanKnockBack()
    {
        // On calcule la progression du knockback
        float knockBackFreezeProgress = (_canKnockBackDuration - (_knockBackEndTime - Time.time)) / _canKnockBackDuration;
        // On calcule le easing grâce à la courbe, la progression et la force
        float curvedAmount = _canKnockBackHorizontalEasing.Evaluate(knockBackFreezeProgress) * _canKnockBackHorizontalForce;
        // On crée un vecteur de mouvement grâce à la direction et au easing
        Vector2 motion = _knockBackDirection * curvedAmount;
        // On applique le mouvement
        _rigidbody.velocity = motion;
    }
    public void EndKnockBack()
    {
        // On snap la position Y du sprite à 0
        _body.localPosition = new Vector3(0, 0, _body.localPosition.z);
    }
}
