using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

    private PlayerController player;
    

    void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

	void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.totalTreasure += 100;
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if(Vector3.Distance(player.gameObject.transform.position, this.transform.position) < 6)
        {
            this.gameObject.GetComponent<MoveTowardsObject>().enabled = true;
        }
    }
}
