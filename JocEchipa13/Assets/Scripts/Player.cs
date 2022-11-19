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

    private Vector2 movementDirection;
    private float movementSpeed;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        movementSpeed = 10.0f;
        movementDirection = Vector2.zero;
        rigidBody.gravityScale = 0.0f;
    }

    void FixedUpdate()
    {
        // Movement
        rigidBody.velocity = movementDirection * movementSpeed;

        // Facing the mouse pointer
        var mousePos = Mouse.current.position.ReadValue();
        var playerPos = Camera.main.WorldToScreenPoint(transform.position);
        var playerToMouse = new Vector3(mousePos.x - playerPos.x, mousePos.y - playerPos.y, transform.up.z);
        float anglesToRotate = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg - 90.0f; 
        rigidBody.rotation = anglesToRotate;
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

    void OnCollisionEnter2D(Collision2D collision){
        var other = collision.gameObject;

        if (other.CompareTag("Enemy")){
            print("Player Hit");
        }
    }


}
