using UnityEngine;
using System.Collections;

public class elevatorFirstLevel : MonoBehaviour
{
    bool triggered;
    public Animator elevatorAC;

    // Use this for initialization
    void Start()
    {
        triggered = false;
        this.elevatorAC = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && !triggered)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                triggered = true;
                this.elevatorAC.SetBool("triggered", true);
            }
        }
    }
}
