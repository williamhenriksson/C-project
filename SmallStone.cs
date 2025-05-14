using UnityEngine;

public class SmallStone : Obstacle
{
    protected override void Start()
    {
        size = 1f;
        isDestructible = false;
        health = 0;
        
        base.Start();
        
        // Set specific scale for small stone
        transform.localScale = new Vector3(size, size, 1f);
    }
} 