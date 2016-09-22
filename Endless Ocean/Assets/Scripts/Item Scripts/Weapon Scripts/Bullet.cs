using UnityEngine;
using System.Collections;

/// <summary>
/// This class is a bullet that can be fired. It should be instantiated in the fire method of a weapon. When it is created it should be given a speed and damage so that it can interact with the game world.
/// </summary>
public class Bullet : MonoBehaviour
{

    public int damage;
    public float speed;
    public int knockBack;


    // Use this for initialization
    void Start()
    {
        this.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(this.gameObject, 25f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getDamage()
    {
        return damage;
    }

    public int getKnockBack()
    {
        return knockBack;
    }

    protected void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Enemy" && col.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }

}
