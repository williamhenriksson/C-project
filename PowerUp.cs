using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected new Collider2D collider2D;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        
        // Debug log to check if PowerUp is initialized
        Debug.Log($"PowerUp {gameObject.name} initialized");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug log to check collision
        Debug.Log($"Collision detected with {other.gameObject.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tag detected");
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("Applying powerup to player");
                ApplyPowerUp(player);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                ApplyPowerUp(enemy);
                Destroy(gameObject); // Destroy after applying to enemy
            }
        }
    }

    protected abstract void ApplyPowerUp(Player player);
    protected abstract void ApplyPowerUp(Enemy enemy);
} 