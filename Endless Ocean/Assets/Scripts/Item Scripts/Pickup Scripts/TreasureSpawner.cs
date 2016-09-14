using UnityEngine;
using System.Collections;

/// <summary>
/// This method spawns treasure blocks when mobs die.
/// </summary>
public class TreasureSpawner : MonoBehaviour {

    /// <summary>
    /// This method spawns treasure. One sphere for each 10 points of exp the player gets.
    /// </summary>
    /// <param name="amountOfTreasure">The amount of trasure the player gets.</param>
    /// <param name="positionToSpawnAt">The position to spawn the treasure at.</param>
    public static void spawnTreasure(int amountOfTreasure, Transform positionToSpawnAt)
    {
        for (int i = 0; i < amountOfTreasure / 10; i++)
        {
            GameObject expShere = Instantiate(Resources.Load("Prefabs/Pickups/Treasure"), positionToSpawnAt.position, positionToSpawnAt.rotation) as GameObject;
            expShere.GetComponent<MoveTowardsObject>().objectToMoveTowards = GameObject.FindWithTag("Player");
        }
    }
}
