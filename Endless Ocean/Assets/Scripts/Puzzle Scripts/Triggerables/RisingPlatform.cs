using UnityEngine;
using System.Collections;
using System;

public class RisingPlatform : PuzzleObject  {
    [SerializeField]
    Transform platform;

    [SerializeField]
    Transform endTransform;

    [SerializeField]
    Transform startTransform;

    [SerializeField]
    float speed;

    Vector3 direction;
    Transform destination;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Vector3.Distance(destination.position, platform.position) > speed * Time.fixedDeltaTime)
            {
                platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction * speed * Time.fixedDeltaTime);
            }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startTransform.position, platform.localScale);

        Gizmos.color = Color.red;
        float height = endTransform.position.y - startTransform.position.y;
        Gizmos.DrawWireCube(new Vector3(endTransform.position.x, endTransform.position.y - (height / 2),endTransform.position.z), new Vector3(platform.localScale.x,height+platform.localScale.y,platform.localScale.z));
    }

    void SetDestination(Transform dest)
    {
        destination = dest;
        direction = (destination.position - platform.position).normalized;
    }

    protected override void onActive()
    {
        SetDestination(endTransform);
    }

    protected override void onDeactive()
    {
        SetDestination(startTransform);
    }
}
