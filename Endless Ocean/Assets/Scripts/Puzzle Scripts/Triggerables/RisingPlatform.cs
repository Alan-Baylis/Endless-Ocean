using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The rising platform code that is used for elevators
/// </summary>
public class RisingPlatform : PuzzleObject  {
    // The physical platform
    [SerializeField]
    Transform platform;

    //the start and end points
    [SerializeField]
    Transform endTransform;
    [SerializeField]
    Transform startTransform;

    //platform speed
    [SerializeField]
    float speed;

    //the current direction and destination for the moving platform
    Vector3 direction;
    Transform destination;

    //the elevator sound
    public AudioSource risingPlatformAudioSource;
	
	// Update is called after a fixed period
	void FixedUpdate () {
        if (Vector3.Distance(destination.position, platform.position) > speed * Time.fixedDeltaTime)
            {
                platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction * speed * Time.fixedDeltaTime);
            }
        else
        {
            this.risingPlatformAudioSource.enabled = false;
        }
	}
    /// <summary>
    /// Unity method for drawing visual aids in the developer screen
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startTransform.position, platform.localScale);

        Gizmos.color = Color.red;
        float height = endTransform.position.y - startTransform.position.y;
        Gizmos.DrawWireCube(new Vector3(endTransform.position.x, endTransform.position.y - (height / 2),endTransform.position.z), new Vector3(platform.localScale.x,height+platform.localScale.y,platform.localScale.z));
    }

    /// <summary>
    /// Move the platform towards the destination
    /// </summary>
    /// <param name="dest"> The platform's destination</param>
    void SetDestination(Transform dest)
    {
        destination = dest;
        direction = (destination.position - platform.position).normalized;
        this.risingPlatformAudioSource.enabled = true;
    }

    /// <summary>
    /// onActive code overriden from the PuzzleObject interface
    /// </summary>
    protected override void onActive()
    {
        SetDestination(endTransform);

    }

    /// <summary>
    /// onDeactive code overriden from the PuzzleObject interface
    /// </summary>
    protected override void onDeactive()
    {
        SetDestination(startTransform);
    }
}
