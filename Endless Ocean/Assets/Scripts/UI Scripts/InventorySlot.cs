using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    public Item item;
    private Image itemImage;
    public int slotNumber;
    public Inventory inventory;

    /// <summary>
    /// This function runs when the user clicks on the slot.
    /// </summary>
    /// <param name="eventData">The event data from the click.</param>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }

    /// <summary>
    /// This function runs when the user mouses over a slot.
    /// </summary>
    /// <param name="eventData">The data from the mouse over.</param>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Over");
    }

    /// <summary>
    /// Runs when the Gameobject is first created. Initializes key variables.
    /// </summary>
    void Start()
    {
        this.inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        this.itemImage = transform.FindChild("ItemIcon").gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //If this slot is holdin an item - show it.
        if (this.inventory.items[this.slotNumber].itemName != null)
        {
            this.itemImage.enabled = true;
            this.itemImage.sprite = this.inventory.items[this.slotNumber].itemIcon;
        }
        else
        {
            this.itemImage.enabled = false;
        }
    }
}
