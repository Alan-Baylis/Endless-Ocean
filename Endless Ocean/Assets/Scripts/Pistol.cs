using UnityEngine;
using System.Collections;
/// <summary>
/// This is the class for the pistol weapon it contians the damage, projectile speed and range of bullets for the pistol.
/// </summary>
public class Pistol : Weapon  {

    //The damage the bullet does when it collides.
    private float damage = 3;
    //The travel speed of the bullet.
    private float projectileSpeed = 15;
    //The range of the pistol.
    private float range = 4;
    //Firerate of the gun.
    private float fireRate = .5f;
    private float nextFire;

    /// <summary>
    /// Instantiates a single bullet travelling the direction the pistol is facing.
    /// </summary>
    /// <param name="playerDamage">The players damage which is added to the weapons attack.</param>
    /// <param name="mousePositionInWorldCoords">The mouse position in the game which is the direction the bullet will travel.</param>
    override public void attack(float playerDamage, Vector3 mousePositionInWorldCoords)
    {
        if (Time.time > this.nextFire)
        {
            this.nextFire = Time.time + this.fireRate;
            GameObject bullet = base.getBulletPrefab();
            bullet.GetComponent<Bullet>().damage = this.damage + playerDamage;
            bullet.GetComponent<Bullet>().speed = this.projectileSpeed;
            bullet.transform.LookAt(mousePositionInWorldCoords);
        }
    }

    override public void reload()
    {

    }
}
