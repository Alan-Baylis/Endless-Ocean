using UnityEngine;
using System.Collections;
using System;

public class Club : MeleeWeapon {

    Animator myAnimator;
    PlayerController character;


    // Use this for initialization
    void Start()
    {
        Debug.Log("Started");
        base.damage = 25;
        base.weaponAttackSpeed = 3f;
        base.knockBack = 250;
    }

    void FixedUpdate()
    {
    }

    public override void attack(float playerDamage, Vector3 mousePositionInWorldCoords)
    {

    }

    //public override void attack(float playerDamage, Vector3 mousePositionInWorldCoords)
    //{
    //   bool facingRight = (gameObject.GetComponentInParent<Transform>().lossyScale.z > 0);
    //   Collider[] enemyCollisions = Physics.OverlapSphere(this.gameObject.transform.position + (this.gameObject.transform.forward * base.reach), reach, base.enemyLayerMask);
    //   Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), this.gameObject.transform.position + transform.right * base.reach, this.gameObject.transform.rotation);

    //   foreach (Collider enemyCollider in enemyCollisions)
    //   {

    //   }
    //}

}
