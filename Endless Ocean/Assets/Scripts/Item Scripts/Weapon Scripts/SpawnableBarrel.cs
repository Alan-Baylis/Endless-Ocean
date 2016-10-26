using UnityEngine;
using System.Collections;

public class SpawnableBarrel : Bullet {

    public BarrelSpawner spawner;

	// Use this for initialization
    protected override void Start () {
	    
	}

    protected void OnTriggerEnter(Collider col)
    {
        Debug.Log("Not Inside");
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Inside");
            this.spawner.hasBarrelSpawned = false;
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
