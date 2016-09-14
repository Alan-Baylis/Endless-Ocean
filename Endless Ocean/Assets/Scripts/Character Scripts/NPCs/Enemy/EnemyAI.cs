using UnityEngine;
using System.Collections;
using System;

public class EnemyAI : NPCBehaviour
{

    //enemy variables
    public float attackRange;
    public float detectRange;

    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;

    public Boolean floating = false;

    public Transform target;

	// Use this for initialization
	new void Start () {
        this.health = 100;
        this.maxHealth = 100;

        base.Start();

        // Assign objects that damage this character upon collision
        base.fears = "Player";

        if (primaryWeapon != null)
        {
            equipWeapon(primaryWeapon.getModelPath(), weaponMounts.Primary, "EnemyWeapon");

            //set weapon quality
            primaryMount.Weapon.setQuality(luck);
            primaryMount.Weapon.useAmmo = false;
        }
        if (secondaryWeapon != null)
        {
            equipWeapon(secondaryWeapon.getModelPath(), weaponMounts.Secondary, "EnemyWeapon");

            //set weapon quality
            secondaryMount.Weapon.setQuality(luck);
            secondaryMount.Weapon.useAmmo = false;
        }

        //set the starter weapon to set weapon
        switch (activeWeaponType)
        {
            case weaponMounts.Primary:
                weapon = primaryMount.Weapon;
                break;
            case weaponMounts.Secondary:
                weapon = secondaryMount.Weapon;
                break;
        }
    }
	
	// Update is called once per frame
	new void Update () {
	
	}

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (floating)
        {
            transform.LookAt(target);
        }

        //check if player is in range
        if (Vector3.Distance(transform.position, target.position) <= detectRange)
        {
            
            if (Vector3.Distance(transform.position, target.position) >= attackRange)
            {
                pathToLocation(target.position);
            }else
            {
                attackTarget(target);
            }

        }
    }

    protected override void updateHealthBar()
    {
        healthBar.fillAmount = (float)this.health / (float)this.maxHealth;
    }
}
