using UnityEngine;
using System.Collections;

/// <summary>
/// This is the interface for weapons it contains functions for weapons that will be exposed to the player. 
/// </summary>
public interface IWeapon{


    /// <summary>
    /// This function will be called when the player left clicks with a weapon. It handles animation the attack and instianting the bullets for each weapon.
    /// 
    /// This function will have very different for different weapons eg: Pistol vs flamethrower.
    /// </summary>
    void attack(float playerDamage, Vector3 mousePositionInWorldCoords);


    //May not be necessary.
    void reload();

}
