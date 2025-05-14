using UnityEngine;

public class SmallTree : Obstacle
{
    protected override void Start()
    {
        size = 1.5f;
        isDestructible = true;
        health = 50;
        
        base.Start();
        
        // Set specific scale for small tree
        transform.localScale = new Vector3(size, size, 1f);
    }

    protected override void DestroyObstacle()
    {
        // Add tree destruction effects here (e.g., particles, sound)
        Debug.Log("Small tree destroyed!");
        base.DestroyObstacle();
    }
} 