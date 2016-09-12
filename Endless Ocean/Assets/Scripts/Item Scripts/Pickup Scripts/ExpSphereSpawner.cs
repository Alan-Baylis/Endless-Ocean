﻿using UnityEngine;
using System.Collections;
/// <summary>
/// This class contains the method for spawning exp spheres.
/// </summary>
public class ExpSphereSpawner : MonoBehaviour {

    /// <summary>
    /// This method spawns exp spheres. One sphere for each 10 points of exp the player gets.
    /// </summary>
    /// <param name="amountOfExp">The amount of exp the player gets.</param>
    /// <param name="positionToSpawnAt">The position to spawn the exp spheres at.</param>
    public static void spawnExpOrbs(int amountOfExp, Transform positionToSpawnAt) { 
        for (int i = 0; i < amountOfExp/10; i++)
        {
            GameObject expShere = Instantiate(Resources.Load("Prefabs/Environment/ExpSphere"), positionToSpawnAt.position, positionToSpawnAt.rotation) as GameObject;
            expShere.GetComponent<MoveTowardsObject>().objectToMoveTowards = GameObject.FindWithTag("Player");
        }
    }

}