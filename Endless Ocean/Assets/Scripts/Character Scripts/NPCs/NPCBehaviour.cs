using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public abstract class NPCBehaviour : CharacterSuper
{
    public String[] itemDrops = {"Prefabs/Consumables/TestPotion"};

    public int[] itemPossibilites = {100};

    private int maxAmountOfTreasure;
    private int maxAmountOfExp;
    private int maxAmountOfAmmo;

    //float healthbar above enemy
    public Image healthBar;
    protected bool tracking = false;

    // Use this for initialization
    new void Start () {
        // healthBar = transform.FindChild("EnemyCanvas").FindChild("HealthBG").FindChild("HealthFG").GetComponent<Image>();
        base.Start();
        tracking = false;
    }
	
	// Update is called once per frame
	new protected void FixedUpdate() {
        base.FixedUpdate();
        weapon.useAmmo = false;

        checkIfOnGround();
        animator.SetBool("grounded", true); //Currently having issues with checkIfOnground and AI
    }

    protected void pathToLocation(Vector3 destination)
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
        tracking = false;
    }

    new protected void equipWeapon(GameObject weaponGameObject, weaponMounts mount, string tag)
    {
        Weapon weapon = weaponGameObject.GetComponent<Weapon>();
        GameObject weaponToEquip = Instantiate(Resources.Load(weapon.getModelPath())) as GameObject;
        base.equipWeapon(weaponToEquip, mount, tag);
    }

    protected void attackTarget(Transform target)
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
        System.Random random = new System.Random();
        ExpSphereSpawner.spawnExpOrbs(random.Next(0, maxHealth/2), this.transform);
        StartCoroutine(TreasureSpawner.spawnTreasureCoroutine(random.Next(0, maxHealth/2), this.transform));
        AmmoSpawner.spawnAmmo(random.Next(0, maxHealth/10), this.transform);
        ItemSpawner.spawnSpecificItems(itemDrops, itemPossibilites, this.transform);
        Instantiate(Resources.Load("Prefabs/Explosions/explosion_enemy"), this.transform.position, Quaternion.identity);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
    }
}
