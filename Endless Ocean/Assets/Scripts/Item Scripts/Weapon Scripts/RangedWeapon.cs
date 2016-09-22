using UnityEngine;
using System.Collections;

public abstract class RangedWeapon : Weapon {

    protected PlayerController player { get; set; }
   
    //protected void Start()
    //{
    //    this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    //}

    //The travel speed of the bullet.
    protected float projectileSpeed;

    //May not be necessary.
    abstract public void reload();

    /// <summary>
    /// Checks if the player has ammo to fire their weapon.
    /// </summary>
    /// <returns>A bool inidicating if the player has ammo to fire.</returns>
    new protected virtual bool useAmmo()
    {
        if (!base.useAmmo)
        {
            return true;
        }else if(this.player.ammo > 0 )
        {
            this.player.ammo--;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns a bullet prefab.
    /// </summary>
    /// <returns>A bullet prefab at the specified position.</returns>
    protected GameObject getBulletPrefab()
    {
        GameObject bullet = Instantiate(Resources.Load("Prefabs/Weapons/Bullet"), this.transform.position, this.transform.rotation) as GameObject;

        switch (tag)
        {
            case "PlayerWeapon":
                bullet.tag = "PlayerProjectile";
                break;
            case "EnemyWeapon":
                bullet.tag = "EnemyProjectile";
                break;
            default:
                break;
        }
        return bullet;
    }   
}
