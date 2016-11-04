using UnityEngine;
using System.Collections;
using System;

public class EnemyAI : NPCBehaviour
{

    //enemy variables
    [SerializeField]
    public float detectRange;

    [SerializeField]
    public Weapon primaryWeapon;
    [SerializeField]
    public Weapon secondaryWeapon;

    [SerializeField]
    public Transform target;

    // Use this for initialization
    new void Start () {
        base.Start();
        this.target = PreserveAcrossLevels.playerInstance.transform;
        // Assign objects that damage this character upon collision
        base.fears = "Player";

        if (primaryWeapon != null)
        {
            equipWeapon(primaryWeapon.gameObject, weaponMounts.Primary, "EnemyWeapon");

            //set weapon quality
            primaryMount.Weapon.setQuality(luck);
        }
        if (secondaryWeapon != null)
        {
            equipWeapon(secondaryWeapon.gameObject, weaponMounts.Secondary, "EnemyWeapon");

            //set weapon quality
            secondaryMount.Weapon.setQuality(luck);
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
        base.Update();
	}

    new void FixedUpdate()
    {
        base.FixedUpdate();

        //check if player is in range
        if (Vector3.Distance(transform.position, target.position) <= detectRange)
        {
            makeActionDecision();
        }
        else if (!patrolling && !pathing)
        {
            moveCharacter(0);
        }
        else if (pathing)
        {
            pathToLocation(pathLocationObjective);
        }
        else if (patrolling) //MUST BE THE LAST OPTION
        {
            patrol();
        }
    }

    protected virtual void makeActionDecision()
    {
        //Decide on an action
        if (Vector3.Distance(transform.position, target.position) < shortestRange)
        {
            swapWeapons(shortRangeWeapon);
            attackTarget(target);
        }
        else if (Vector3.Distance(transform.position, target.position) < longestRange)
        {
            swapWeapons(longRangeWeapon);
            attackTarget(target);
        }
        else
        {
            pathToLocation(target.position);
        }
    }

    protected override void takeDamage(int damage, Vector3 source, int knockBack)
    {
        base.takeDamage(damage, source, knockBack);
        pathToLocation(target.position); //only takes a step at the moment
    }

    protected override void updateHealthBar()
    {
        healthBar.fillAmount = (float)this.health / (float)this.maxHealth;
    }

    /// <summary>
    /// Flashes the character model red over several frames when the ytake damage.
    /// </summary>
    /// <returns>Return null. Is a co-routine so returns at the end of each frame.</returns>
    protected override IEnumerator flashOnDamageTaken()
    {
        if (!flashing)
        {
            this.flashing = true;
            //Initializing colors.
            Transform bodyTransform = this.gameObject.transform.Find("Body");
            SkinnedMeshRenderer body = bodyTransform.gameObject.GetComponent<SkinnedMeshRenderer>();
            Material[] colorBackup = body.materials;
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    body.materials = new Material[] { this.damageMaterial };
                }
                else if (i % 2 == 0)
                {
                    body.materials = new Material[] { this.damageMaterial };
                }
                else
                {
                    body.materials = colorBackup;
                }
                yield return new WaitForSeconds(.15f);
            }
            body.materials = colorBackup;
            this.flashing = false;
        }
    }
}
