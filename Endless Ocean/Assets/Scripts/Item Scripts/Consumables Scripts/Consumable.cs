using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class that contains the functionality for consumables in the game. 
/// </summary>
public class Consumable : Item {

    /// <summary>
    /// Making consumables stack multiple times in the same slot in the inventory.
    /// </summary>
    public override bool stackable
    {
        get { return true; }
    }

    /// <summary>
    /// Allows consumables to placed in the slot in the Quick Item Bar.
    /// </summary>
    public override bool quickItemEquipable
    {
        get { return true; }
    }

    /// <summary>
    /// Runs each frame. Checks if the itemCount of a stack of consumables is zero. If it is it destroys the stack.
    /// </summary>
    protected virtual void Update()
    {
        base.Update();
        if (this.itemCount < 1)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Creating concrete implementation of the use function so that compile time references to Consumables can access the use function.
    /// </summary>
    /// <param name="player">A reference to the player.</param>
    public virtual void use(PlayerController player)
    {
        Debug.Log("Use called on Consumable reference. Nothing Happens.");
        return;
    }

    
}
