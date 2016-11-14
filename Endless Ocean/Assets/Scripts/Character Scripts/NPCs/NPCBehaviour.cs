using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

/// <summary>
/// NPC Behaviour class which controls non-character players allowing them to perform actions that would otherwise be performed by the player
/// </summary>
public abstract class NPCBehaviour : CharacterSuper
{
    public AudioClip deathExplosion;
    
    //Drops
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

    //Pathing and Patrolling
    [SerializeField]
    public bool active;

    [SerializeField]
    protected bool patrolling;
    [SerializeField]
    protected Transform[] patrolLocales; //serializable setter
    protected List<Vector3> patrolLocations = new List<Vector3>();
    private int currentPatrolObjective;

    protected Vector3 pathLocationObjective;
    protected bool pathing;

    //wall and drop avoidance
    [SerializeField]
    protected Transform wallCheck;
    [SerializeField]
    protected Transform dropCheck;

    //Know thine weapons
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

    /// <summary>
    /// convert transform locales into 3D point locations
    /// </summary>
    private void convertLocales()
    {
        foreach (Transform locale in patrolLocales)
        {
            patrolLocations.Add(locale.position);
        }
    }
	
	// FixedUpdate is called after a set period
	new protected void FixedUpdate() {
        base.FixedUpdate();

        checkIfOnGround();
        animator.SetBool("grounded", true); //Currently having issues with checkIfOnground and AI
    }

    /// <summary>
    /// Set the patrol locations
    /// </summary>
    /// <param name="locations">The list of partol locations</param>
    public void setPatrol(List<Vector3> locations)
    {
        patrolling = true;
        patrolLocations = locations;
    }

    /// <summary>
    /// Control the patrol sequence 
    /// - set the pathToLocation and switching to the next location once the NPC reaches the current location
    /// </summary>
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

    /// <summary>
    /// Calculates the direction to the given destination and calls the CharacterSuper moveCharacter method
    /// </summary>
    /// <param name="destination">Where the robot should path to</param>
    public virtual void pathToLocation(Vector3 destination)
    {
        pathing = true;
        pathLocationObjective = destination;
        //get direction of travel
        float direction = 0;
        if (Math.Abs(Vector3.Distance(transform.position, pathLocationObjective)) < movementSpeed)
        {
            pathing = false;
        }
        else if (rigidbody.position.x > pathLocationObjective.x)
        {
             direction = -1;
        }
        else
        {
            direction = 1;
        }
        setDirection(direction);

        if (!groundCollisionCheck(dropCheck.position))
        {
            direction = 0;
        } else if (groundCollisionCheck(wallCheck.position))
        {
            direction = 0;
            //Jump
            this.rigidbody.AddForce(new Vector3(direction,1) * 120  );
        }

        moveCharacter(direction);
    }

    /// <summary>
    /// New implementation of the equipWeapon method from CharacterSuper
    /// Manages the equiping of weapons.
    /// </summary>
    /// <param name="weaponGameObject"> The Weapon to equip as a GameObject</param>
    /// <param name="mount"> The mount point to connect the weapon to</param>
    /// <param name="tag">The tag representing the user "player" for friendlies and "enemy" for enemies</param>
    new protected void equipWeapon(GameObject weaponGameObject, weaponMounts mount, string tag)
    {
        Weapon weapon = weaponGameObject.GetComponent<Weapon>();
        GameObject weaponToEquip = Instantiate(Resources.Load(weapon.getModelPath())) as GameObject;
        base.equipWeapon(weaponToEquip, mount, tag);
        checkWeapons();
    }

    /// <summary>
    /// Checks the current weapons equiped by the AI, deciding which should be used for long and short range
    /// Also uses these lengths for deciding the distance from their target before attacking
    /// </summary>
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

    /// <summary>
    /// Calls the attack method of the weapon, after aiming at the given target
    /// </summary>
    /// <param name="target"> target to aim attack at</param>
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
        Destroy(this.gameObject);
        GameObject onDeathSpawner = Instantiate(Resources.Load("Prefabs/Pickups/OnDeathSpawner"), this.transform.position, Quaternion.identity) as GameObject;
        onDeathSpawner.GetComponent<OnDeathSpawner>().startItemSpawningCoroutines(this.maxHealth, this.itemPossibilites, this.itemDrops);
        Instantiate(Resources.Load("Prefabs/Explosions/explosion_enemy"), this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathExplosion, this.transform.position, 2f);
    }


    /// <summary>
    /// Unity method for drawing visual aids in the developer screen
    /// </summary>
    protected virtual void OnDrawGizmos()
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

    /// <summary>
    /// Set the character to active
    /// </summary>
    public virtual void activate()
    {
        active = true;
    }

    /// <summary>
    /// Deactivate the character
    /// </summary>
    public virtual void deactivate()
    {
        active = false;
    }
}
