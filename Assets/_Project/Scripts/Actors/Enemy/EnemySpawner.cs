using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyToSpawn;

    [SerializeField]
    private float _spawnInterval;

    [SerializeField]
    private bool _isSpawning;

    private void Start()
    {
        StartCoroutine(SpawningCoroutine());
    }

    private IEnumerator SpawningCoroutine()
    {
        while(_isSpawning)
        {
            GameObject enemy = Instantiate(_enemyToSpawn, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
