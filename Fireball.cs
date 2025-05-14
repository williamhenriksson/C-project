using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float lifetime = 4f;  // Changed to 4 seconds as requested
    
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider2D;
    private bool isAlive = true;
    private float birthTime;
    private bool isPiercing = false;
    private int enemiesHit = 0;
    private const int MAX_PIERCE = 2;

    private void Start()
    {
        // Get required components
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        
        // Record birth time for this specific fireball
        birthTime = Time.time;
        
        // Ensure this fireball starts in alive state
        SetAliveState(true);
    }

    private void Update()
    {
        // Always move regardless of state
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Check if this specific fireball should transition to dead state
        if (isAlive && Time.time - birthTime >= lifetime)
        {
            SetAliveState(false);
        }
    }

    private void SetAliveState(bool alive)
    {
        isAlive = alive;
        if (spriteRenderer != null) spriteRenderer.enabled = alive;
        if (collider2D != null) collider2D.enabled = alive;
    }

    public void SetPiercing(bool piercing)
    {
        isPiercing = piercing;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlive) return;

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemiesHit++;

            if (!isPiercing || enemiesHit >= MAX_PIERCE)
            {
                SetAliveState(false);
            }
        }
    }
} 