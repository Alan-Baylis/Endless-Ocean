using UnityEngine;
using System.Collections;

public class coreRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    public Vector3 direction;
    public int speed;

	// Update is called once per frame
	void Update () {

        this.transform.Rotate(direction, speed * Time.deltaTime);
    }
}
