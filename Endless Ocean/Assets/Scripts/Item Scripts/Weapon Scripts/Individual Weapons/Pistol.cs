using UnityEngine;
using System.Collections;
/// <summary>
/// This is the class for the pistol weapon it contians the damage, projectile speed and range of bullets for the pistol.
/// </summary>
public class Pistol : RangedWeapon  {

    static public string modelPathLocal = "Prefabs/Weapons/HandgunFBX";

    protected override void Start()
    {
        base.Start();
        base.weaponAttackSpeed = 0.5f;
        base.projectileSpeed = 30f;
        this.knockBack = 300;
        range = 10;
    }

    /// <summary>
    /// Instantiates a single bullet travelling the direction the pistol is facing.
    /// </summary>
    /// <param name="playerDamage">The players damage which is added to the weapons attack.</param>
    /// <param name="target">the direction the bullet will travel.</param>
    override public void attack(float playerDamage, Vector3 target)
    {
        GameObject bullet = base.getBulletPrefab();
        bullet.GetComponent<Bullet>().speed = base.projectileSpeed;
        bullet.GetComponent<Bullet>().damage = base.damage;
        bullet.GetComponent<Bullet>().knockBack = this.knockBack;
        bullet.transform.LookAt(target);
    }

    override public void reload()
    {

    }

    public override string getModelPath()
    {
        return modelPathLocal;
    }
}
