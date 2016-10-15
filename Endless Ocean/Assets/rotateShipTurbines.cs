using UnityEngine;
using System.Collections;

public class rotateShipTurbines : MonoBehaviour {

    public Rigidbody shipBody;
    float factor = 15; // degrees per meter
    float bfactor = -10; // degrees per meter

    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Rotate(factor, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.Rotate(bfactor, 0, 0);
        }


        // Use this for initialization
    }

    void Start ()
    {
	}

}
