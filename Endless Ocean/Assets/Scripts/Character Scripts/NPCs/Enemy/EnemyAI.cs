using UnityEngine;
using System.Collections;

public class EnemyAI : NPCBehaviour
{

    //enemy variables
    public float attackRange;
    public float detectRange;

    public Transform target;

	// Use this for initialization
	new void Start () {
        this.health = 100;
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
	
	}

    void FixedUpdate()
    {
        transform.LookAt(target);

        //check if player is in range
        if (Vector3.Distance(transform.position, target.position) <= detectRange)
        {
            
            if (Vector3.Distance(transform.position, target.position) >= attackRange)
            {
                //move towards player   
                pathToLocation(target.position);
            }else
            {
                attackTarget(target);
            }

        }
    }
}
