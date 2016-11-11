using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class for the exp spheres that enemeies drop which travel towards the player and when colliding with the player grant exp.
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

    /// <summary>
    /// Runs each FixedUpdate. Makes the EXP orb move towards the player if it is close enough.
    /// </summary>
    void FixedUpdate()
    {
        if(Vector3.Distance(this.transform.position, player.gameObject.transform.position) < 20f)
        {
            this.gameObject.GetComponent<MoveTowardsObject>().objectToMoveTowards = this.player.gameObject;
            this.gameObject.GetComponent<MoveTowardsObject>().enabled = true;
        }
    }
}
