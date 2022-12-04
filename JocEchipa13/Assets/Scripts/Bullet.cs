using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player player;
    public const float bulletSpeed = 30.0f;
    private const float maxActiveTime = 5.0f;
    public BulletPool BulletPool {get; set;}
    private float currentActiveTime;

    void OnEnable() => currentActiveTime = 0;

    void Update()
    {
        currentActiveTime += Time.deltaTime;
        if (currentActiveTime >= maxActiveTime)
            BulletPool.Pool.Release(this);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var other = collider.gameObject;
        if (other.CompareTag("Enemy"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            BulletPool.Pool.Release(this);

            // Those happen when the enemy dies.
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.releaseEnemy();
            player.Score += 1;
            player.OnScoreUpdate();

        }
    }
}
