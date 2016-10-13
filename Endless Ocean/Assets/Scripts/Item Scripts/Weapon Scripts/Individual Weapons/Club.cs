using UnityEngine;
using System.Collections;
using System;

public class Club : MeleeWeapon {

    Animator myAnimator;
    PlayerController character;

    static public string modelPathLocal = "Prefabs/Weapons/Club";
    // Use this for initialization
    new void Start()
    {
        base.Start();
        this.character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
    }

    public override void attack(float playerDamage, Vector3 mousePositionInWorldCoords)
    {
        this.trailRenderer.enabled = true;
        StartCoroutine(hideTrailWhenFinishedAttacking());

    }

    public override string getModelPath()
    {
        return modelPathLocal;
    }
    
    private IEnumerator hideTrailWhenFinishedAttacking()
    {
        //if(this.character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < this.character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        //{
        //    yield return null;
        //}
        yield return new WaitForSeconds(.2f);
        this.trailRenderer.enabled = false;
    }
}
