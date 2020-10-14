using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public bool hidden = false;
    public List<GameObject> cover;
    int chosenCoverIndex = 0;
    public void IncludeCover(GameObject newP)
    {
        cover.Add(newP);
    }

    public void ExcludeCover(GameObject ExcP)
    {
        if (cover.Contains(ExcP))
        {
            cover.Remove(ExcP);
        }
    }

    public void ChooseCover()
    {
        chosenCoverIndex = 0;
        for (int i = 0; i < cover.Count; i++)
        {
            chosenCoverIndex = i;
        }
    }

    public void Hide()
    {
            hidden = true;
            anim.SetVar("Hide", true);
        if (cover[chosenCoverIndex].GetComponent<FakeCoverMovement>() != null)
        {
            cover[chosenCoverIndex].GetComponent<FakeCoverMovement>().SetWake(25);
        }
    }
    public void UnHide()
    {
        hidden = false;
        anim.SetVar("Hide", false);
    }

    public bool CanHide()
    {
        if (cover.Count > 0)
        {
            return true;
        }
        return false;
    }

    public void CheckHide()
    {
        if (hidden && (!CanHide() || BasicCheckMidAir()))
        {
            UnHide();
        }
    }
}
