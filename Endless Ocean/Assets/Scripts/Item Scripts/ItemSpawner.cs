﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// This class spawns items from enemies and chests.
/// </summary>
public class ItemSpawner : MonoBehaviour
{

    //Array containing resource path for all items in the game.
    public static String[] itemDatabase = {"Prefabs/Consumables/TestPotion", "Prefabs/Equipment/TestHelmet"};

    /// <summary>
    /// This function is used to spawn specific items.
    /// 
    /// Should be used when we want to control what items will spawn.
    /// </summary>
    /// <param name="possibleItems">The items that the enemy may drop.</param>
    /// <param name="itemChances">The chances of each item dropping.</param>
    /// <param name="positionToSpawnAt">Teh position the items should spawn at.</param>
    public static List<GameObject> spawnSpecificItems(String[] possibleItems, int[] itemChances, Transform positionToSpawnAt)
    {
        List<GameObject> spawnedObjects = new List<GameObject>(); 
        System.Random random = new System.Random();
        int randomInt = random.Next(0, 99);
        for(int i = 0; i < possibleItems.Length; i++)
        {
            if(randomInt < itemChances[i])
            {
                spawnedObjects.Add(Instantiate(Resources.Load(possibleItems[i]), positionToSpawnAt.position, positionToSpawnAt.rotation) as GameObject);
            }
        }
        return spawnedObjects;
    }

    /// <summary>
    /// This function spawns the specified number of random items.
    /// </summary>
    /// <param name="positionToSpawnAt">The position to spawn the items at.</param>
    /// <param name="numberOfItemsToSpawn">The number of items to spawn - default is one.</param>
    public static List<GameObject> spawnAnyItems(Transform positionToSpawnAt, int numberOfItemsToSpawn = 1)
    {
        List<GameObject> spawnedObjects = new List<GameObject>();
        System.Random random = new System.Random();
        for(int i = 0; i < numberOfItemsToSpawn; i++)
        {
            int randomInt = random.Next(0, itemDatabase.Length);
            spawnedObjects.Add(Instantiate(Resources.Load(itemDatabase[i]), positionToSpawnAt.position + new Vector3(0, 2, 0), positionToSpawnAt.rotation) as GameObject);
        }
        return spawnedObjects;
    }
}	