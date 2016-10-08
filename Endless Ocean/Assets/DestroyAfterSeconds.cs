﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This class destory's the obejct after the specfied time.
/// </summary>
public class DestroyAfterSeconds : MonoBehaviour
{

    public int timeToWait;

    /// <summary>
    /// Runs when the object is created. Destroys it after the specified time.
    /// </summary>
    void Start()
    {
        if (timeToWait == 0)
        {
            Destroy(this.gameObject, 5);
        }
        else
        {
            Destroy(this.gameObject, this.timeToWait);
        }
    }
}
