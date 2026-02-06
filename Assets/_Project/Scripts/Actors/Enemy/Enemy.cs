using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(IAttackBehaviour))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyStats _stats;

    private CircleCollider2D _rangeCollider;
    private Rigidbody2D _rb;
    private IAttackBehaviour _attackBehaviour;

    private GameObject _targetObj;
    private Rigidbody2D _targetRb;

    private bool _playerInRange;
    
    private void Awake()
    {
        _rangeCollider = GetComponentInChildren<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _attackBehaviour = GetComponent<IAttackBehaviour>();

        _rangeCollider.radius = _stats.AttackRange;
        _stats.CurrentHp = _stats.MaxHp;

        Logger.Log("Enemy Initialized");
    }

    private void Start()
    {
        _targetObj = GameObject.FindGameObjectWithTag("Character");
        _targetRb = _targetObj.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(_targetObj != null)
        {
            _rb.MovePosition(Vector2.MoveTowards(_rb.position, _targetRb.position, (_stats.MovementSpeed / 10) * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInRange = true;
        StartCoroutine(Attack());
        Logger.Log("Attacking player");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerInRange = false;
        Logger.Log("Player out of range");
    }

    private IEnumerator Attack()
    {
        Logger.Log("Attack coroutine started");
        while(_playerInRange)
        {
            _targetObj?.GetComponent<Character>().TakeDamage(2f, _stats.Affinity);
            yield return new WaitForSeconds(1 / _stats.AttackSpeed);
        }
    }

    public void TakeDamage(float amount, Affinity damageAffinity)
    {
        float affinityMultiplier = AffinityLookup.GetMultiplier(damageAffinity, _stats.Affinity);
        float finalDamage = amount * affinityMultiplier;
        Logger.Log(string.Format("Received Damage: {0}, {1} base * {2}, {3}", finalDamage, amount, affinityMultiplier, _stats.EnemyName));

        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - finalDamage, 0, _stats.MaxHp);

        if (_stats.CurrentHp <= 0)
            Destroy(gameObject);
    }


    // Editor stuff
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stats.AttackRange);
    }
}
