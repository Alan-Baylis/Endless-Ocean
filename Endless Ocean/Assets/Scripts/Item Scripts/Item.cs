using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable()]
public class Item : MonoBehaviour {

    bool flyingOutOfChest = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    //Used to stack consumables.
    public int itemCount = 1;

    public virtual bool stackable
    {
        get { return false; }
    }

    protected void Update()
    {
        if (flyingOutOfChest)
        {
            if(this.transform.position.y >= targetPosition.y)
            {
                flyingOutOfChest = false;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

            }
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, .4f);
            Debug.Log(Time.deltaTime);
        }
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
}
