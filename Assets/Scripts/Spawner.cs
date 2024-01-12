using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using TestWork.Infrastructure;
using TestWork.Units;

public class Spawner
{
    private Factory _factory;
    private Config _config;
    private GameManager _gameManager;

    public Action<int,int> OnSpawn;

    public List<Enemy> ActiveEnemies => _activeEnemies;

    private List<Enemy> _enemiesInPool = new List<Enemy>();
    private List<Enemy> _activeEnemies = new List<Enemy>();

    private int _currentWave;
    private int _delayBetweenWaves = 2000;
    private int _spawnSmallGoblinsCount = 2;
    private float _maxSpawnPosition = 10f;

    public Spawner(Factory factory, Config config, GameManager gameManager, Player player)
    {
        _gameManager = gameManager;
        _factory = factory;
        _config = config;
        _gameManager.OnStartGame += DeactivateEnemies;
        _gameManager.OnStartGame += ResetWaveIndex;
        _gameManager.OnStartGame += SpawnWave;        
        player.Spawner = this;
    }

    public void ReturnToPool(Enemy enemy)
    {
        _enemiesInPool.Add(enemy);
        _activeEnemies.Remove(enemy);
        if (enemy.UnitType == UnitType.BigGoblin)
        {
            CreateSmallGoblins(enemy);
        }
    }

    private void DeactivateEnemies()
    {
        while (_activeEnemies.Count > 0)
        {
            _activeEnemies[0].DeactivateUnit();
            _activeEnemies[0].gameObject.SetActive(false);
            _enemiesInPool.Add(_activeEnemies[0]);
            _activeEnemies.Remove(_activeEnemies[0]);
        }
    }

    private void ResetWaveIndex() => _currentWave = 0;

    private void SpawnWave()
    {
        for (int i = 0; i < _config.Waves[_currentWave].Enemies.Length; i++)
        {
            var enemyType = _config.Waves[_currentWave].Enemies[i];
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-_maxSpawnPosition, _maxSpawnPosition),
                0, UnityEngine.Random.Range(-_maxSpawnPosition, _maxSpawnPosition));

            SpawnEnemy(enemyType, spawnPosition);
        }
        _currentWave++;
        OnSpawn?.Invoke(_currentWave, _config.Waves.Length);
    }

    private Enemy SpawnEnemy(UnitType enemyType, Vector3 spawnPosition)
    {
        var enemy = GetEnemyInPool(enemyType);
        if (enemy == null)
        {
            enemy = _factory.Create<Enemy>(_config.GetEnemy(enemyType));
            enemy.Init();
            enemy.OnDie += CheckWinGameState;
        }
        else
            _enemiesInPool.Remove(enemy);

        enemy.transform.position = spawnPosition;
        enemy.transform.LookAt(enemy.Target.transform);

        enemy.OnSpawn?.Invoke();
        _activeEnemies.Add(enemy);
        return enemy;
    }

    private void CreateSmallGoblins(Enemy bigGoblin)
    {
        Vector3 spawnPosition;
        for (int i = 0; i < _spawnSmallGoblinsCount; i++)
        {
            if (i == 0)
                spawnPosition = bigGoblin.transform.position - bigGoblin.transform.forward + bigGoblin.transform.right;
            else
                spawnPosition = bigGoblin.transform.position - bigGoblin.transform.forward + -bigGoblin.transform.right;

            SpawnEnemy(UnitType.SmallGoblin, spawnPosition);
        }
    }    

    private async void CheckWinGameState()
    {
        await UniTask.Delay(_delayBetweenWaves);
        if (_activeEnemies.Count == 0 && _currentWave >= _config.Waves.Length)
        {
            _gameManager.OnWinGame?.Invoke();
        }
        else if (_activeEnemies.Count == 0)
        {
            SpawnWave();
        }
    }

    private Enemy GetEnemyInPool(UnitType unitType)
    {
        for (int i = 0; i < _enemiesInPool.Count; i++)
        {
            if (_enemiesInPool[i].gameObject.activeInHierarchy == false
                && _enemiesInPool[i].UnitType == unitType) return _enemiesInPool[i];
        }
        return null;
    }
}
