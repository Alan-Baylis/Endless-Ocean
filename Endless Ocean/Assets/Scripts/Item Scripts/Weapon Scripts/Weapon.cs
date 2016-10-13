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

    [HideInInspector]
    public int requiredAmmo;

    public override bool mouseClickEquipable
    {
        get { return true; }
    }

    public override bool reforgable
    {
        get
        {
            return true;
        }
    }

    // Attack variables
    [HideInInspector]
    public bool collisonHandled = false;
    public int stun;
    public int damage;
    public int knockBack;
    public int energyCost;
    public int range;

    [HideInInspector]
    protected string weaponTag;
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

    //Changing buy and sell values based off item quality.
    public override int buyValue
    {
        get
        {
            return ((int)quality * base.buyValue);
        }
    }

    public override int sellValue
    {
        get
        {
            return ((int)quality * base.sellValue);
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

    /// <summary>
    /// Sets the weapons tag depending on if the enemy or player is holding the weapon.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        if(this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            this.tag = "EnemyWeapon";
            if(this.transform.FindChild("collider") != null)
            {
                this.transform.FindChild("collider").tag = "EnemyWeapon";
            }
        }
        else
        {
            this.tag = "PlayerWeapon";
            if (this.transform.FindChild("collider") != null)
            {
                this.transform.FindChild("collider").tag = "PlayerWeapon";
            }
        }
    }
    
    public void setQuality(float luck)
    {
        int qualityInt = Random.Range(1, 100);
        qualityInt = (int) (qualityInt * luck);
        //if (quality != ItemQuality.NULL)
        //{
        //    qualityInt = (int)quality;
        //}


        if (qualityInt <= (int)ItemQuality.Crude)
        {
            qualityModifier = 0.5f;
            quality = ItemQuality.Crude;
        }
        else if (qualityInt <= (int)ItemQuality.Basic)
        {
            qualityModifier = 1f;
            quality = ItemQuality.Basic;
        }
        else if (qualityInt <= (int)ItemQuality.Improved)
        {
            qualityModifier = 1.5f;
            quality = ItemQuality.Improved;
        }
        else if (qualityInt <= (int)ItemQuality.Legendary)
        {
            qualityModifier = 2f;
            quality = ItemQuality.Legendary;
        }
        else
        {
            qualityModifier = 100f;
            quality = ItemQuality.Godly;
        }
    }  

    public int getDamage()
    {
        return (int) (damage * qualityModifier);
    }

    public float getStun()
    {
        return stun + (knockBack * 0.1f);
    }

    public int getKnockBack()
    {
        return knockBack;
    }

    public float getAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    protected void OnTriggerExit(Collider col)
    {
        collisonHandled = false;
    }

    public abstract string getModelPath();
}
