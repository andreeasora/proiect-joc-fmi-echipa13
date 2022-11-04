using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    private Rigidbody2D rigidBody;

    private Vector2 movementDirection;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        movementSpeed = 10.0f;
        movementDirection = Vector2.zero;
        rigidBody.gravityScale = 0.0f;
    }

    void FixedUpdate()
    {
        rigidBody.velocity = movementDirection * movementSpeed;

        var mousePos = Mouse.current.position.ReadValue();
        var playerPos = Camera.main.WorldToScreenPoint(transform.position);
        var playerToMouse = new Vector3(mousePos.x - playerPos.x, mousePos.y - playerPos.y, transform.up.z);
        transform.up = playerToMouse.normalized;
    }

    public void MoveEvent(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }
}
