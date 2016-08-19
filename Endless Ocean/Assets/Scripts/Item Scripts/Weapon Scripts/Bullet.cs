using UnityEngine;
using System.Collections;

/// <summary>
/// This class is a bullet that can be fired. It should be instantiated in the fire method of a weapon. When it is created it should be given a speed and damage so that it can interact with the game world.
/// </summary>
public class Bullet : MonoBehaviour { 

    public float damage;
    public float speed;


	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(this.gameObject, 8f);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
