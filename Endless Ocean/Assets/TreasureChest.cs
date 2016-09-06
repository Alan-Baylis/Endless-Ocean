using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour {

    Animator animator;

    // Use this for initialization
    void Start () {
        this.animator = this.GetComponent<Animator>();
    }

    protected void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown("e"))
            {
                this.animator.SetBool("opened", true);

                // Tressure drop code goes here
                /*
                System.Random treasureRandomizer = new System.Random();
                int treasureMax = treasureRandomizer.Next(1, 10);

                GameObject treasureObject = Instantiate(Resources.Load("Prefabs/Environment/Treasure"), this.transform.position, this.transform.rotation) as GameObject;
                treasureObject.GetComponent<MoveTowardsObject>().objectToMoveTowards = GameObject.FindWithTag("Player");*/
            }
        }
    }
}
