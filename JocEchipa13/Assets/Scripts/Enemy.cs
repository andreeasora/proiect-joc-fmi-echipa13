using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerObj;
    private Rigidbody2D rb;
    public EnemyPool EnemyPool {get; set;}

    public int enemyScoreValue = 1;

    private float hitpoints;
    [SerializeField]
    private float maxHitpoints = 5;
    [SerializeField]
    private HealthbarBehaviour healthbar;

    void Start() {
        playerObj = GameObject.Find("Player").transform;
        rb = this.GetComponent<Rigidbody2D>();

        hitpoints = maxHitpoints;
        healthbar.SetHealth(hitpoints, maxHitpoints);
    }

    void FixedUpdate() {
        // Facing the player
        Vector3 direction = playerObj.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
        rb.rotation = angle;
    }

    public void releaseEnemy() {
        EnemyPool.Pool.Release(this);
    }

    public void takeHit(float damage, Player player) {
        hitpoints -= damage;
        healthbar.SetHealth(hitpoints, maxHitpoints);

        if (hitpoints <= 0) {
            releaseEnemy();
            player.Score += enemyScoreValue;
            player.OnScoreUpdate();
		PlayerPrefs.SetInt("score", player.Score);
            HighScoreManager highScoreManager = FindObjectOfType<HighScoreManager>();
            highScoreManager.SaveHighScore(player.Score);
        }
    }

    public void SetHealth(float health) {
        hitpoints = health;
        healthbar.SetHealth(hitpoints, maxHitpoints);
    }

    public void SetHealth() {
        hitpoints = maxHitpoints;
        healthbar.SetHealth(hitpoints, maxHitpoints);
    }
}
