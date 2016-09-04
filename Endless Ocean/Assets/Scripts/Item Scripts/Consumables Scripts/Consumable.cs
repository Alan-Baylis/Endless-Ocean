using UnityEngine;
using System.Collections;

public class Consumable : Item {

    public override bool stackable
    {
        get { return true; }
    }

    public override bool quickItemEquipable
    {
        get { return true; }
    }

    // Use this for initialization
    void Start () {
	
	}

    protected virtual void Update()
    { 
        if (this.itemCount < 1)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void use(PlayerController player)
    {
        return;
    }

    
}
