using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] // Add this attribute to ensure the reference persists
    private GameObject fireballPrefab; // Changed to private with SerializeField
    [SerializeField] protected float fireRate = 0.2f;  // Increased fire rate for more constant shooting
    [SerializeField] private string playerName;
    [SerializeField] private int damage;
    [SerializeField] private int health = 100;  // Set default health
    
    private float nextFireTime = 0f;
    private Camera mainCamera;
    private bool isDead = false;
    private SpriteRenderer spriteRenderer;  // To change appearance when dead
    private float damageMultiplier = 1f;
    private bool hasPiercingShots = false;
    private Collider2D playerCollider;
    private Rigidbody2D rb;
    
    // Add the TakeDamage method that Enemy script is trying to call
    public void TakeDamage(int amount)
    {
        if (isDead) return;
        
        health -= amount;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        health = 0;
        
        // Visual feedback
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;  // Gray out the player
        }
        
        // Disable collisions
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }
        
        Debug.Log($"Player {playerName} died!");
        EndGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize player
        if (string.IsNullOrEmpty(playerName))
            playerName = "Player";
            
        // Get the main camera reference
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        // Configure collider
        if (playerCollider != null)
        {
            playerCollider.isTrigger = false; // Ensure collider is not a trigger
        }

        // Configure Rigidbody2D
        if (rb != null)
        {
            rb.gravityScale = 0f; // No gravity for top-down game
            rb.drag = 0.5f; // Add some drag for smoother movement
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Better collision detection
        }

        // Validate the prefab reference
        if (fireballPrefab == null)
        {
            Debug.LogError("Fireball prefab is not assigned in the Unity Inspector!");
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Don't process input if player is dead
        if (isDead) return;

        // Get mouse position in world coordinates
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = (mousePosition - (Vector2)transform.position).normalized;

        // Continuous firing while holding left mouse button
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            ShootFireball(aimDirection);
            nextFireTime = Time.time + fireRate;
        }
    }

    void ShootFireball(Vector2 aimDirection)
    {
        if (fireballPrefab == null || isDead)
            return;

        Vector2 spawnPosition = (Vector2)transform.position + aimDirection * 0.5f;
        GameObject fireballInstance = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
        
        Fireball fireball = fireballInstance.GetComponent<Fireball>();
        if (fireball != null)
        {
            fireball.damage = Mathf.RoundToInt(fireball.damage * damageMultiplier);
            fireball.SetPiercing(hasPiercingShots);
        }

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        fireballInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void EndGame()
    {
        // Implement game over logic here
        Debug.Log("Game Over!");
        // You can add more logic to handle game over, like showing a UI or restarting the game
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public void SetPiercingShots(bool piercing)
    {
        hasPiercingShots = piercing;
    }

    // Public method to check if player is dead (useful for other scripts)
    public bool IsDead()
    {
        return isDead;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public void SetFireRate(float newRate)
    {
        fireRate = newRate;
        Debug.Log($"Player fire rate changed to: {fireRate}");
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug log to verify player collision detection is working
        Debug.Log($"Player collided with: {collision.gameObject.name}");
    }
}
