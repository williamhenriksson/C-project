using UnityEngine;

public class DamagePowerUp : PowerUp
{
    private float damageMultiplier = 1.5f;

    protected override void ApplyPowerUp(Player player)
    {
        player.SetDamageMultiplier(player.GetDamageMultiplier() * damageMultiplier);
    }

    protected override void ApplyPowerUp(Enemy enemy)
    {
        int newDamage = Mathf.RoundToInt(enemy.GetDamage() * damageMultiplier);
        enemy.SetDamage(newDamage);
    }
} 