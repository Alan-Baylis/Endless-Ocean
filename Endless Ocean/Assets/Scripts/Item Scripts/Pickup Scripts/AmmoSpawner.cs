using UnityEngine;
using System.Collections;

/// <summary>
/// This method spawns ammo blocks when mobs die.
/// </summary>
public class AmmoSpawner : MonoBehaviour
{

    /// <summary>
    /// This method spawns ammo. One ammo prefab for each piece of ammo a mob drops.
    /// </summary>
    /// <param name="amountOfAmmo">The amount of ammo the player gets.</param>
    /// <param name="positionToSpawnAt">The position to spawn the ammo at.</param>
    public static void spawnAmmo(int amountOfAmmo, Transform positionToSpawnAt)
    {
        for (int i = 0; i < amountOfAmmo; i++)
        {
            GameObject ammo = Instantiate(Resources.Load("Prefabs/Pickups/Ammo"), positionToSpawnAt.position, positionToSpawnAt.rotation) as GameObject;
            ammo.GetComponent<MoveTowardsObject>().objectToMoveTowards = GameObject.FindWithTag("Player");
        }
    }
}
