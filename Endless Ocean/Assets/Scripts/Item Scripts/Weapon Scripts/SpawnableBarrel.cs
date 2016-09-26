using UnityEngine;
using System.Collections;

public class SpawnableBarrel : Bullet {

    public BarrelSpawner spawner;

	// Use this for initialization
    protected override void Start () {
	    
	}

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            this.spawner.hasBarrelSpawned = false;
            Destroy(this.gameObject.transform.parent);
        }
    }
}
