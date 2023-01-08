using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [Serializable]
    public struct EnemySpawnAreas {
        public BoxCollider2D Area;
        public float Chance;
    }
    [Serializable]
    public struct EnemyPoolChance {
        public EnemyPool enemyPool;
        public float Chance;
    }
    [SerializeField]
    public static GameController Instance {get; private set;}
    [SerializeField]
    private float enemyRadius = 1.0f;
    [SerializeField]
    private EnemySpawnAreas[] enemySpawnAreas;
    [SerializeField]
    private EnemyPoolChance[] enemyPools;
    [SerializeField]
    private float enemySpawnWaveDelay = 5.0f;
    [SerializeField]
    private int enemiesNumberWave = 10;
    private float lastEnemySpawn = 0;
    [SerializeField]
    private int enemyIncreaseRate = 1;
    [SerializeField]
    private float increaseEnemyDelay = 10.0f;
    private float lastEnemyIncrease = 0;

    [SerializeField]
    private int maxEnemiesSpawnRate = 10;

    

    public static String MapSceneName {get; set;} = null;

    private void Awake() {
        if (MapSceneName is not null)
            SceneManager.LoadScene(MapSceneName, LoadSceneMode.Additive);
    }

    private void OnEnable() {
        Instance = this;
    }

    private void OnDisable() {
        Instance = null;
    }

    private void Start() {
        AstarPath.active.Scan();
    }

    private void Update() {
        if (Time.time - lastEnemySpawn > enemySpawnWaveDelay) {
            lastEnemySpawn = Time.time;
            for (int i = 0; i < enemiesNumberWave; ++i){
                if (FindObjectsOfType<Enemy>().Length < 200)
                    SpawnEnemy();
            }
        }
        if (Time.time - lastEnemyIncrease > increaseEnemyDelay && enemiesNumberWave < maxEnemiesSpawnRate) {
            lastEnemyIncrease = Time.time;
            enemiesNumberWave += enemyIncreaseRate;
            if (enemiesNumberWave > maxEnemiesSpawnRate) {
                enemiesNumberWave = maxEnemiesSpawnRate;
            }
        }
    }

    private void SpawnEnemy() {
        // functions places an enemy in a random position in a valid spawn area
        EnemyPool enemyPool = RandomEnemyPool();
        Enemy enemy = enemyPool.Pool.Get();
        BoxCollider2D area = RandomAreaSelect();

        var bounds = area.bounds;

        bounds.size -= new Vector3(enemyRadius, enemyRadius, 0) * 2.0f;

        var position = new Vector3(
            Mathf.Lerp(bounds.min.x, bounds.max.x, Random.value),
            Mathf.Lerp(bounds.min.y, bounds.max.y, Random.value),
            0
        );

        enemy.transform.position = position;
    }

    private BoxCollider2D RandomAreaSelect() {
        // Function returns a randomly selected area
        var sum = enemySpawnAreas.Select(x => x.Chance).Sum();

        var point = Random.value * sum;
        var accumulator = 0.0f;

        foreach (var area in enemySpawnAreas)
        {
            accumulator += area.Chance;
            if (accumulator >= point)
                return area.Area;
        }

        return enemySpawnAreas.First().Area;
    }
    private EnemyPool RandomEnemyPool() {
        // Function returns a randomly selected area
        var sum = enemyPools.Select(x => x.Chance).Sum();

        var point = Random.value * sum;
        var accumulator = 0.0f;

        foreach (var pool in enemyPools)
        {
            accumulator += pool.Chance;
            if (accumulator >= point)
                return pool.enemyPool;
        }

        return enemyPools.First().enemyPool;
    }
}
