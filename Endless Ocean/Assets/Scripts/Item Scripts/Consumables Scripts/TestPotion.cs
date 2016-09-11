using UnityEngine;
using System.Collections;

public class TestPotion : Consumable
{
    /// <summary>
    /// Restores all the players energy.
    /// </summary>
    /// <param name="player">The player.</param>
    public override void use(PlayerController player)
    {
        player.energy = player.maxEnergy;
    }
}
