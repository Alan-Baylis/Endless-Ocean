using UnityEngine;
using System.Collections;
using System;

public class OpeningDoor : PuzzleObject
{
    // Door Animator
    public Animator animator;

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    protected override void onActive()
    {
        animator.SetBool("PressurePlateDown", true); ;
    }

    protected override void onDeactive()
    {
        animator.SetBool("PressurePlateDown", false);
    }
}
