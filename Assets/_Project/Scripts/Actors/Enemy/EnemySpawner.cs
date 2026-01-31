using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyBaseSO enemyData;

    [SerializeField]
    private float _spawnInterval;

    private void Start()
    {
        StartCoroutine(SpawningCoroutine());
    }

    private IEnumerator SpawningCoroutine()
    {
        while(true)
        {
            GameObject enemy = Instantiate(enemyData.prefab, transform.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetBaseData(enemyData);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
