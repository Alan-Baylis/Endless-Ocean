using UnityEngine;
using System.Collections;
/// <summary>
/// The ammo pickups have two colliders to make them collide with everything but they player but still register collisions with the player.
/// 
/// Ths class is for the collider that registers collisions with the player.
/// </summary>
public class AmmoTriggerCollider : MonoBehaviour
{

    PlayerController player;

    /// <summary>
    /// Initializes a reference to the player.
    /// </summary>
    void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }


    /// <summary>
    /// Runs when the treasure's trigger collider collides with something. If it collides with the player - the player picks up the treasure.
    /// </summary>
    /// <param name="other">The collider the object collided with.</param>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            player.ammo += 1;
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
