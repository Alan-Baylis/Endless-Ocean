using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public abstract class NPCBehaviour : CharacterSuper
{


    //float healthbar above enemy
    public Image healthBar;

    // Use this for initialization
    new void Start () {
        // healthBar = transform.FindChild("EnemyCanvas").FindChild("HealthBG").FindChild("HealthFG").GetComponent<Image>();

        base.Start();
    }
	
	// Update is called once per frame
	new void Update () {
	
	}

    protected void pathToLocation(Vector3 destination)
    {
        Vector3 direction = rigidbody.position - destination;
        float distance = direction.magnitude;
        direction = direction.normalized;

        moveCharacter(direction.y);
    }

    protected void attackTarget(Transform target)
    {
        weapon.attack(attack, target.position);
    }

    /// <summary>
    /// Kills the NPC and makes them drop exp, treasure and equipment.
    /// </summary>
    public override void die()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject expShere = Instantiate(Resources.Load("Prefabs/Environment/ExpSphere"), this.transform.position, this.transform.rotation) as GameObject;
            expShere.GetComponent<MoveTowardsObject>().objectToMoveTowards = GameObject.FindWithTag("Player");
        }
        System.Random treasureRandomizer = new System.Random();
        int treasureMax = treasureRandomizer.Next(1, 10);
        for(int i = 0; i < treasureMax; i++)
        {
            GameObject treasureObject = Instantiate(Resources.Load("Prefabs/Environment/Treasure"), this.transform.position, this.transform.rotation) as GameObject;
            treasureObject.GetComponent<MoveTowardsObject>().objectToMoveTowards = GameObject.FindWithTag("Player");
        }

        Destroy(this.gameObject);
    }
}
