using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerObj;
    private Rigidbody2D rb;
    public EnemyPool EnemyPool {get; set;}

    public void releaseEnemy() {
        EnemyPool.Pool.Release(this);
    }
    void Start() {
        playerObj = GameObject.Find("Player").transform;
        rb = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        // Facing the player
        Vector3 direction = playerObj.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
        rb.rotation = angle;
    }
}
