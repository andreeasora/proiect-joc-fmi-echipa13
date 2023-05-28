using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BlackHole : MonoBehaviour
{
    public Player Creator { get; set; }
    public const float duration = 10.0f;  // in seconds

    private const float damageDealt = 1.0f;
    private const float effectRadius = 12.0f;
    private const float attractionSpeed = 10.0f;  // in world units per second

    private Dictionary<GameObject, Tuple<AIBase, Rigidbody2D>> affectedEnemies;

    private void Awake()
    {
        affectedEnemies = new Dictionary<GameObject, Tuple<AIBase, Rigidbody2D>>();
        var attractionEffectArea = GetComponent<CircleCollider2D>();
        attractionEffectArea.radius = effectRadius;
        attractionEffectArea.isTrigger = true;
        Destroy(this.gameObject, duration);
    }

    private void FixedUpdate()
    {
        foreach (var (aiComp, rbComp) in affectedEnemies.Values)
        {
            Vector2 attractionDir = (transform.position - rbComp.transform.position).normalized;
            rbComp.velocity = (Vector2)aiComp.velocity + attractionDir * attractionSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemies"))
            return;

        var aiComp = other.GetComponent<AIBase>();
        var rbComp = other.GetComponent<Rigidbody2D>();
        aiComp.updatePosition = false;
        affectedEnemies.TryAdd(other.gameObject, new Tuple<AIBase, Rigidbody2D>(aiComp, rbComp));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemies"))
            return;
        if (!affectedEnemies.ContainsKey(other.gameObject))
            return;

        affectedEnemies[other.gameObject].Item1.updatePosition = true;
        affectedEnemies.Remove(other.gameObject);
    }

    private void OnDestroy()
    {
        foreach (var (enemyGO, (aiComp, _)) in affectedEnemies)
        {
            if (enemyGO != null)
            {
                enemyGO.GetComponent<Enemy>().takeHit(damageDealt, Creator);
                aiComp.updatePosition = true;
            }
        }
    }
}
