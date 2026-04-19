using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private List<TileBase> _tiles;

    [SerializeField]
    private List<GameObject> _shrinePrefabs;

    [SerializeField]
    private float _shrineChance = 30f;

    [SerializeField]
    private int _chunkSize = 16;

    [SerializeField]
    private int _renderDistance = 3;

    [SerializeField]
    private float _scale = 20f;

    private Vector2 _offset;
    private HashSet<Vector2Int> _generatedChunks = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> _shrineChunks = new HashSet<Vector2Int>();
    private GameObject _playerObj;

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
    }
    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;
    }

    private void Awake()
    {
        _offset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));
    }

    private void Update()
    {
        if (_playerObj == null)
            return;

        Vector2Int playerChunk = WorldToChunk(_playerObj.transform.position);
        GenerateChunksAround(playerChunk);
        ClearDistantChunks(playerChunk);
    }

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player;
    }

    private Vector2Int WorldToChunk(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / _chunkSize),
            Mathf.FloorToInt(worldPos.y / _chunkSize)
        );
    }

    private void GenerateChunksAround(Vector2Int centerChunk)
    {
        for(int x = -_renderDistance; x <= _renderDistance; x++)
        {
            for(int y = -_renderDistance; y <= _renderDistance; y++)
            {
                Vector2Int chunk = new Vector2Int(centerChunk.x + x, centerChunk.y + y);
                if(!_generatedChunks.Contains(chunk))
                    GenerateChunk(chunk);
            }
        }
    }

    private void GenerateChunk(Vector2Int chunk)
    {
        _generatedChunks.Add(chunk);

        for(int x = 0; x < _chunkSize; x++)
        {
            for(int y = 0;  y < _chunkSize; y++)
            {
                Vector3Int tilePos = new Vector3Int(
                    chunk.x * _chunkSize + x,
                    chunk.y * _chunkSize + y
                );

                float noise = Mathf.PerlinNoise(
                    (tilePos.x + _offset.x) / _scale,
                    (tilePos.y + _offset.y) / _scale
                );

                _tilemap.SetTile(tilePos, GetTileFromNoise(noise));
            }
        }

        if (!_shrineChunks.Contains(chunk) && Utility.RollChance(_shrineChance))
        {
            Vector3 shrinePos = new Vector3(
                chunk.x * _chunkSize + Random.Range(2, _chunkSize - 2),
                chunk.y * _chunkSize + Random.Range(2, _chunkSize - 2),
                0
            );

            Shrine shrine = Instantiate(GetRandomShrine(), shrinePos, Quaternion.identity).GetComponent<Shrine>();
            shrine.SetChunk(chunk, OnShrineRemoved);
            _shrineChunks.Add(chunk);
        }
    }

    private void ClearDistantChunks(Vector2Int centerChunk)
    {
        HashSet<Vector2Int> toRemove = new HashSet<Vector2Int>();

        foreach(Vector2Int chunk in _generatedChunks)
        {
            if(Mathf.Abs(chunk.x - centerChunk.x) > _renderDistance + 1 ||
               Mathf.Abs(chunk.y - centerChunk.y) > _renderDistance + 1)
            {
                for(int x = 0; x < _chunkSize; x++)
                {
                    for (int y = 0; y < _chunkSize; y++)
                    {
                        Vector3Int tilePos = new Vector3Int(
                            chunk.x * _chunkSize + x,
                            chunk.y * _chunkSize + y
                        );

                        _tilemap.SetTile(tilePos, null);
                    }
                }

                toRemove.Add(chunk);
            }
        }

        foreach(Vector2Int chunk in toRemove)
        {
            _generatedChunks.Remove(chunk);
        }
    }

    private void OnShrineRemoved(Vector2Int chunk)
    {
        Logger.Log("Removed shrine from chunk");
        _shrineChunks.Remove(chunk);
    }

    private TileBase GetTileFromNoise(float noise)
    {
        if (noise < 0.4f) return _tiles[0];
        if (noise < 0.7f) return _tiles[1];
        if (noise < 0.85f) return _tiles[2];
        return _tiles[3];
    }

    private GameObject GetRandomShrine()
    {
        float random = Random.Range(0f, 1f);
        if (random <= 0.9f) return _shrinePrefabs[0];
        return _shrinePrefabs[1];
    }
}
