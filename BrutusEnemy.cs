using UnityEngine;

public class BrutusEnemy : Enemy
{
    protected override void Start()
    {
        // Set Brutus-specific stats
        zombieType = "Brutus";
        damage = 40;        // Very high damage
        health = 100;       // High health
        moveSpeed = 2f;     // Slow movement
        
        // Call base class Start to set up components
        base.Start();
    }
} 