using UnityEngine;
using System.Collections.Generic;
using System;

public class TriggerableGroup : PuzzleObject
{
    [SerializeField]
    private List<PuzzleObject> groupedObjects;

    protected override void onActive()
    {
        foreach(PuzzleObject puzzleObject in groupedObjects){
            puzzleObject.isActive = true;
        }
    }

    protected override void onDeactive()
    {
        foreach (PuzzleObject puzzleObject in groupedObjects)
        {
            puzzleObject.isActive = false;
        }
    }
}
