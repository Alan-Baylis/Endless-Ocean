using UnityEngine;
using System.Collections;

public class BarrelSpawner : MonoBehaviour {

    private GameObject player;
    public Vector3 barrelSpawnPosition;
    private float nextBarrel;

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
	}

    /// <summary>
    /// Called once  per frame. Lets the user spawn a barrel if they are close enough.
    /// </summary>
    void Update () {
        if (Vector3.Distance(this.transform.position, this.player.transform.position) < 10)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (Time.time >= nextBarrel)
                {
                    GameObject barrel = Instantiate(Resources.Load("Prefabs/Environment/SpawnableBarrel")) as GameObject;
                    barrel.transform.position = barrelSpawnPosition;
                    nextBarrel = Time.time + 25f;
                }
            }
        }
    }

}
