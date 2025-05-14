using UnityEngine;

public class RunnerEnemy : Enemy
{
    protected override void Start()
    {
        // Set Runner-specific stats
        zombieType = "Runner";
        damage = 25;        // High damage
        health = 20;        // Low health
        moveSpeed = 11f;     // Fast movement
        
        // Call base class Start to set up components
        base.Start();
    }
} 