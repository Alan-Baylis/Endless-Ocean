using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class for the exp spheres that enemeies drop which travel towards teh player and when colliding with the player grant exp.
/// </summary>
public class ExpSphere : MonoBehaviour {

    private PlayerController player;

    /// <summary>
    /// Runs when the object is created - initializes key variables.
    /// </summary>
    void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    /// <summary>
    /// Runs when the exp shere collides. If it collides with player it gives him exp.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.currentExperience += 28;
            Destroy(this.gameObject);
        }
    }
}
