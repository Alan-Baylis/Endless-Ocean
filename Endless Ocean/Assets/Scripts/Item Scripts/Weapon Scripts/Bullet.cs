using UnityEngine;
using System.Collections;

/// <summary>
/// This class is a bullet that can be fired. It should be instantiated in the fire method of a weapon. When it is created it should be given a speed and damage so that it can interact with the game world.
/// </summary>
public class Bullet : MonoBehaviour
{

    static public AudioClip impactSound;

    public int damage;
    public float speed;
    public int knockBack;
    private ParticleSystem bulletTrail;

    // Use this for initialization
    protected virtual void Start()
    {
        if (impactSound == null)
        {
            impactSound = Resources.Load("Sounds/Enemy Death Explosion Sound") as AudioClip;
        }
        this.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        this.bulletTrail = this.GetComponentInChildren<ParticleSystem>();
        Destroy(this.gameObject, 25f);
        StartCoroutine(hideBulletTrail());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float getStun()
    {
        return knockBack * 0.005f;
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
        if (col.gameObject.tag != "Enemy" && col.gameObject.tag != "Player" && !col.isTrigger)
        {
            Instantiate(Resources.Load("Prefabs/Explosions/explosion_enemy"), this.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(Bullet.impactSound, this.transform.position, 2.5f);
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
