using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TreasureChest : MonoBehaviour {

    Animator animator;
    private bool opened = false;
    public Image prompt;

    // Use this for initialization
    void Start () {
        this.animator = this.GetComponent<Animator>();
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
}
