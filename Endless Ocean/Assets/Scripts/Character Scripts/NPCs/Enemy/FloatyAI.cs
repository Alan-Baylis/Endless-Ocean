using UnityEngine;
using System.Collections;

public class FloatyAI : EnemyAI {

    private bool attacking;
    private Quaternion groundCheckLocationRotation;

	// Use this for initialization
	new void Start () {
        base.Start();
        this.longestRange = 10;
        this.groundCheckLocationRotation = this.groundCheck.rotation;
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
            this.facePlayer(this.target);
            this.makeActionDecision();
        }
        //else if (!patrolling)
        //{
        //    base.moveCharacter(0);
        //}
    }

    void LateUpdate()
    {
        this.groundCheck.transform.rotation = this.groundCheckLocationRotation;
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
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
        //shoot at target, not their feet
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 1.5f, target.position.z);
        
        
        
        
        
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

        //Decide on an action
        if (Vector3.Distance(transform.position, target.position) < longestRange)
        {
            attackTarget(target);
        }
        else
        {
            base.pathToLocation(target.position);
        }
    }

    /// <summary>
    /// Makes the floaty NPC look at the target.
    /// </summary>
    /// <param name="target">The target of the enemy. The player.</param>
    private void facePlayer(Transform target)
    {
        this.transform.LookAt(target);
    }


}
