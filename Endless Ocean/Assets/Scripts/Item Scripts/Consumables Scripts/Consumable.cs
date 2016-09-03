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
	
	// Update is called once per frame
	void Update () {
	
	}
}
