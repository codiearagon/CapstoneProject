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
    private float _statScaling;
    private float _expScaling;

    private void Awake()
    {
        _statScaling = 1;
        _expScaling = 1;
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

    public void ChangeScaling(float scaling, float expScaling)
    {
        _statScaling = scaling;
        _expScaling = expScaling;
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
        SpawnEnemy(_statScaling);
    }

    public void SpawnEnemy(float scaling)
    {
        GameObject enemy = Instantiate(_enemyToSpawn, transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().MultiplyStats(_statScaling, _expScaling);
    }

    public void SpawnElite(GameObject prefab, float scaling)
    {
        GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().MakeElite(scaling);
    }

    public GameObject EnemyPrefab => _enemyToSpawn;
}
