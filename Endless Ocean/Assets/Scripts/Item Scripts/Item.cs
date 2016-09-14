using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
/// This is the super class for all items it contains functionality that is shared between all types ofitems such as flying out of chests and shoing tooltips.
/// 
/// IMPORTANT: When overriding the Start and Update methods of this class. Be sure to call the base vesion as well so that the base functionality is not lost.
/// </summary>
[System.Serializable()]
public class Item : MonoBehaviour{

    bool flyingOutOfChest = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public GameObject tooltip;

    //Used to stack consumables.
    public int itemCount = 1;

    public virtual bool stackable
    {
        get { return false; }
    }

    protected virtual void Start()
    {
        this.tooltip = Instantiate(Resources.Load("Prefabs/UI/ItemTooltip"), new Vector3(), Quaternion.identity) as GameObject;
        this.tooltip.transform.GetChild(0).GetComponent<Text>().text = this.itemName;
        this.tooltip.transform.parent = this.gameObject.transform;
        this.tooltip.SetActive(false);
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    protected virtual void Update()
    {
        if (flyingOutOfChest)
        {
            this.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation);
            if (this.transform.position.y >= targetPosition.y)
            {
                flyingOutOfChest = false;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                this.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ);

            }
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, .4f);
            Debug.Log(Time.deltaTime);
        }
    }

    protected void LateUpdate()
    {
        this.repositionTooltip();
    }

    public string itemName;

    public string description;

    public Sprite itemIcon;

    /// <summary>
    /// Returns a bool indicating whether or not the Item can be placed in a quick item slot.
    /// </summary>
    public virtual bool quickItemEquipable
    {
        get { return false; }
    }

    public void startFlyingOutOfChest(Vector3 startPosition, Vector3 targetPosition)
    {
        this.startPosition = startPosition;
        this.targetPosition = targetPosition;
        this.flyingOutOfChest = true;
    }

    /// <summary>
    /// Reposition the items tooltip to be above the item.
    /// </summary>
    public void repositionTooltip()
    {
        if (this.tooltip.activeSelf)
        {
            this.tooltip.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
            this.tooltip.transform.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Shows the items tooltip when the player mouses over it.
    /// </summary>
    void OnMouseEnter()
    {
        try { 
            if (!(this.transform.parent.gameObject.CompareTag("WeaponMount"))) {
                this.tooltip.SetActive(true);
            }
        }
        catch (NullReferenceException ex)
        {
            this.tooltip.SetActive(true);
        }

    }

    /// <summary>
    /// Shows the items tooltip when the player mouses off of it.
    /// </summary>
    void OnMouseExit()
    {
        this.tooltip.SetActive(false);
    }
}
