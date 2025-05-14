using UnityEngine;

public class BigStone : Obstacle
{
    protected override void Start()
    {
        size = 2f;
        isDestructible = false;
        health = 0;
        
        base.Start();
        
        // Set specific scale for big stone
        transform.localScale = new Vector3(size, size, 1f);
    }
} 