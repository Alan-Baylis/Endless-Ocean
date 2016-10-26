using UnityEngine;
using System.Collections;
using System;

public class Club : MeleeWeapon {

    private static AudioClip meleeAttackSound1;
    private static AudioClip meleeAttackSound2;

    Animator myAnimator;
    PlayerController character;

    static public string modelPathLocal = "Prefabs/Weapons/Club";
    // Use this for initialization
    new void Start()
    {
        if(meleeAttackSound1 == null)
        {
            meleeAttackSound1 = Resources.Load("Sounds/Melee Attack Sound 1") as AudioClip;
        }
        if(meleeAttackSound2 == null)
        {
            meleeAttackSound2 = Resources.Load("Sounds/Melee Attack Sound 2") as AudioClip;
        }
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
        System.Random random = new System.Random();
        int soundChoice = random.Next(0, 2);
        if(soundChoice == 0)
        {
            AudioSource.PlayClipAtPoint(meleeAttackSound1, this.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(meleeAttackSound2, this.transform.position);
        }
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
