using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void Button_Click()
    {//todo: delete this file
        MenuController Menu = GameObject.Find("Menu").GetComponent<MenuController>(); ;
        switch (name)
        {
            case "Yes":
                Menu.ContinueAction(true);
                break;
            case "No":
                Menu.ContinueAction(false);
                break;
        }
    }
}
