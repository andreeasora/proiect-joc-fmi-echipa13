using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [Serializable]
    public struct EnemySpawnAreas {
        public BoxCollider2D Area;
        public float Chance;
    }
    [SerializeField]
    private EnemyPool enemyPool;
    [SerializeField]
    public static GameController Instance {get; private set;}
    [SerializeField]
    private float enemyRadius = 1.0f;
    [SerializeField]
    private EnemySpawnAreas[] enemySpawnAreas;
    [SerializeField]
    private float enemySpawnWaveDelay = 5.0f;
    [SerializeField]
    private int enemiesNumberWave = 10;

    private float lastEnemySpawn = 0;

    private void OnEnable() {
        Instance = this;
    }

    private void OnDisable() {
        Instance = null;
    }

    private void Update() {
        if (Time.time - lastEnemySpawn > enemySpawnWaveDelay) {
            lastEnemySpawn = Time.time;
            for (int i = 0; i < enemiesNumberWave; ++i){
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy() {
        // functions places an enemy in a random position in a valid spawn area
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
}
