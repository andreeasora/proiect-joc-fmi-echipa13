using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public const float minX = -30;
    public const float maxX = 30;
    public const float minY = -30;
    public const float maxY = 30;
    public const int maxLives = 3;
    public const int noWeapons = 3;
    public const int maxWeapon2Ammo = 20;
    
    public int Score {get; set;}
    public int WeaponType {get; private set;}
    public int Weapon2Ammo {get; private set;}
    public bool Weapon3Ready {get; private set;}
    public event System.Action onScoreUpdate;
    public event System.Action onLifeLost;
    public event System.Action onWeaponChange;
    public event System.Action onAmmoUpdate;

    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Sprite weaponGreen, weaponRed, weaponPurple;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    private const float ammoRechargeDelay = 1.5f;  // in seconds

    private Rigidbody2D rigidBody;
    private SpriteRenderer spRenderer;
    private int remainingLives;
    private bool isGhosted;
    private IEnumerator ghostingCoroutine;
    private float rechargeElapsedTime;
    private Vector2 movementDirection;
    private float movementSpeed;

    void Awake()
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
        
        WeaponType = 0;
        weaponSpriteRenderer.sprite = weaponGreen;
        Weapon2Ammo = maxWeapon2Ammo;
        Weapon3Ready = true;
        rechargeElapsedTime = 0.0f;
    }

    void Update()
    {
        // Recharge ammo for weapon 2
        if (Weapon2Ammo < maxWeapon2Ammo)
        {
            rechargeElapsedTime += Time.deltaTime;
            if (rechargeElapsedTime >= ammoRechargeDelay)
            {
                Weapon2Ammo += 1;
                rechargeElapsedTime = 0.0f;
                onAmmoUpdate?.Invoke();
            }
        }
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
            switch (WeaponType)
            {
                case 0:
                    FireBullet(firePoint.up);
                    break;
                case 1:
                    if (Weapon2Ammo == 0)
                        return;

                    const float angleBetweenBulletDirs = 15.0f;
                    FireBullet(Quaternion.AngleAxis(angleBetweenBulletDirs, Vector3.forward) * firePoint.up);
                    FireBullet(firePoint.up);
                    FireBullet(Quaternion.AngleAxis(-angleBetweenBulletDirs, Vector3.forward) * firePoint.up);
                    
                    Weapon2Ammo -= 1;
                    onAmmoUpdate?.Invoke();
                    break;
                case 2:
                    if (!Weapon3Ready)
                        return;

                    Vector3 blackHolePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    blackHolePosition.z = 0.0f;
                    var blackHole = Instantiate(blackHolePrefab, blackHolePosition, Quaternion.identity).GetComponent<BlackHole>();
                    blackHole.Creator = this;
                    Weapon3Ready = false;

                    onAmmoUpdate?.Invoke();
                    Invoke("SetWeapon3Ready", BlackHole.duration);
                    break;
            }
        }
    }

    public void SwitchWeaponEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            float mouseScrollValue = context.ReadValue<float>();
            if (mouseScrollValue > 0)
                WeaponType = (WeaponType + 1) % noWeapons;
            else if (mouseScrollValue < 0)
                WeaponType = (WeaponType - 1 + noWeapons) % noWeapons;
            
            switch (WeaponType)
            {
                case 0:
                    weaponSpriteRenderer.sprite = weaponGreen;
                    break;
                case 1:
                    weaponSpriteRenderer.sprite = weaponRed;
                    break;
                case 2:
                    weaponSpriteRenderer.sprite = weaponPurple;
                    break;
            }

            onWeaponChange?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Enemy") && !isGhosted) 
        {
            OnEnemyHitPlayer();
            other.GetComponent<Enemy>().releaseEnemy();
        }
    }

    private void OnEnemyHitPlayer()
    {
        if (remainingLives == 1)
        {
            if (ghostingCoroutine is not null)
            {
                StopCoroutine(ghostingCoroutine);
                ghostingCoroutine = null;
            }
            Debug.Log("Game Over!");
            SceneManager.LoadScene("LoseScreen", LoadSceneMode.Single);
        }
        else 
        {
            --remainingLives;
            ghostingCoroutine = OnPlayerGhosting();
            StartCoroutine(ghostingCoroutine);
            onLifeLost?.Invoke();
        }
    }

    private IEnumerator OnPlayerGhosting()
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

    private void FireBullet(Vector3 bulletDirection)
    {
        Bullet bullet = bulletPool.Pool.Get();
        bullet.player = this;
        bullet.transform.position = firePoint.position;
        bullet.transform.up = bulletDirection;
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * Bullet.bulletSpeed, ForceMode2D.Impulse);
    }

    private void SetWeapon3Ready()
    {
        Weapon3Ready = true;
        onAmmoUpdate?.Invoke();
    }
}
