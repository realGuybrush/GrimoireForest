using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public GameObject talks;
    public void IncludeTalk(GameObject newP)
    {
        talks =newP;
    }

    public void ExcludeTalk(GameObject ExcP)
    {
        if (talks == ExcP)
        {
            talks=null;
        }
    }

    public void StartTalk()
    {
        ShowHideMenu(inMenu, false, false, false, false,  false, !inMenu);
    }

    public bool IsTalking()
    {
        return Talks.activeSelf;
    }

    public void UpdateGlobalTalks()
    {
        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        GameObject.Find("WorldManager").GetComponent<WorldManagement>().UpdateGlobalTalks(WM.EntityNumberByName(talks.GetComponent<NPCBehaviour>().name),new Vector3(WM.PlayerXMap, WM.PlayerYMap), talks.GetComponent<NPCBehaviour>().talk);
    }
}
