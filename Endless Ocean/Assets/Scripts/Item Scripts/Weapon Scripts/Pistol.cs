using UnityEngine;
using System.Collections;
/// <summary>
/// This is the class for the pistol weapon it contians the damage, projectile speed and range of bullets for the pistol.
/// </summary>
public class Pistol : RangedWeapon  {

    public void Start()
    {
        base.damage = 3;
        base.weaponAttackSpeed = 3f;
        base.projectileSpeed = 20f;
    }

    /// <summary>
    /// Instantiates a single bullet travelling the direction the pistol is facing.
    /// </summary>
    /// <param name="playerDamage">The players damage which is added to the weapons attack.</param>
    /// <param name="mousePositionInWorldCoords">The mouse position in the game which is the direction the bullet will travel.</param>
    override public void attack(float playerDamage, Vector3 mousePositionInWorldCoords)
    {
        if (Time.time > base.weaponNextAttack)
        {
            base.weaponNextAttack = Time.time + base.weaponAttackSpeed;
            GameObject bullet = base.getBulletPrefab();
            bullet.GetComponent<Bullet>().damage = base.damage + playerDamage;
            bullet.GetComponent<Bullet>().speed = base.projectileSpeed;
            bullet.transform.LookAt(mousePositionInWorldCoords);
        }
    }

    override public void reload()
    {

    }

}
