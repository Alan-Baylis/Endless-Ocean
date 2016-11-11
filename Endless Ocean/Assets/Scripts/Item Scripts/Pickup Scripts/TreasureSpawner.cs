using UnityEngine;
using System.Collections;

/// <summary>
/// This method spawns treasure blocks when mobs die.
/// </summary>
public class TreasureSpawner : MonoBehaviour {

    private static string[] treasurePrefabs = {"Prefabs/Pickups/Treasure/Emerald", "Prefabs/Pickups/Treasure/Gold Coin", "Prefabs/Pickups/Treasure/Gold Goblet", "Prefabs/Pickups/Treasure/Ruby", "Prefabs/Pickups/Treasure/Topaz"};

    /// <summary>
    /// This method spawns treasure.
    /// </summary>
    /// <param name="amountOfTreasure">The amount of trasure the player gets.</param>
    /// <param name="positionToSpawnAt">The position to spawn the treasure at.</param>
    public static void spawnTreasure(int amountOfTreasure, Transform positionToSpawnAt)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < amountOfTreasure / 10; i++)
        {
            GameObject treasure = Instantiate(Resources.Load(treasurePrefabs[random.Next(0, 4)]), positionToSpawnAt.position, positionToSpawnAt.rotation) as GameObject;
            treasure.GetComponent<MoveTowardsObject>().objectToMoveTowards = GameObject.FindWithTag("Player");
        }
    }

    /// <summary>
    /// This co routine spawns the trasure over 10 frames to improve performance.
    /// </summary>
    /// <param name="amountOfTreasure">The amount of trasure the player gets.</param>
    /// <param name="positionToSpawnAt">The position to spawn the treasure at.</param>
    public static IEnumerator spawnTreasureCoroutine(int amountOfTreasure, Transform positionToSpawnAt, OnDeathSpawner.SpawnerCoroutineCallback callback)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < amountOfTreasure/10; i++)
        {
            GameObject treasure = Instantiate(Resources.Load(treasurePrefabs[random.Next(0, 4)]), positionToSpawnAt.position, positionToSpawnAt.rotation) as GameObject;
            treasure.GetComponent<MoveTowardsObject>().objectToMoveTowards = GameObject.FindWithTag("Player");
            yield return null;
        }
        callback();
    }
}
