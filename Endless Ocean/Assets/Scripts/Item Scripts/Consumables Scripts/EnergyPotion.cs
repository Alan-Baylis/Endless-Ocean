using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class for the energy potion it handles all functionality to do with the potion.
/// </summary>
public class EnergyPotion : Consumable
{

    /// <summary>
    /// Uses the potion. Restores a third of the player's health.
    /// </summary>
    /// <param name="player">The player whose health is restored.</param>
    public override void use(PlayerController player)
    {
        player.energy += player.maxEnergy / 3;
        Mathf.Clamp(player.energy, 0, player.maxEnergy);
    }

}