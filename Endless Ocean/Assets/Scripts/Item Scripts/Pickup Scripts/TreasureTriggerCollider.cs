using UnityEngine;
using System.Collections;

public class TreasureTriggerCollider : MonoBehaviour {

    PlayerController player;

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
            Debug.Log("Collided2");
            player.totalTreasure += 100;
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
