using UnityEngine;
using System.Collections;

/// <summary>
/// Puzzle object superclass, provides an interface for all puzzle objects
/// </summary>
public abstract class PuzzleObject : MonoBehaviour
{
    //active status
    [SerializeField]
    protected bool active;

    //is puzzle status inverted (i.e. on is closed)
    private bool inverted;

    /// <summary>
    /// The active status public modifier
    /// get; gets the active
    /// set; sets the active figuring in the inverted status
    /// </summary>
    public bool isActive
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
            if (active ^ inverted)
            {
                onActive();
            }
            else
            {
                onDeactive();
            }
        }
    }

    /// <summary>
    /// Toggles the current active staus by changing the inverted boolean
    /// By changing the inverted not the active means this doesn't break the button links
    /// </summary>
    public void toggle()
    {
        inverted = !inverted;


        if (active ^ inverted)
        {
            onActive();
        }
        else
        {
            onDeactive();
        }
    }

    // Use this for initialization
    public void Start()
    {
        inverted = false;
        isActive = active;
        inverted = active;
    }

    /// <summary>
    /// action on activate
    /// </summary>
    protected abstract void onActive();

    /// <summary>
    /// action on deactivate
    /// </summary>
    protected abstract void onDeactive();
}
