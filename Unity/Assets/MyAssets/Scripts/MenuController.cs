using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject NewGameSheet;
    public GameObject SaveLoadSheet;
    public GameObject OptionsSheet;
    public GameObject Messager;
    public bool inGame = false;
    private string continueAction = "";
    private int SlotIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowMessage(string message)
    {
        Messager.SetActive(true);
        Messager.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = message;
    }
    public void NewGame()
    {
        NewGameSheet.SetActive(true);
    }
    public void Resume()
    {
        CloseAll();
        GameObject.Find("Player").GetComponent<PlayerControls>().ShowHideMenu(true, false);
    }
    public void CloseAll()
    {
        NewGameSheet.SetActive(false);
        SaveLoadSheet.SetActive(false);
        OptionsSheet.SetActive(false);
        continueAction = "";
    }
    public void Load(int slotIndex = 0)
    {
        SlotIndex = slotIndex;
        SaveLoadSheet.SetActive(true);
        continueAction = "Load";
        if (inGame)
        {
            ShowMessage("Do you really want to load?\nAll current progress will be lost.");
        }
        else
        {
            ContinueAction(true);
            Resume();
        }
    }
    public void Save(int slotIndex = 0)
    {
        //SlotIndex = slotIndex;
        SaveLoadSheet.SetActive(true);
        continueAction = "Save";
        //if(WM.SlotOccupied(slotIndex))
        //{
        //   ShowMessage("Overwrite this save file?");
        //}
        //else
        //{
        //   ContinueAction(true);
        //}
    }
    public void Options()
    {
        OptionsSheet.SetActive(true);
    }
    public void Quit()
    {
        ShowMessage("Quit to Title Screen?");
        continueAction = "Quit";
    }
    public void Exit()
    {
        ShowMessage("Exit The Game?");
        continueAction = "Exit";
    }

    public void ContinueAction(bool answer)
    {
        Messager.SetActive(false);
        if (!answer)
        {
            continueAction = "";
        }
        else
        {
            GameObject G, F;
            switch (continueAction)
            {
                case "Load":
                    G = GameObject.Find("NewGame");
                    if (G != null)
                    {
                        G.name = "Resume";
                        G.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Resume";
                    }
                    F = GameObject.Find("Exit");
                    if (F != null)
                    {
                        F.name = "Quit";
                        F.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Quit";
                    }
                    //WM.Load(SlotIndex);
                    inGame = true;
                    Resume();
                    break;
                case "Save":
                    //WM.Save(SlotIndex);
                    break;
                case "Quit":
                    //WM.DestroyWorld();
                    G = GameObject.Find("Resume");
                    if (G != null)
                    {
                        G.name = "NewGame";
                        G.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "New Game";
                    }
                    F = GameObject.Find("Quit");
                    if (F != null)
                    {
                        F.name = "Exit";
                        F.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Exit";
                    }
                    inGame = false;
                    break;
                case "Exit":
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }

    public bool Escape()
    {
        if (Messager.activeSelf)
        {
            Messager.SetActive(false);
            ContinueAction(false);
        }
        else
        {
            if (NewGameSheet.activeSelf || SaveLoadSheet.activeSelf || OptionsSheet.activeSelf)
            {
                CloseAll();
            }
            else
            {
                if (inGame)
                {
                    return true;
                }
                else
                {
                    Exit();
                }
            }
        }
        return false;
    }
}
