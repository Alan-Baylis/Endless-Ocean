using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class NPCBehaviour : CharacterSuper
{


    //float healthbar above enemy
    public Image healthBar;

    // Use this for initialization
    new void Start () {
        // healthBar = transform.FindChild("EnemyCanvas").FindChild("HealthBG").FindChild("HealthFG").GetComponent<Image>();

        base.Start();
    }
	
	// Update is called once per frame
	new void Update () {
	
	}

    protected void pathToLocation(Vector3 destination)
    {
        Vector3 direction = rigidbody.position - destination;
        float distance = direction.magnitude;
        direction = direction.normalized;

        moveCharacter(direction.y);
    }

    protected void attackTarget(Transform target)
    {
        weapon.attack(attack, target.position);
    }

    /// <summary>
    /// What happens when enemy collides with certain objects
    /// </summary>
    /// <param name="col">GameObject involved in collision</param>
    void OnTriggerEnter(Collider col)
    {
        // When enemy collides DeathFromfalling gameObject (fall down hole)
        if (col.gameObject.tag == "DeathCollider")
        {
            Destroy(this.gameObject);
        }
        // When player collides DeathFromfalling gameObject (fall down hole)
        if (col.gameObject.tag == "PlayerWeapon")
        {
            int damage = col.gameObject.GetComponent<Weapon>().getDamage();
            int knockBack = col.gameObject.GetComponent<Weapon>().getKnockBack();
            
            
            this.takeDamage(damage,col.gameObject.GetComponentInParent<Rigidbody>().position, knockBack);
            // Update health bar with new health
            healthBar.fillAmount = (float)this.health / (float)this.maxHealth;
            
            Debug.Log("My health is now " + this.health + "and I took " + damage + "damage");

            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
