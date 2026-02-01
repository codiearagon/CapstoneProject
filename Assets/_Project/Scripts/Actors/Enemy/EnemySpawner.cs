using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyBaseSO enemyData;

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
            GameObject enemy = Instantiate(enemyData.prefab, transform.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetBaseData(enemyData);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
