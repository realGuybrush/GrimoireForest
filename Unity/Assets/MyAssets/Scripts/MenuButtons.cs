using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void Button_Click()
    {
        MenuController Menu = GameObject.Find("Menu").GetComponent<MenuController>(); ;
        switch (name)
        {
            case "NewGame":
                Menu.NewGame();
                break;
            case "Resume":
                Menu.Resume();
                break;
            case "Save":
                Menu.Save();
                break;
            case "Load":
                Menu.Load();
                break;
            case "Options":
                Menu.Options();
                break;
            case "Quit":
                Menu.Quit();
                break;
            case "Exit":
                Menu.Exit();
                break;
            case "Yes":
                Menu.ContinueAction(true);
                break;
            case "No":
                Menu.ContinueAction(false);
                break;
            default:
                break;
        }
    }
}
