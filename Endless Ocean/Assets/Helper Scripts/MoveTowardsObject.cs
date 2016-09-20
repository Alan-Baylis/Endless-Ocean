using UnityEngine;
using System.Collections;

/// <summary>
/// This class moves an object towards the player - it is used to move pulled objects towards the player.
/// </summary>
public class MoveTowardsObject: MonoBehaviour
{

    public float velocity;
    public GameObject objectToMoveTowards;
    public bool moveInYAxis = false;

    /// <summary>
    /// Initalizes key varialbes for the class as mono behaviour classes cannot have constructors.
    /// </summary>
    public void init(GameObject objectToMoveTowards, float velocity, bool moveInYAxis)
    {
        this.velocity = velocity;
        this.objectToMoveTowards = objectToMoveTowards;
        this.moveInYAxis = moveInYAxis;
    }

    /// <summary>
    /// Runs once each frame and moves the object towards the player.
    /// </summary>
    void Update()
    {
        if (Vector3.Distance(objectToMoveTowards.transform.position, this.transform.position) > 2)
        {
            Vector3 tempDirection = objectToMoveTowards.transform.position;
            tempDirection.y += 1;
            Vector3 direction = (tempDirection - this.transform.position);
            if (!moveInYAxis)
            {
                direction.y = 0;
            }
            this.GetComponent<Rigidbody>().velocity = (direction * velocity);
        }
    }
}
