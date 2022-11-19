using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public const float bulletSpeed = 30.0f;
    public BulletPool BulletPool {get; set;}

    void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        BulletPool.Pool.Release(this);
        
        var other = collision.gameObject;

        if (other.CompareTag("Enemy")){
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.releaseEnemy();
        }
    }
}
