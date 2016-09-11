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

    protected virtual void Update()
    {
        base.Update();
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
