using UnityEngine;
using System.Collections;

public abstract class PuzzleObject : MonoBehaviour
{
    [SerializeField]
    protected bool active;

    private bool inverted;

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

    protected abstract void onActive();

    protected abstract void onDeactive();
}
