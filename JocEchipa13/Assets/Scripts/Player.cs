using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private BulletPool bulletPool;
    [SerializeField]
    private Transform firePoint;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spRenderer;

    private Vector2 movementDirection;
    private float movementSpeed;

    public const float minX = -30;
    public const float maxX = 30;
    public const float minY = -30;
    public const float maxY = 30;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spRenderer = GetComponent<SpriteRenderer>();

        movementSpeed = 10.0f;
        movementDirection = Vector2.zero;
        rigidBody.gravityScale = 0.0f;
    }

    void FixedUpdate()
    {
        // Movement
        var newVelocity = movementDirection * movementSpeed;
        if ((rigidBody.position.x <= minX && newVelocity.x < 0) || (rigidBody.position.x >= maxX && newVelocity.x > 0))
            newVelocity.x = 0;
        if ((rigidBody.position.y <= minY && newVelocity.y < 0) || (newVelocity.y > 0 && rigidBody.position.y >= maxY))
            newVelocity.y = 0;
        rigidBody.velocity = newVelocity;
        
        // Flip character so its facing the current direction
        if (rigidBody.velocity.x > 0)
            spRenderer.flipX = false;
        else if (rigidBody.velocity.x < 0)
            spRenderer.flipX = true;
    }

    public void MoveEvent(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }

    public void FireEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Bullet bullet = bulletPool.Pool.Get();
            bullet.transform.position = firePoint.position;
            bullet.transform.up = firePoint.up;
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * Bullet.bulletSpeed, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Enemy"))
            print("Player Hit");
    }
}
