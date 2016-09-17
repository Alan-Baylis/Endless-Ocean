using UnityEngine;
using System.Collections;

/// <summary>
/// This is the super class for weapons it contains functions for weapons that will be exposed to the player. 
/// 
/// Weapons have their own gam,e object which instantiated buleets and effects for the weapon. 
/// Each child class will have its own implemetation of the attack method which will create its own effects.
/// 
/// It also cointains a method for getting a bullet prefab which the child classes may use.
/// </summary>
public abstract class Weapon: Item{



    public bool useAmmo = true;


    public override bool reforgable
    {
        get
        {
            return true;
        }
    }

    // Attack variables
    public int damage;
    public int knockBack;
    public int energyCost;
    public string weaponTag;
    public string WeaponTag
    {
        get
        {
            return weaponTag;
        }
        set
        {
            weaponTag = value;
        }
    }

    protected float qualityModifier = 1;

    //Firerate of the gun.
    public float weaponAttackSpeed;

    /// <summary>
    /// This function will be called when the player left clicks with a weapon. It handles animation the attack and instianting the bullets for each weapon.
    /// 
    /// This function will have very different for different weapons eg: Pistol vs flamethrower.
    /// </summary>
    abstract public void attack(float playerDamage, Vector3 mousePositionInWorldCoords);

    protected override void Start()
    {
        base.Start();
    }
    
    public void setQuality(float luck)
    {
        int qualityInt = Random.Range(1, 100);
        qualityInt = (int) (qualityInt * luck);
        //if (quality != ItemQuality.NULL)
        //{
        //    qualityInt = (int)quality;
        //}


        if (qualityInt <= (int)ItemQuality.crude)
        {
            qualityModifier = 0.5f;
            quality = ItemQuality.crude;
        }
        else if (qualityInt <= (int)ItemQuality.basic)
        {
            qualityModifier = 1f;
            quality = ItemQuality.basic;
        }
        else if (qualityInt <= (int)ItemQuality.improved)
        {
            qualityModifier = 1.5f;
            quality = ItemQuality.improved;
        }
        else if (qualityInt <= (int)ItemQuality.legendary)
        {
            qualityModifier = 2f;
            quality = ItemQuality.legendary;
        }
        else
        {
            qualityModifier = 100f;
            quality = ItemQuality.godly;
        }
    }  

    public int getDamage()
    {
        return (int) (damage * qualityModifier);
    }

    public int getKnockBack()
    {
        return knockBack;
    }

    public float getAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    public abstract string getModelPath();
}
