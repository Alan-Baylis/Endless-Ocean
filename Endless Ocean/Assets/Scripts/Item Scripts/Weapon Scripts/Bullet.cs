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
    private ParticleSystem bulletTrail;

    // Use this for initialization
    protected virtual void Start()
    {
        this.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        this.bulletTrail = this.GetComponentInChildren<ParticleSystem>();
        Destroy(this.gameObject, 25f);
        StartCoroutine(hideBulletTrail());
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

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Enemy" && col.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator hideBulletTrail()
    {
        for(int i = 0; i < 5; i++)
        {
            this.bulletTrail.emissionRate--;
            yield return new WaitForSeconds(.15f);
        }
        this.bulletTrail.enableEmission = false;
    }

}
