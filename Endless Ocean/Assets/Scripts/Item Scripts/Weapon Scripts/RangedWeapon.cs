using UnityEngine;
using System.Collections;

public abstract class RangedWeapon : Weapon {

   
    //The travel speed of the bullet.
    protected float projectileSpeed;
    //The range of the pistol.
    protected float range;

    //May not be necessary.
    abstract public void reload();
}
