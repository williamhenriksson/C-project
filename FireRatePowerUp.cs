using UnityEngine;

public class FireRatePowerUp : PowerUp
{
    [SerializeField] private float speedMultiplier = 1.5f;

    protected override void ApplyPowerUp(Player player)
    {
        Debug.Log($"Applying FireRate powerup. Current rate: {player.GetFireRate()}");
        float newFireRate = player.GetFireRate() / speedMultiplier;
        player.SetFireRate(newFireRate);
        Debug.Log($"New fire rate: {newFireRate}");
    }

    protected override void ApplyPowerUp(Enemy enemy)
    {
        // Example: If enemies have a shooting mechanic
        // float newFireRate = enemy.GetFireRate() / speedMultiplier;
        // enemy.SetFireRate(newFireRate);
    }
} 