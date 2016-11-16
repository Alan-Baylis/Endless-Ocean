using UnityEngine;
using System.Collections;

/// <summary>
/// An iterface for controlling an NPC to perform actions whent the player enters a collider
/// </summary>
public class ScriptedNPCActionCollider : MonoBehaviour
{
    [SerializeField]
    private NPCBehaviour NPC;

    //Path to Location
    [SerializeField]
    private bool pathToLocation;
    [SerializeField]
    private Transform location;

    //Equip a Weapon
    [SerializeField]
    private bool equipWeapon;
    [SerializeField]
    private CharacterSuper.weaponMounts mount;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private bool isFriendly;

    //set character status
    [SerializeField]
    private bool activeAtEnd;


    //Status
    private bool active;

    // Use this for initialization
    void Start()
    {
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            //path to location
            if (pathToLocation)
            {
                NPC.pathToLocation(location.position);
            }

            //equip a weapon
            if (equipWeapon)
            {
                NPC.equipWeapon(weapon, mount, (isFriendly) ? "PlayerWeapon" : "EnemyWeapon");
            }

            //Set character status
            NPC.active = activeAtEnd;
        }
        active = false;
    }

    /// <summary>
    /// When something enters the collider's area
    /// </summary>
    /// <param name="col">item which entered the collider</param>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && NPC != null)
        {
            active = true;
        }
    }
}
