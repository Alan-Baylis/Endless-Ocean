using UnityEngine;
using System.Collections;

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
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.totalExperience += 100;
            Destroy(this.gameObject);
        }
    }
}
