using UnityEngine;
using System.Collections;
/// <summary>
/// The treasure pickups have two colliders to make them collide with everything but they player but still register collisions with the player.
/// 
/// Ths class is for the collider that registers collisions with everything but the player and other trasure functionality.
/// </summary>
public class Treasure : MonoBehaviour {

    private PlayerController player;
    
    /// <summary>
    /// Initalizes referene to the player.
    /// </summary>
    void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    /// <summary>
    /// Moves the object towards the player if it is close enough.
    /// </summary>
    void Update()
    {
        if(Vector3.Distance(player.gameObject.transform.position, this.transform.position) < 6)
        {
            this.gameObject.GetComponent<MoveTowardsObject>().enabled = true;
        }
    }
}
