using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// This class initializes enemies in the level by setting key variables on Start.
/// </summary>
public class EnemyInitializer : MonoBehaviour {

    List<EnemyAI> floatyArray = new List<EnemyAI>();
    List<EnemyAI> enemies = new List<EnemyAI>();

    /// <summary>
    /// Runs when the level starts. Sets the target transform of enemies in the level to the player.
    /// </summary>
    void Start () {
        //Getting all the floaty objects.
        GameObject[] enemyProjectileGameObjects = GameObject.FindGameObjectsWithTag("EnemyProjectile");
        foreach(GameObject gameObject in enemyProjectileGameObjects)
        {
            if(gameObject.GetComponent<FloatyAI>() != null)
            {
                floatyArray.Add(gameObject.GetComponent<FloatyAI>());
            }
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy").Select(enemy => enemy.GetComponent<EnemyAI>()).ToList();
        enemies.AddRange(floatyArray);
        enemies.ForEach(enemy => enemy.target = GameObject.FindGameObjectWithTag("Player").transform);
	}
	
}
