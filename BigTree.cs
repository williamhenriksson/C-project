using UnityEngine;

public class BigTree : Obstacle
{
    protected override void Start()
    {
        size = 3f;
        isDestructible = true;
        health = 100;
        
        base.Start();
        
        // Set specific scale for big tree
        transform.localScale = new Vector3(size, size, 1f);
    }

    protected override void DestroyObstacle()
    {
        // Add tree destruction effects here (e.g., particles, sound)
        Debug.Log("Big tree destroyed!");
        base.DestroyObstacle();
    }
} 