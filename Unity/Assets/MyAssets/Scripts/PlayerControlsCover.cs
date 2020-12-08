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
        if (cover[chosenCoverIndex].transform.parent.gameObject.GetComponent<FakeCoverMovement>() != null)
        {
            cover[chosenCoverIndex].transform.parent.gameObject.GetComponent<FakeCoverMovement>().SetWake(25);
        }
        //HideWeapons();
    }
    public void UnHide()
    {
        hidden = false;
        anim.SetVar("Hide", false);
        //ShowWeapons();
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
        if (hidden && (!CanHide() || BasicCheckMidAir())&&!anim.a.GetBool("Roll"))
        {
            move.crawl.UnCrawl();
            UnHide();
            //ShowWeapons();
        }
    }
    /*public void HideWeapons(int delta = -18)
    {
        ChangeItemPosition(Gun, delta);
        ChangeItemPosition(Sword, delta);
        ChangeItemPosition(Spear, delta);
        ChangeItemPosition(Arrow, delta);
        ChangeItemPosition(Rod, delta);
        ChangeItemPosition(MagicArtefact, delta);
        ChangeItemPosition(Bow, delta);
        ChangeItemPosition(Helmet, delta);
        ChangeItemPosition(Armor, delta);
        ChangeItemPosition(Pants, delta);
        ChangeItemPosition(Boots, delta);
        ChangeItemPosition(Gloves, delta);
        ChangeItemPosition(Shield, delta);
        ChangeItemPosition(Weapon, delta);
    }

    public void ShowWeapons(int delta = 18)
    {
        ChangeItemPosition(Gun, delta);
        ChangeItemPosition(Sword, delta);
        ChangeItemPosition(Spear, delta);
        ChangeItemPosition(Arrow, delta);
        ChangeItemPosition(Rod, delta);
        ChangeItemPosition(MagicArtefact, delta);
        ChangeItemPosition(Bow, delta);
        ChangeItemPosition(Helmet, delta);
        ChangeItemPosition(Armor, delta);
        ChangeItemPosition(Pants, delta);
        ChangeItemPosition(Boots, delta);
        ChangeItemPosition(Gloves, delta);
        ChangeItemPosition(Shield, delta);
        ChangeItemPosition(Weapon, delta);
    }
    public void ChangeItemPosition(GameObject GO, int delta)
    {
        if (GO != null)
        {
            if (GO.transform.childCount > 0)
            {
                if (GO.transform.GetChild(0).GetComponent<Animator>() != null)
                {
                    GO.transform.GetChild(0).transform.position = new Vector3(GO.transform.GetChild(0).transform.position.x, GO.transform.GetChild(0).transform.position.y, Mathf.Sign(delta)==-1? 1000.0f:0.0f);
                }
            }
            LayerPositionChange(GO.transform, delta);
        }
    }
    public void LayerPositionChange(Transform transform, int delta)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            LayerPositionChange(transform.GetChild(i), delta);
            if (transform.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder += delta;
            }
            if (transform.gameObject.GetComponent<SkinnedMeshRenderer>() != null)
            {
                transform.gameObject.GetComponent<SkinnedMeshRenderer>().sortingOrder += delta;
            }
        }
    }*/
}
