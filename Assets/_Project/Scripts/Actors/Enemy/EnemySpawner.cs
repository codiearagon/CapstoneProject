using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyToSpawn;

    [SerializeField]
    private float _spawnInterval;

    [SerializeField]
    private bool _isSpawningConstantly;
    private float _currentScaling;

    private void Awake()
    {
        _currentScaling = 1;
    }

    private void Start()
    {
        TriggerSpawning(_isSpawningConstantly);
    }

    private IEnumerator SpawningCoroutine()
    {
        while(_isSpawningConstantly)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    public void ChangeScaling(float scaling)
    {
        _currentScaling = scaling;
    }

    public void TriggerSpawning(bool value)
    {
        if (value)
        {
            _isSpawningConstantly = true;
            StartCoroutine(SpawningCoroutine());
        }
        else
            _isSpawningConstantly = false;
    }

    public void SpawnEnemy()
    {
        SpawnEnemy(_currentScaling);
    }

    public void SpawnEnemy(float scaling)
    {
        GameObject enemy = Instantiate(_enemyToSpawn, transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().MultiplyStats(_currentScaling);
    }

    public void SpawnElite(GameObject prefab, float scaling)
    {
        GameObject enemy = Instantiate(_enemyToSpawn, transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().MakeElite(scaling);
    }

    public GameObject EnemyPrefab => _enemyToSpawn;
}
