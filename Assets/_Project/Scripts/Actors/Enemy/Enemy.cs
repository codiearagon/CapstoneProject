using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyStats _stats;

    private CircleCollider2D _rangeCollider;
    private Rigidbody2D _rb;

    private GameObject _targetObj;

    private bool _playerInRange;
    
    private void Awake()
    {
        _rangeCollider = GetComponentInChildren<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        _rangeCollider.radius = _stats.AttackRange;
        _stats.CurrentHp = _stats.MaxHp;

        Logger.Log("Enemy Initialized");
    }

    private void Start()
    {
        _targetObj = GameObject.FindGameObjectWithTag("Character");
    }

    private void FixedUpdate()
    {
        if(_targetObj != null)
        {
            Rigidbody2D targetRb = _targetObj.GetComponent<Rigidbody2D>();
            Vector2 moveValue = new Vector2(targetRb.position.x - _rb.position.x, targetRb.position.y - _rb.position.y);
            _rb.MovePosition(_rb.position + moveValue * (_stats.MovementSpeed / 10) * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            return;

        if(collision.CompareTag("Character"))
        {
            _playerInRange = true;
            StartCoroutine(Attack());
            Logger.Log("Attacking player");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null)
            return;

        if (collision.CompareTag("Character"))
        {
            _playerInRange = false;
            Logger.Log("Player out of range");
        }
    }

    private IEnumerator Attack()
    {
        Logger.Log("Attack coroutine started");
        while(_playerInRange)
        {
            _targetObj?.GetComponent<Character>().TakeDamage(2);
            yield return new WaitForSeconds(1 / _stats.AttackSpeed);
        }
    }

    public void TakeDamage(int amount)
    {
        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - amount, 0, _stats.MaxHp);
    }


    // Editor stuff
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stats.AttackRange);
    }
}
