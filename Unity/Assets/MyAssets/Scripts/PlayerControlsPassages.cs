using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public List<GameObject> Passages;
    public void IncludePassage(GameObject newP)
    {
        Passages.Add(newP);
    }

    public void ExcludePassage(GameObject ExcP)
    {
        if (Passages.Contains(ExcP))
        {
            Passages.Remove(ExcP);
        }
    }

    public void RemoveDoors()
    {
        Passages = new List<GameObject>();
    }
    public void Pass(DirectionType dir)
    {
        GameObject.Find("WorldManager").GetComponent<WorldManagement>().EnterDoor(dir);
    }

    private bool HaveSuchPassage(DirectionType dir)
    {
        for (int i = 0; i < Passages.Count; i++)
        {
            if (Passages[i].GetComponent<Passage>().LocalDirection == dir)
                return true;
        }
        return false;
    }
}
