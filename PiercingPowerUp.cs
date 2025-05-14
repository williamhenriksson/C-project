using UnityEngine;

public class PiercingPowerUp : PowerUp
{
    protected override void ApplyPowerUp(Player player)
    {
        player.SetPiercingShots(true); // Permanently enable piercing shots
    }

    protected override void ApplyPowerUp(Enemy enemy)
    {
        // Implement logic if enemies can have piercing shots
        // Example: enemy.SetPiercingShots(true);
    }
} 