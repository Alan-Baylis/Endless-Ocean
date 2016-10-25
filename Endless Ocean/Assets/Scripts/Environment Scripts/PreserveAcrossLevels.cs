using UnityEngine;
using System.Collections;
/// <summary>
/// This class preserves a gameobject's state across levels.
/// </summary>
public class PreserveAcrossLevels : MonoBehaviour {

    public static PreserveAcrossLevels playerInstance;
    public static PreserveAcrossLevels playerGuiInstance;

    /// <summary>
    /// Runs before the object is created.
    /// 
    /// Makes it so the object is not deleted when scenes are loaded.
    /// </summary>
    void Awake()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            if (PreserveAcrossLevels.playerInstance == null)
            {
                PreserveAcrossLevels.playerInstance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else if (this.gameObject.CompareTag("Player Related"))
        {
            if (PreserveAcrossLevels.playerGuiInstance == null)
            {
                PreserveAcrossLevels.playerGuiInstance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
