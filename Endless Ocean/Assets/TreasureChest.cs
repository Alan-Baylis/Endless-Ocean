using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TreasureChest : MonoBehaviour {

    Animator animator;
    private bool opened = false;
    public Image prompt;

    private ParticleSystem chestEffect;

    public AudioClip openSound;

    // Use this for initialization
    void Start () {
        this.animator = this.GetComponent<Animator>();
        this.chestEffect = this.GetComponentInChildren<ParticleSystem>();
    }

    protected void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!opened)
            {
                prompt.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown("e") && (!opened))
            {
                AudioSource.PlayClipAtPoint(this.openSound, this.transform.position, 5.5f);
                StartCoroutine(this.showTreasureOpenEffect());
                prompt.gameObject.SetActive(false);
                opened = true;
                this.animator.SetBool("opened", true);

                List<GameObject> spawnedObjects = ItemSpawner.spawnAnyItems(this.transform, 2);
                for(int i = 0; i < spawnedObjects.Count; i++)
                {
                    if((i % 2) == 0)
                    {
                        Vector3 targetPosition = new Vector3(spawnedObjects[i].transform.position.x + 2, spawnedObjects[i].transform.position.y + 2, 0);
                        spawnedObjects[i].GetComponent<Item>().startFlyingOutOfChest(spawnedObjects[i].transform.position, targetPosition);
                    }
                    else
                    {
                        Vector3 targetPosition = new Vector3(spawnedObjects[i].transform.position.x - 2, spawnedObjects[i].transform.position.y + 2, 0);
                        spawnedObjects[i].GetComponent<Item>().startFlyingOutOfChest(spawnedObjects[i].transform.position, targetPosition);
                    }
                }
            }
        }
    }

    protected void OnTriggerExit(Collider col)
    {
        if(!opened)
        {
            prompt.gameObject.SetActive(false);
        }
    }

    IEnumerator showTreasureOpenEffect()
    {
        int framesToRunFor = 25;
        this.chestEffect.enableEmission = true;
        for(int i = 0; i < framesToRunFor; i++)
        {
            yield return null;
        }
        this.chestEffect.enableEmission = false;
    }
}
