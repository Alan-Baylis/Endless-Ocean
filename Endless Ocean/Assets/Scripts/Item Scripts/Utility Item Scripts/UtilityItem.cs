using UnityEngine;
using System.Collections;

/// <summary>
/// Class for utility items. The player can use the methods it to interact with utility items.
/// 
/// Utility itema re items that mayaffect the player object in some way. including the players movement.
/// 
/// If an object overrides or effects the player's movement the affects player's movement will be set to true and the 
/// moveWithItem function will need to be implemented - this will tell the player controller to use the utility 
/// items movement function's rather than the default movement implementation.
/// 
/// Each utility item has an empty game object assocatied with it. This object can be used to add componenets for display effects etc, this will 
/// prevent the playergame object from getting cluttered with too many componenets and helps separate the components by purpose. 
/// 
/// </summary>
public abstract class UtilityItem: ItemSuper {

    //A boolean indicating whether or not the item effects the players movement.
    public bool affectsPlayerMovement;

    /// <summary>
    /// Makes the player attempt to use the item.
    /// </summary>
    /// <param name="mouseLocationInWorldCoordinates">THe location fo the mouse in world coordinates often use to aim utility items.</param>
    /// <param name="playerRigidBody">The player's rigidbody object which the utility item will affect.</param>
    /// <returns>Returns a boolean indicating whether or not the player succesfully used the item.</returns>
    abstract public bool useItem(Vector3 mouseLocationInWorldCoordinates, Rigidbody playerRigidBody);

    /// <summary>
    /// Makes the player attempt to stop using the item.
    /// </summary>
    /// <returns>Returns a bool indicating whether or not the player is still using the item (false for succesfully stopping).</returns>
    abstract public bool stopUsingItem();

    /// <summary>
    /// Runs for every frame the player is using the item. Used to take the users input if they are using the item and affect the player in other ways.
    /// </summary>
    /// <param name="horizontalMove">The horizontal input axis value.</param>
    /// <param name="verticalMove">The vertical input axis value.</param>
    /// <param name="onGround">A boolean indicating whether or no the player is on the ground.</param>
    /// <param name="playerRigidBody">The player's rigidbody object which the utility item will affect.</param>
    /// <returns>Returns a boolean indicating whether or not the utility item influenced the player.</returns>
    abstract public bool whileUsingItem(float horizontalMove, float verticalMove, bool onGround, Rigidbody playerRigidBody);

    /// <summary>
    /// Runs for every frame the item is being used moves the player using input gather from whlieUsingItem.
    /// </summary>
    /// <param name="horizontalMove">The horizontal input axis value.</param>
    /// <param name="verticalMove">The vertical input axis value.</param>
    /// <param name="onGround">A boolean indicating whether or no the player is on the ground.</param>
    /// <param name="playerRigidBody">The player's rigidbody object which the utility item will affect.</param>
    abstract protected void moveWithItem(float horizontalMove, float verticalMove, bool onGround, Rigidbody playerRigidbody);
   

    
}
