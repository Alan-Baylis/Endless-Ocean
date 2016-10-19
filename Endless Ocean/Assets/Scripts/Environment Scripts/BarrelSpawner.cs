using UnityEngine;
using System.Collections;

public class BarrelSpawner : MonoBehaviour {

    private GameObject player;
    public Vector3 barrelSpawnPosition;
   // private float nextBarrel;

    public bool hasBarrelSpawned;

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
	}

    /// <summary>
    /// Called as long as the player is inside the barrels collider.
    /// </summary>
    /// <param name="other">The other collider.</param>
    void OnTriggerStay (Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                Debug.Log(hasBarrelSpawned);
                if (!hasBarrelSpawned)
                {
                    GameObject barrel = Instantiate(Resources.Load("Prefabs/Environment/SpawnableBarrel")) as GameObject;
                    barrel.transform.position = barrelSpawnPosition;
                    barrel.GetComponentInChildren<SpawnableBarrel>().spawner = this;
                    this.hasBarrelSpawned = true;
                }
            }
        }
    }

}
