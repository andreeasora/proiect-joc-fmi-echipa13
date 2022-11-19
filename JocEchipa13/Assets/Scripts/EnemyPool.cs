using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    private ObjectPool<Enemy> pool;

    public ObjectPool<Enemy> Pool
    {
        get
        {
            if (pool is null)
                pool = new ObjectPool<Enemy>(OnCreateEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy);
            return pool;
        }
    }

    [SerializeField]
    private Enemy enemyPrefab;

    Enemy OnCreateEnemy(){
        Enemy newEnemy = Instantiate(enemyPrefab);
        newEnemy.EnemyPool = this;
        newEnemy.transform.SetParent(this.transform);
        return newEnemy;
    }

    void OnGetEnemy(Enemy enemy){
        enemy.gameObject.SetActive(true);
    }

    void OnReleaseEnemy(Enemy enemy){
        enemy.gameObject.SetActive(false);
    }

    void OnDestroyEnemy(Enemy enemy){
        Destroy(enemy.gameObject);
    }

}
