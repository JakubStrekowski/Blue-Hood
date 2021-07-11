using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterHealth : MonoBehaviour
{
    private readonly float KNOCKBACK_Y_AXIS_FACTOR = 0.4f;

    [SerializeField]
    private bool knockbackEnabled = false;

    [SerializeField]
    private bool invisibilityOnInjured = false;

    [SerializeField]
    private float knockbackVelocity = 200f;

    [SerializeField]
    private float horizontalKnockbackModifier = 200f;

    [SerializeField]
    private int startingHealth = 3;

    [SerializeField]
    private Animator _animator;

    private int _currenthealth;
    public int CurrentHealth
    {
        get
        {
            return _currenthealth;
        }
        set
        {
            if(IsImmune && value < _currenthealth)
            {
                return;
            }
            _currenthealth = value;
            OnHealthChange?.Invoke();
        }
    }

    private bool _isDead = false;
    public bool IsDead
    {
        get => _isDead;
        set
        {
            _isDead = value;
            OnIsDeathChange?.Invoke();
        }
    }

    public Collider2D myHitbox;
    public LayerMask enemyLayer;
    public bool IsImmune { get; set; }

    private Rigidbody2D _rb;

    public delegate void OnHeatlhChangeDelegate();
    public event OnHeatlhChangeDelegate OnHealthChange;

    public delegate void OnDeathChangeDelegate();
    public event OnDeathChangeDelegate OnIsDeathChange;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Injured") && invisibilityOnInjured)
        {
            IsImmune = true;
        }
        else
        {
            IsImmune = false;
        }
    }

    private void Start()
    {
        _currenthealth = startingHealth;
        OnHealthChange += CheckDeath;
    }

    private void CheckDeath()
    {
        if(_currenthealth < 0)
        {
            Destroy(gameObject);
        }
    }

    public void GetAttacked(Vector2 sourcePos)
    {
        if (!IsImmune) 
        {
            CurrentHealth--;
            _animator.SetTrigger("gotInjured");
            if(knockbackEnabled)
            {
                Vector2 knockBack = ((Vector2)transform.position - sourcePos + new Vector2(0, KNOCKBACK_Y_AXIS_FACTOR)).normalized * knockbackVelocity;
                _rb.velocity = new Vector2(knockBack.x * horizontalKnockbackModifier, knockBack.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!IsImmune && (((1 << collision.gameObject.layer) & enemyLayer.value) > 0) && collision.otherCollider == myHitbox) //check if bit of collision layer matches hurtfulLayers bit
            {
                CurrentHealth--;
                _animator.SetTrigger("gotInjured");
                if (knockbackEnabled)
                {
                    Vector2 knockBack = ((Vector2)transform.position - (Vector2)collision.gameObject.transform.position + new Vector2(0, KNOCKBACK_Y_AXIS_FACTOR)).normalized * knockbackVelocity;
                    _rb.velocity = new Vector2(knockBack.x * horizontalKnockbackModifier, knockBack.y);
                }
            }
        }
    }


}
