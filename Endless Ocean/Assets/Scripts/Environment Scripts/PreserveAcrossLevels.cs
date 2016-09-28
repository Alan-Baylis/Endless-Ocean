using UnityEngine;
using System.Collections;
/// <summary>
/// This class preserves a gameobject's state across levels.
/// </summary>
public class PreserveAcrossLevels : MonoBehaviour {

    /// <summary>
    /// Runs before the object is created.
    /// 
    /// Makes it so the object is not deleted when scenes are loaded.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
