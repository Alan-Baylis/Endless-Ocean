using UnityEngine;
using System.Collections;

public abstract class RangedWeapon : Weapon {

    private PlayerController player;

    protected void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    //The travel speed of the bullet.
    protected float projectileSpeed;
    //The range of the pistol.
    protected float range;

    //May not be necessary.
    abstract public void reload();

    /// <summary>
    /// Checks if the player has ammo to fire their weapon.
    /// </summary>
    /// <returns>A bool inidicating if the player has ammo to fire.</returns>
    protected virtual bool useAmmo()
    {
        if(this.player.ammo > 0)
        {
            this.player.ammo--;
            return true;
        }
        else
        {
            return false;
        }
    }
}
