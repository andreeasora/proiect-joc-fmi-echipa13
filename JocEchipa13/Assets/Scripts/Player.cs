using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public const float minX = -30;
    public const float maxX = 30;
    public const float minY = -30;
    public const float maxY = 30;
    public const int maxLives = 3;
    
    public int Score {get; set;}
    public event System.Action onScoreUpdate;
    public event System.Action onLifeLost;

    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private Transform firePoint;
    
    private Rigidbody2D rigidBody;
    private SpriteRenderer spRenderer;
    private int remainingLives;
    private bool isGhosted;
    private IEnumerator ghostingCoroutine;
    private Vector2 movementDirection;
    private float movementSpeed;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spRenderer = GetComponent<SpriteRenderer>();

        Score = 0;
        remainingLives = maxLives;
        isGhosted = false;
        ghostingCoroutine = null;
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
        
        // Flip character so its facing the mouse pointer
        var mousePos = Mouse.current.position.ReadValue();
        var playerPos = Camera.main.WorldToScreenPoint(rigidBody.transform.position);
        var playerToMouseX = mousePos.x - playerPos.x;
        if (playerToMouseX < 0)
            spRenderer.flipX = true;
        else
            spRenderer.flipX = false;
    }

    public void OnScoreUpdate() => onScoreUpdate?.Invoke();

    public void MoveEvent(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }

    public void FireEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Bullet bullet = bulletPool.Pool.Get();
            bullet.player = this;
            bullet.transform.position = firePoint.position;
            bullet.transform.up = firePoint.up;
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * Bullet.bulletSpeed, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Enemy") && !isGhosted) 
        {
            OnEnemyHitPlayer();
            other.GetComponent<Enemy>().releaseEnemy();
        }
    }

    void OnEnemyHitPlayer()
    {
        if (remainingLives == 0)
        {
            if (ghostingCoroutine is not null)
            {
                StopCoroutine(ghostingCoroutine);
                ghostingCoroutine = null;
            }
            Debug.Log("Game Over!");
        }
        else 
        {
            --remainingLives;
            ghostingCoroutine = OnPlayerGhosting();
            StartCoroutine(ghostingCoroutine);
            onLifeLost?.Invoke();
        }
    }

    IEnumerator OnPlayerGhosting()
    {
        const float ghostingDuration = 2.0f;  // seconds
        const int ghostingPhases = 9;
        const int playerLayer = 8;
        const int ghostedPlayerLayer = 9;
        const float phaseDuration = ghostingDuration / ghostingPhases;

        isGhosted = true;
        this.gameObject.layer = ghostedPlayerLayer;
        Color playerColor = spRenderer.color;
        for (int i = 0; i < ghostingPhases; ++i)
        {
            if (playerColor.a == 1.0f)
                playerColor.a = 0.5f;
            else
                playerColor.a = 1.0f;
            spRenderer.color = playerColor;
            yield return new WaitForSeconds(phaseDuration);
        }
        playerColor.a = 1.0f;
        spRenderer.color = playerColor;
        this.gameObject.layer = playerLayer;
        isGhosted = false;
    }
}
