using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(ActorMovement))]
public class EnemyBrain : MonoBehaviour
{
    private Enemy _enemy;
    private ActorMovement _movement;

    private Coroutine _attackCoroutine;

    private bool attacking;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _movement = GetComponent<ActorMovement>();
        attacking = false;
    }

    private void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _enemy.AggroRadius);
        foreach(Collider2D hit in hits)
            IsCharacter(hit);
    }

    private void IsCharacter(Collider2D collider)
    {
        if (collider.GetComponent<Character>() != null)
            DecideAction(collider.gameObject);
        else
        {
            if(attacking)
            {
                StopCoroutine(_attackCoroutine);
                attacking = false;
            }

            _enemy.UpdateInputData(Vector2.zero, Vector2.zero);
        }
    }

    private void DecideAction(GameObject target)
    {
        // Within attack range
        if (_enemy.AttackRange >= Vector2.Distance(transform.position, target.transform.position))
        {
            if(!attacking)
            {
                _enemy.UpdateInputData(Vector2.zero, Vector2.zero);
                attacking = true;
                _attackCoroutine = StartCoroutine(Attack(target));
            }
        }
        else
        {
            if(attacking)
            {
                StopCoroutine(_attackCoroutine);
                attacking = false;
            }
            Move(target);
        }
    }

    private IEnumerator Attack(GameObject target)
    {
        while(attacking)
        {
            Logger.Log(gameObject.name + " attacked " + target.name);
            yield return new WaitForSeconds(_enemy.Stats.AttackSpeed);
        }
    }

    private void Move(GameObject target)
    {
        Logger.Log("Moving");
        _enemy.UpdateInputData(target.transform.position.normalized, Vector2.zero);
    }
}
