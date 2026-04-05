using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemies;

    [SerializeField]
    private float _spawnPadding;

    [SerializeField]
    private float _spawnInterval;

    [SerializeField]
    private bool _isSpawningConstantly;
    private float _statScaling;
    private float _expScaling;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;

        _statScaling = 1;
        _expScaling = 1;
    }

    private IEnumerator SpawningCoroutine()
    {
        while(_isSpawningConstantly)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private Vector2 GetSpawnPosition()
    {
        float camHeight = _camera.orthographicSize;
        float camWidth = camHeight * _camera.aspect;

        Vector2 camPos = _camera.transform.position;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:
                return new Vector2(Random.Range(camPos.x - camWidth, camPos.x + camWidth), camPos.y + camHeight + _spawnPadding);
            case 1:
                return new Vector2(Random.Range(camPos.x - camWidth, camPos.x + camWidth), camPos.y - camHeight - _spawnPadding);
            case 2:
                return new Vector2(camPos.x - camWidth - _spawnPadding, Random.Range(camPos.y - camHeight, camPos.y + camHeight));
            case 3:
                return new Vector2(camPos.x + camWidth + _spawnPadding, Random.Range(camPos.y - camHeight, camPos.y + camHeight));
            default:
                return new Vector2(Random.Range(camPos.x - camWidth, camPos.x + camWidth), camPos.y + camHeight + _spawnPadding);
        }
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
        int idx = Random.Range(0, _enemies.Count);
        GameObject prefab = _enemies[idx];
        Vector2 pos = GetSpawnPosition();

        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
        enemy.GetComponent<Enemy>().MultiplyStats(_statScaling, _expScaling);
    }
    
    public void SpawnElite()
    {
        int idx = Random.Range(0, _enemies.Count);
        GameObject prefab = _enemies[idx];
        Vector2 pos = GetSpawnPosition();

        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
        enemy.GetComponent<Enemy>().MakeElite(5 * _statScaling, 5 * _expScaling);
    }
}
