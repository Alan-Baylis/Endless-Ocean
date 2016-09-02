using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

    // Is pressure plate down?
    public bool pressurePlateDown = false;
    // Animation controller
    Animator animator;
    // Door reference
    public GameObject triggeredObject;
    // Parent reference
    public GameObject parent;
    // Door Animator
    Animator doorAnimator;

    // Use this for initialization
    void Start () {
        this.animator = this.parent.GetComponent<Animator>();
        this.doorAnimator = this.triggeredObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        this.animator.SetBool("PressurePlateDown", this.pressurePlateDown);
        this.doorAnimator.SetBool("PressurePlateDown", this.pressurePlateDown);
        if (pressurePlateDown)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "WeightCube" || col.gameObject.tag == "Player")
        {
            pressurePlateDown = true;
        }
    }


    void OnTriggerExit(Collider col)
    {
         pressurePlateDown = false;
    }
}
