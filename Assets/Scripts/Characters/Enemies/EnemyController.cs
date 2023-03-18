using KevinCastejon.CollisionEvents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyController : MonoBehaviour
{
    //Reference settings
    [Header("Références Settings")]
    [SerializeField]
    [Tooltip("Référence à l'enfant contenant le corps du personnage")]
    private Transform _body;
    [SerializeField]
    [Tooltip("Référence au SpawnDust")]
    private Transform _spawnDust;
    [SerializeField]
    [Tooltip("Référence à l'AttackBox")]
    private Collider2D _attackBox;
    [SerializeField]
    [Tooltip("Référence à l'AttackRangeBox")]
    private Collider2D _attackRangeBox;
    [SerializeField]
    [Tooltip("Référence à la CanAttackRangeBox")]
    private Collider2D _canAttackRangeBox;
    [SerializeField]
    [Tooltip("Référence à la SlamBox")]
    private GameObject _slamBox;
    [SerializeField]
    [Tooltip("Référence à la SlamRangeBox")]
    private Collider2D _slamRangeBox;
    [SerializeField]
    [Tooltip("Référence à la position de la canette portée")]
    private Transform _holdCanPosition;
    [Tooltip("Prefabs de cannettes")]
    [SerializeField] private Throwable[] _throwablesPool;
    [Tooltip("Prefabs de loots")]
    [SerializeField] private Collectible[] _lootsPool;

    //Health settings
    [Header("Health Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Points de vie de départ")]
    private int _healthPoints;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Delai avant disparition du cadavre")]
    private float _vanishingDuration = 3f;
    [Min(0)]
    [SerializeField]
    [Tooltip("Nombre de loots")]
    private int _lootCount = 3;

    //Speed settings
    [Header("Speed Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Vitesse de marche")]
    private float _speed = 2.5f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Vitesse de dash")]
    private float _dashSpeed = 6f;

    //Jump settings
    [Header("Jump Settings")]
    [SerializeField]
    [Tooltip("Capacité de saut")]
    private bool _canJump;
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
    [SerializeField]
    [Tooltip("Capacité de Slam")]
    private bool _canSlam;
    [SerializeField]
    [Tooltip("Capacité de lancer")]
    private bool _canThrow;
    [SerializeField]
    [Tooltip("Capacité de dash")]
    private bool _canDash;
    [SerializeField]
    [Tooltip("Cooldown de detection de collision après un dash")]
    private float _dashCollisionDetectionCoolDown = 0.1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du groundpound")]
    private float _slamDuration = 1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du ramassage de cannette")]
    private float _pickingDuration = 0.25f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du lancer de cannette")]
    private float _throwingDuration = 0.25f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du taunt")]
    private float _tauntingDuration = 1.5f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Nombre de combo maximum")]
    private int _maxCombo = 2;

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

    [Header("Special Knockback Settings")]
    [Min(0f)]
    [SerializeField]
    [Tooltip("Durée du Special knockback")]
    private float _specialKnockBackDuration = 0.1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Puissance horizontale du Special knockback")]
    private float _specialKnockBackHorizontalForce = 1f;
    [Min(0f)]
    [SerializeField]
    [Tooltip("Puissance verticale du Special knockback")]
    private float _specialKnockBackVerticalForce = 0.5f;
    [SerializeField]
    [Tooltip("Easing horizontal du Special knockback")]
    private AnimationCurve _specialKnockBackHorizontalEasing;
    [SerializeField]
    [Tooltip("Easing vertical du Special knockback")]
    private AnimationCurve _specialKnockBackVerticalEasing;

    // Références de composants
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private PlayerController _target;
    private Collision2DEvent _collDetector;
    private ScreenShaker _screenShaker;

    // Variables de timer
    private float _jumpEndTime;                                 // heure de fin de saut
    private float _slamEndTime;                                 // heure de fin de groundpound
    private float _dashCoolDownEndTime;                         // heure de fin de cooldown de dash
    private float _attackEndTime;                               // heure de fin d'attaque coup de poing
    private float _pickingEndTime;                              // heure de fin de ramassage de canette
    private float _throwingEndTime;                             // heure de fin de lancer de canette
    private float _tauntingEndTime;                             // heure de fin de taunt
    private float _knockBackEndTime;                            // heure de fin de knockBack
    private float _vanishingEndTime;                            // heure de fin de disparition de cadavre
    private float _landingEndTime;                              // heure de fin d'atterissage

    // Variables d'attaques
    private Vector2 _lastMoveDirection = Vector2.right;                             // direction du dernier déplacement
    private Vector2 _knockBackDirection;                                            // direction du knockBack en cours
    private HitType _hitType;                                                       // type du knockBack en cours
    private int _combo;                                                             // combo actuel
    private int _currentAttackMaxCombo;                                             // le nombre de fois où l'ia attaquera d'affilé
    private Throwable _throwable;                                                   // reference à la cannette portée


    // Propriétés
    public int HealthPoints => _healthPoints;
    public int Combo => _combo;
    public int CurrentAttackMaxCombo => _currentAttackMaxCombo;
    public bool CanSlam => _canSlam;
    public bool CanThrow => _canThrow;
    public bool CanJump => _canJump;
    public bool CanDash => _canDash;
    public bool IsJumpEnded => Time.time >= _jumpEndTime;
    public bool IsSlamEnded => Time.time >= _slamEndTime;
    public bool IsLandingEnded => Time.time >= _landingEndTime;
    public bool IsAttackEnded => Time.time >= _attackEndTime;
    public bool IsPickingEnded => Time.time >= _pickingEndTime;
    public bool IsThrowingEnded => Time.time >= _throwingEndTime;
    public bool IsTauntEnded => Time.time >= _tauntingEndTime;
    public bool IsDashEnded => _collDetector.TriggeringCollidersCount > 0;
    public bool IsDashCoolDownEnded => Time.time >= _dashCoolDownEndTime;
    public bool IsKnockBackEnded => Time.time >= _knockBackEndTime;
    public bool IsTargetReachable => Physics2D.IsTouching(_target.HitBox, _attackRangeBox);
    public bool IsTargetThrowable => Physics2D.IsTouching(_target.HitBox, _canAttackRangeBox);
    public bool IsTargetSlammable => Physics2D.IsTouching(_target.HitBox, _slamRangeBox);


    private void Awake()
    {
        // On récupère les composants
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collDetector = GetComponent<Collision2DEvent>();
        _screenShaker = GameObject.FindGameObjectWithTag("ScreenShaker").GetComponent<ScreenShaker>();
    }
    private void Start()
    {
        // On trouve une cible
        PickTarget();
        _spawnDust.parent = null;
    }

    public void PickTarget()
    {
        // On cherche une cible aléatoire
        PlayerController[] targets = FindObjectsOfType<PlayerController>();
        _target = targets.Length == 1 ? targets[0] : targets[Random.value > 0.5f ? 0 : 1];
    }
    public void PickComboCount()
    {
        if (_currentAttackMaxCombo == -1)
        {
            _currentAttackMaxCombo = Random.Range(1, _maxCombo + 1);
        }
    }
    public void ResetComboCount()
    {
        _currentAttackMaxCombo = -1;
        // On envoie le numéro de combo à l'Animator
        _animator.SetInteger("AttackCombo", 0);
    }

    public void DoTurnCharacter()
    {
        // On tourne le personnage dans le sens de son déplacement
        _transform.right = _lastMoveDirection.x < 0 ? Vector2.left : Vector2.right;
    }
    public void DoIdle()
    {
        // On fige le personnage
        _rigidbody.velocity = Vector2.zero;
    }
    public void DoHunt()
    {
        if (_target == null)
        {
            PickTarget();
        }
        // On détermine la direction vers la cible
        Vector2 direction = (_target.transform.position - transform.position).normalized;
        // On applique le déplacement
        _rigidbody.velocity = direction * _speed;
        //
        _lastMoveDirection = direction.x < 0 ? Vector2.left : Vector2.right;
    }
    public void DoFlee()
    {
        if (_target == null)
        {
            PickTarget();
        }
        // On détermine la direction opposée à la cible
        Vector2 direction = (transform.position - _target.transform.position).normalized;
        // On applique le déplacement
        _rigidbody.velocity = direction * _speed;
        //
        _lastMoveDirection = direction.x < 0 ? Vector2.left : Vector2.right;
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

    public void StartLand()
    {
        // On définit le paramètre d'animator concernant la progression de l'atterrissage
        _animator.SetFloat("LandProgress", 0f);
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

    public void StartSlaming()
    {
        // On active la SlamBox
        _slamBox.SetActive(true);
        // On définit l'heure où le slam sera terminé
        _slamEndTime = Time.time + _slamDuration;
    }
    public void DoSlaming()
    {
        // On calcule la progression du slam
        float slamProgress = (_slamDuration - (_slamEndTime - Time.time)) / _slamDuration;
        // On définit le paramètre d'animator concernant la progression du slam
        _animator.SetFloat("SlamingProgress", slamProgress);
    }
    public void SlamShake()
    {
        if (Mathf.Approximately(0f, _animator.GetFloat("SlamingProgress")))
        {
            return;
        }
        _screenShaker.Shake();        
    }
    public void EndSlaming()
    {
        // On active la SlamBox
        _slamBox.SetActive(false);
        // On définit le slam comme étant terminé
        _slamEndTime = Time.time;
        // On définit le paramètre d'animator concernant la progression du slam
        _animator.SetFloat("SlamingProgress", 0f);
    }

    public void StartAttack()
    {
        // On active l'AttackBox
        _attackBox.gameObject.SetActive(true);
        // On détermine l'heure où l'attaque prendra fin
        _attackEndTime = Time.time + _attackDuration;
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
        //
        _combo = _combo + 1 == _currentAttackMaxCombo ? 0 : _combo + 1;
        // On envoie le numéro de combo à l'Animator
        _animator.SetInteger("AttackCombo", _combo);
        // On désactive l'AttackBox
        _attackBox.gameObject.SetActive(false);
        // On définit le paramètre d'animator concernant la progression de l'attaque à 0
        _animator.SetFloat("AttackProgress", 0f);
    }

    public void StartTaunting()
    {
        // On définit le paramètre d'animator concernant la progression du taunt
        _animator.SetFloat("TauntProgress", 0f);
        // On définit l'heure où le taunt sera terminé
        _tauntingEndTime = Time.time + _tauntingDuration;
        //
        _lastMoveDirection = (_target.transform.position - _transform.position).normalized;
    }
    public void DoTaunting()
    {
        // On calcule la progression du taunt
        float tauntProgress = (_tauntingDuration - (_tauntingEndTime - Time.time)) / _tauntingDuration;
        // On définit le paramètre d'animator concernant la progression de l'atterrissage
        _animator.SetFloat("TauntProgress", tauntProgress);
    }
    public void EndTaunting()
    {
        // On définit l'atterissage comme étant terminé
        _tauntingEndTime = Time.time;
        // On définit le paramètre d'animator concernant la progression de l'atterrissage
        _animator.SetFloat("TauntProgress", 0f);
    }

    public void StartDashing()
    {
        // On applique le déplacement
        _rigidbody.velocity = _lastMoveDirection * _dashSpeed;
        //
        _dashCoolDownEndTime = Time.time + _dashCollisionDetectionCoolDown;
    }
    public void DoDashing()
    {
        // On applique le déplacement
        _rigidbody.velocity = _lastMoveDirection * _dashSpeed;
    }
    public void EndDashing()
    {
        //
        _dashCoolDownEndTime = Time.time;
    }
    public void StartPicking()
    {
        //
        _throwable = Instantiate(_throwablesPool[Random.Range(0, _throwablesPool.Length)]);
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
        _throwable.Throw(_lastMoveDirection, _attackBox.gameObject.layer);
    }
    public void DoThrowing()
    {
        // On calcule la progression du lancer
        float throwingProgress = (_throwingDuration - (_throwingEndTime - Time.time)) / _throwingDuration;
        // On définit le paramètre d'animator concernant la progression du lancer
        _animator.SetFloat("ThrowingProgress", throwingProgress);
    }

    public void StartVanishing()
    {
        while (_lootCount > 0)
        {
            Instantiate(_lootsPool[Random.Range(0, _lootsPool.Length)], transform.position, Quaternion.identity);
            _lootCount--;
        }
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
        //
        PlayerController attacker = attackData.AttackBox.GetComponentInParent<PlayerController>();
        //
        if (attacker)
        {
            //
            attacker.Stamina++;
        }
        // On détermine l'heure où le knockback sera terminé et on retranche des points de vie en fonction du type d'attaque
        // On retranche des points de vie au personnage et on détermine l'heure où le knockback sera terminé
        switch (_hitType)
        {
            case HitType.ATTACK:
                _knockBackEndTime = Time.time + (_knockBackDuration);
                break;
            case HitType.CAN:
                _knockBackEndTime = Time.time + (_canKnockBackDuration);
                break;
            case HitType.GROUNDPOUND:
                _knockBackEndTime = Time.time + (_groundPoundKnockBackDuration);
                break;
            case HitType.SPECIAL:
                _knockBackEndTime = Time.time + (_specialKnockBackDuration);
                break;
            default:
                break;
        }
        _healthPoints = Mathf.Max(0, _healthPoints - attackData.Damages);
        int it = _lootCount - _healthPoints;
        for (int i = 0; i < it; i++)
        {
            Instantiate(_lootsPool[Random.Range(0, _lootsPool.Length)], transform.position, Quaternion.identity);
            _lootCount--;
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
                DoVerticalSlamKnockBack();
                break;
            case HitType.CAN:
                DoVerticalCanKnockBack();
                break;
            case HitType.SPECIAL:
                DoVerticalSpecialKnockBack();
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
    private void DoVerticalSlamKnockBack()
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
    private void DoVerticalSpecialKnockBack()
    {
        // On calcule la progression du knockback
        float knockBackFreezeProgress = (_specialKnockBackDuration - (_knockBackEndTime - Time.time)) / _specialKnockBackDuration;
        // On calcule le easing grâce à la courbe, la progression et la force
        float curvedAmount = _specialKnockBackVerticalEasing.Evaluate(knockBackFreezeProgress) * _specialKnockBackVerticalForce;
        // On calcule la nouvelle position Y du sprite
        float newYPos = curvedAmount * _specialKnockBackVerticalForce;
        // On applique le mouvement
        _body.localPosition = new Vector3(0, newYPos, _body.localPosition.z);
    }
    public void DoHorizontalKnockback()
    {
        switch (_hitType)
        {
            case HitType.ATTACK:
                DoHorizontalAttackKnockBack();
                break;
            case HitType.GROUNDPOUND:
                DoHorizontalSlamKnockBack();
                break;
            case HitType.CAN:
                DoHorizontalCanKnockBack();
                break;
            case HitType.SPECIAL:
                DoHorizontalSpecialKnockBack();
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
    private void DoHorizontalSlamKnockBack()
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
    private void DoHorizontalSpecialKnockBack()
    {
        // On calcule la progression du knockback
        float knockBackFreezeProgress = (_specialKnockBackDuration - (_knockBackEndTime - Time.time)) / _specialKnockBackDuration;
        // On calcule le easing grâce à la courbe, la progression et la force
        float curvedAmount = _specialKnockBackHorizontalEasing.Evaluate(knockBackFreezeProgress) * _specialKnockBackHorizontalForce;
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
