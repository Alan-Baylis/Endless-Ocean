using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

    private PlayerController player;
    
    /// <summary>
    /// Initalizes key variables.
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
