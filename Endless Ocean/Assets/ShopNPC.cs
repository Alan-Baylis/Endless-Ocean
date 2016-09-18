using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class that controls the display of the vendor windows when the player is close to the vendor.
/// </summary>
public class ShopNPC : MonoBehaviour {

    private GameObject player;
    public GameObject vendorPanels;
    public GameObject inventory;

	/// <summary>
    /// Initializes key variables.
    /// </summary>
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
	}
	
	/// <summary>
    /// Called once  per frame. Lets the user open the vendor windows if they are close enough.
    /// </summary>
	void Update () {
	    if(Vector3.Distance(this.transform.position, this.player.transform.position) < 5)
        {
            if (Input.GetButtonDown("Interact"))
            {
                this.vendorPanels.SetActive(!this.vendorPanels.activeSelf);
                this.inventory.SetActive(!this.inventory.activeSelf);
            }
        }
        else
        {
            this.vendorPanels.SetActive(false);
        }
	}
}
