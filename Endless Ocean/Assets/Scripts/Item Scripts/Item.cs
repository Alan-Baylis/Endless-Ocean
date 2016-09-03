using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable()]
public class Item : MonoBehaviour {

    //Used to stack consumables.
    public int itemCount = 1;

    public virtual bool stackable
    {
        get { return false; }
    }

    public string itemName;

    public string description;

    public Sprite itemIcon;

    public virtual bool quickItemEquipable
    {
        get { return false; }
    }
}
