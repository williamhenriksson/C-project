using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    [SerializeField] protected float size = 1f;         // Size of the obstacle
    [SerializeField] protected bool isDestructible;     // Whether the obstacle can be destroyed
    [SerializeField] protected int health;              // Health if destructible
    
    protected SpriteRenderer spriteRenderer;
    protected Collider2D obstacleCollider;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        // Get or add required components
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            Debug.LogWarning($"Added missing SpriteRenderer to {gameObject.name}");
        }

        obstacleCollider = GetComponent<Collider2D>();
        if (obstacleCollider == null)
        {
            // Add appropriate collider based on the obstacle type
            if (this is BigStone || this is SmallStone)
            {
                obstacleCollider = gameObject.AddComponent<BoxCollider2D>();
                Debug.Log($"Added BoxCollider2D to {gameObject.name}");
            }
            else
            {
                obstacleCollider = gameObject.AddComponent<CircleCollider2D>();
                Debug.Log($"Added CircleCollider2D to {gameObject.name}");
            }
        }

        // Add Rigidbody2D if not present
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
            rb.useFullKinematicContacts = true;
            Debug.Log($"Added Rigidbody2D to {gameObject.name}");
        }
    }

    protected virtual void Start()
    {
        UpdateSize();
    }

    protected virtual void OnValidate()
    {
        // This is called when values are changed in the Unity Inspector
        if (Application.isPlaying)
        {
            UpdateSize();
        }
    }

    protected virtual void UpdateSize()
    {
        // Configure collider
        if (obstacleCollider != null)
        {
            obstacleCollider.isTrigger = false;
            
            // Adjust collider size based on the obstacle's size
            if (obstacleCollider is CircleCollider2D circleCollider)
            {
                circleCollider.radius = size * 0.5f;
                Debug.Log($"Set CircleCollider2D radius to {circleCollider.radius} for {gameObject.name}");
            }
            else if (obstacleCollider is BoxCollider2D boxCollider)
            {
                boxCollider.size = new Vector2(size, size);
                Debug.Log($"Set BoxCollider2D size to {boxCollider.size} for {gameObject.name}");
            }
        }

        // Set the transform scale
        transform.localScale = new Vector3(size, size, 1f);
        Debug.Log($"Set transform scale to {transform.localScale} for {gameObject.name}");
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDestructible)
        {
            health -= damage;
            if (health <= 0)
            {
                DestroyObstacle();
            }
        }
    }

    protected virtual void DestroyObstacle()
    {
        // Override this in subclasses to add specific destruction effects
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}");
    }
} 