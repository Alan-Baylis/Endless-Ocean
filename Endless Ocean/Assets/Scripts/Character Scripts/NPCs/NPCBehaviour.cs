using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public abstract class NPCBehaviour : CharacterSuper
{
    [SerializeField]
    private String[] itemDrops = {"Prefabs/Consumables/TestPotion"};
    [SerializeField]
    private int[] itemPossibilites = {100};

    [SerializeField]
    private int maxAmountOfTreasure;
    [SerializeField]
    private int maxAmountOfExp;
    [SerializeField]
    private int maxAmountOfAmmo; //is this required?


    [SerializeField]
    protected bool patrolling;
    [SerializeField]
    protected Transform[] patrolLocales; //serializable setter
    protected List<Vector3> patrolLocations = new List<Vector3>();
    private int currentPatrolObjective;

    //Known thine weapons
    protected weaponMounts shortRangeWeapon;
    protected int shortestRange;

    protected weaponMounts longRangeWeapon;
    protected int longestRange;

    //float healthbar above enemy
    public Image healthBar;

    // Use this for initialization
    new void Start () {
        base.Start();
        checkWeapons();
        convertLocales();
        currentPatrolObjective = 0;
    }

    //convert locales into locations
    private void convertLocales()
    {
        foreach (Transform locale in patrolLocales)
        {
            patrolLocations.Add(locale.position);
        }
    }
	
	// Update is called once per frame
	new protected void FixedUpdate() {
        base.FixedUpdate();

        checkIfOnGround();
        animator.SetBool("grounded", true); //Currently having issues with checkIfOnground and AI
    }

    public void setPatrol(List<Vector3> locations)
    {
        patrolling = true;
        patrolLocations = locations;
    }

    protected void patrol()
    {
        convertLocales();
        Vector3[] locations = patrolLocations.ToArray();
        if (Vector3.Distance(transform.position, locations[currentPatrolObjective]) < movementSpeed)
        {
            int goTo = currentPatrolObjective + 1;
            if (goTo >= (locations.Length))
            {
                goTo = 0;
            }
            currentPatrolObjective = goTo;
        }

        pathToLocation(locations[currentPatrolObjective]);
    }

    protected virtual void pathToLocation(Vector3 destination)
    {
        float direction = 0;
        if (rigidbody.position.x > destination.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        moveCharacter(direction);
    }

    new protected void equipWeapon(GameObject weaponGameObject, weaponMounts mount, string tag)
    {
        Weapon weapon = weaponGameObject.GetComponent<Weapon>();
        GameObject weaponToEquip = Instantiate(Resources.Load(weapon.getModelPath())) as GameObject;
        base.equipWeapon(weaponToEquip, mount, tag);
        checkWeapons();
    }

    protected void checkWeapons()
    {
        if (primaryMount.weaponLoaded() && secondaryMount.weaponLoaded())
        {
            if (primaryMount.Weapon.range == Math.Min(primaryMount.Weapon.range, secondaryMount.Weapon.range))
            {
                shortRangeWeapon = weaponMounts.Primary;
                shortestRange = primaryMount.Weapon.range;

                longRangeWeapon = weaponMounts.Secondary;
                longestRange = secondaryMount.Weapon.range;
            }
            else
            {
                shortRangeWeapon = weaponMounts.Secondary;
                shortestRange = secondaryMount.Weapon.range;

                longRangeWeapon = weaponMounts.Primary;
                longestRange = primaryMount.Weapon.range;
            }
        }
        else if (primaryMount.weaponLoaded())
        {
            shortRangeWeapon = weaponMounts.Primary;
            shortestRange = 0;

            longRangeWeapon = weaponMounts.Primary;
            longestRange = primaryMount.Weapon.range;
        }
        else if (secondaryMount.weaponLoaded())
        {
            shortRangeWeapon = weaponMounts.Secondary;
            shortestRange = 0;

            longRangeWeapon = weaponMounts.Secondary;
            longestRange = secondaryMount.Weapon.range;
        }
        else
        {
            shortRangeWeapon = weaponMounts.Primary;
            shortestRange = 0;

            longRangeWeapon = weaponMounts.Primary;
            longestRange = 0;
        }
    }

    protected override void takeDamage(int damage, Vector3 source, int knockBack)
    {
        base.takeDamage(damage, source, knockBack);
        patrolling = false;
        pathToLocation(source); //only takes a step at the moment
    }

    protected virtual void attackTarget(Transform target)
    { 
        //shoot at target, not their feet
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 1.5f, target.position.z);
        moveCharacter(0);

        if (weapon != null)
        {
            if (nextMelee < Time.time)
            {
                nextMelee = Time.time + weapon.getAttackSpeed();
                this.animator.SetTrigger("MeleeAttackTrigger");
                weapon.attack(attack, targetPosition);
            }
        }
    }

    /// <summary>
    /// Kills the NPC and makes them drop exp, treasure and equipment.
    /// </summary>
    public override void die()
    {
        GameObject onDeathSpawner = Instantiate(Resources.Load("Prefabs/Pickups/OnDeathSpawner"), this.transform.position, Quaternion.identity) as GameObject;
        onDeathSpawner.GetComponent<OnDeathSpawner>().startItemSpawningCoroutines(this.maxHealth, this.itemPossibilites, this.itemDrops);
        Instantiate(Resources.Load("Prefabs/Explosions/explosion_enemy"), this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }


    //Draws the patrol Path
    void OnDrawGizmos()
    {
        if (patrolLocales != null && patrolLocales.Length > 1)
        {
            Vector3 start = new Vector3();
            Vector3 end = new Vector3();

            foreach(Transform location in patrolLocales)
            {
                if (end != new Vector3())
                {
                    start = end;
                }
                end = location.position;

                if(start != new Vector3())
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(start, end);
                }
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(end, new Vector3(2f, 2f, 2f));
            }

        }
    }
}
