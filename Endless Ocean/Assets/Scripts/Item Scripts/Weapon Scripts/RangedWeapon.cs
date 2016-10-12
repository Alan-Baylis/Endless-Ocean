using UnityEngine;
using System.Collections;

public abstract class RangedWeapon : Weapon {
   
    //protected void Start()
    //{
    //    this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    //}

    //The travel speed of the bullet.
    protected float projectileSpeed;

    //May not be necessary.
    abstract public void reload();

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
