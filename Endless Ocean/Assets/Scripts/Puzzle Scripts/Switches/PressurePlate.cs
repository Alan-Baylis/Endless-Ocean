using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

    // Is pressure plate down?
    public bool pressurePlateDown = false;
    // Animation controller
    Animator animator;
    // Door reference
    public PuzzleObject triggeredObject;
    // Parent reference
    public GameObject parent;

    public AudioClip buttonPressedSound;
    public AudioClip buttonDepressedSound;

    // Use this for initialization
    void Start () {
        if(buttonDepressedSound == null)
        {
            buttonDepressedSound = Resources.Load("Sounds/Button Off") as AudioClip;
        }
        if(buttonPressedSound == null)
        {
            buttonPressedSound = Resources.Load("Sounds/Button On") as AudioClip;
        }
        this.animator = this.parent.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        this.animator.SetBool("PressurePlateDown", this.pressurePlateDown);

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
            UpdateTriggeredObjects();
            AudioSource.PlayClipAtPoint(this.buttonPressedSound, this.transform.position, 4f);
        }
    }


    void OnTriggerExit(Collider col)
    {
         pressurePlateDown = false;
         UpdateTriggeredObjects();
        AudioSource.PlayClipAtPoint(this.buttonDepressedSound, this.transform.position, 4f);
    }

    void UpdateTriggeredObjects()
    {
        if (triggeredObject != null)
        {
            triggeredObject.isActive = pressurePlateDown;
        }
    }
}
