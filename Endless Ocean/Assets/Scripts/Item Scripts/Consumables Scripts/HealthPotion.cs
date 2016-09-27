using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class for the health potion it handles all functionality to do with the potion.
/// </summary>
public class HealthPotion : Consumable
{

    /// <summary>
    /// Uses the potion. Restores a third of the player's health.
    /// </summary>
    /// <param name="player">The player whose health is restored.</param>
    public override void use(PlayerController player)
    {
        player.health += player.maxHealth / 3;
        Mathf.Clamp(player.health, 0, player.maxHealth);
    }

}
