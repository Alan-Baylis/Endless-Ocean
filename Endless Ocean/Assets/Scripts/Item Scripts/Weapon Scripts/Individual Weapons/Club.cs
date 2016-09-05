using UnityEngine;
using System.Collections;
using System;

public class Club : MeleeWeapon {

    Animator myAnimator;
    PlayerController character;

    static public string modelPathLocal = "Prefabs/Weapons/Club";
    // Use this for initialization
    void Start()
    {
        Debug.Log("Started");

        this.damage = 25;
        this.weaponAttackSpeed = 0.5f;
        this.knockBack = 250;
        this.energyCost = 25;

        this.original = Resources.Load(Club.modelPathLocal);
    }

    void FixedUpdate()
    {
    }

    public override void attack(float playerDamage, Vector3 mousePositionInWorldCoords)
    {

    }

    public string getModelPath()
    {
        return modelPathLocal;
    }

}
