using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class that controls the display of the vendor windows when the player is close to the vendor.
/// </summary>
public class ShopNPC : MonoBehaviour {

    private GameObject player;
    public GameObject reforgerPanel;
    public GameObject inventory;

	/// <summary>
    /// Initializes key variables.
    /// </summary>
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.inventory = GameObject.FindGameObjectWithTag("ItemsMenu").transform.Find("Inventory").gameObject;
        this.reforgerPanel = GameObject.FindGameObjectWithTag("VendorPanels").transform.Find("ReforgerPanel").gameObject;
	}
	
	/// <summary>
    /// Called once  per frame. Lets the user open the vendor windows if they are close enough.
    /// </summary>
    /// <param name="other">The other collider.</param>
	void OnTriggerStay (Collider other) {
	    if(other.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                this.reforgerPanel.SetActive(true);
                this.inventory.SetActive(true);
            }
        }
	}

    /// <summary>
    /// Closes the shop menu when the player goes too far from the shop.
    /// </summary>
    /// <param name="other">The other collider.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.reforgerPanel.SetActive(false);
            this.inventory.SetActive(false);
        }
    }
}
