using UnityEngine;
using System.Collections;

public class NPCBehaviour : CharacterSuper
{

	// Use this for initialization
	new void Start () {
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
}
