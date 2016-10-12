using UnityEngine;
using System.Collections;

public class FloatyAI : EnemyAI {

	// Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    // Update is called once per refresh
    new void FixedUpdate()
    {
        base.FixedUpdate();

        //check if player is in range
        if (Vector3.Distance(transform.position, target.position) <= detectRange)
        {
            makeActionDecision();
        }
        else if (!patrolling)
        {
            moveCharacter(0);
        }
    }


    //This is the method used for moving the enemy from current location to a set destination - overrides from NPCBehaviour
    protected override void pathToLocation(Vector3 destination)
    {
        base.pathToLocation(destination);
        //float direction = 0;
        //if (rigidbody.position.x > destination.x)
        //{
        //    direction = -1;
        //}
        //else
        //{
        //    direction = 1;
        //}

        //moveCharacter(direction);
     }


    //This is the code which manages how the enemy attacks - overrides from NPCBehaviour
    protected override void attackTarget(Transform target)
    {
        base.attackTarget(target);
        //shoot at target, not their feet
        //Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 1.5f, target.position.z);
        //moveCharacter(0);

        //if (weapon != null)
        //{
        //    if (nextMelee < Time.time)
        //    {
        //        nextMelee = Time.time + weapon.getAttackSpeed();
        //        this.animator.SetTrigger("MeleeAttackTrigger");
        //        weapon.attack(attack, targetPosition);
        //    }
        //}
    }


    //This is where the enemy decides what to do - called from the fixedUpdate - overrides from enemyAI
    protected override void makeActionDecision()
    {
        base.makeActionDecision();

        ////Decide on an action
        //if (Vector3.Distance(transform.position, target.position) < shortestRange)
        //{
        //    swapWeapons(shortRangeWeapon);
        //    attackTarget(target);
        //}
        //else if (Vector3.Distance(transform.position, target.position) < longestRange)
        //{
        //    swapWeapons(longRangeWeapon);
        //    attackTarget(target);
        //}
        //else
        //{
        //    pathToLocation(target.position);
        //}

    }


}
