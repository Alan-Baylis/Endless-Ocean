using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class handles equipping/unequipping the players weapons with the Help of the WeaponSlot classes.
/// </summary>
public class CharacterWeapons : MonoBehaviour {

    //Reference to the players inventory. 
    public Inventory inventory;
    public Weapon[] equippedWeapons = new Weapon[2] {null, null};
    private PlayerController player;

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        this.transform.Find("FirstWeapon").GetComponent<WeaponSlot>().slotNumber = 0;
        this.transform.Find("SecondWeapon").GetComponent<WeaponSlot>().slotNumber = 1;
    }

    /// <summary>
    /// Equips a weaponMount on the player based on a slot the player dragged the weapon into.
    /// </summary>
    /// <param name="weapon">The weapon game object to equip.</param>
    /// <param name="slotNumber">The slotNUmber used to determine what weaponMount to equip.</param>
    public void equipWeaponFromUI(GameObject weapon, int slotNumber)
    {
        if (slotNumber == 0) {
            player.equipWeapon(weapon, CharacterSuper.weaponMounts.Primary, "Player");
        }
        else if (slotNumber == 1)
        {
            player.equipWeapon(weapon, CharacterSuper.weaponMounts.Secondary, "Player");
        }
    }

    /// <summary>
    /// Removes a weapon from the player based on them dragging a weapon out of a slot on the UI.
    /// </summary>
    /// <param name="slotNumber">The slot number of the weapon that was dragged out.</param>
    public void removeWeaponFromUI(int slotNumber)
    {
        if (slotNumber == 0)
        {
            player.removeWeaponFromMount(CharacterSuper.weaponMounts.Primary);
        }
        else if (slotNumber == 1)
        {
            player.removeWeaponFromMount(CharacterSuper.weaponMounts.Secondary);
        }
    }
}
