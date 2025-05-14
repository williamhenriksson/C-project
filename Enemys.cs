using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected string zombieType = "Basic";  // Default type
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int health = 40;  // Increased health to take multiple fireballs
    [SerializeField] protected float moveSpeed = 3f;
    
    protected Transform playerTransform;
    protected bool isDead;
    protected NavMeshAgent agent;
    protected Collider2D enemyCollider;  // Added collider reference
    protected SpriteRenderer spriteRenderer;  // Add this to control visibility

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Cache the player reference
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        if (playerTransform == null)
            Debug.LogWarning($"{zombieType}: Player not found!");
            
        // Get the collider component
        enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider == null)
            Debug.LogError($"{zombieType} needs a Collider2D component!");

        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the sprite renderer

        // Set up NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && playerTransform != null)
        {
            if (agent != null)
            {
                // Use NavMesh pathfinding
                agent.SetDestination(playerTransform.position);
            }
            else
            {
                // Fallback to direct movement
                Move();
            }
        }
    }

    private void Move()
    {
        if (playerTransform == null || isDead) return;
        
        // Calculate direction to player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        
        // Move towards player
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

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
        if (agent != null) agent.enabled = false;
        if (enemyCollider != null) enemyCollider.enabled = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false;  // Hide the enemy
        
        // Disable the GameObject after a delay but don't destroy it
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        
        // Check if collided with player
        if (collision.CompareTag("Player"))
        {
            // Deal damage to player
            Player player = collision.GetComponent<Player>();
            player?.TakeDamage(damage);
        }
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public int GetDamage()
    {
        return damage;
    }
}