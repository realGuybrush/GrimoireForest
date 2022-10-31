using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    [SerializeField]
    private Button newGame, resume, quit, exit;

    public GameObject NewGameSheet;
    public GameObject SaveLoadSheet;
    public GameObject OptionsSheet;
    public GameObject Messager;
    public bool inGame = false;
    private string continueAction = "";
    private int SlotIndex = 0;

    public Action OnGameGenerationLaunch = delegate { };

    void ShowMessage(string message) {
        Messager.SetActive(true);
        Messager.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = message;
    }

    public void NewGame() {
        NewGameSheet.SetActive(true);
    }

    public void LaunchNewGame() {
        Escape();
        FlipMenu(false);
        inGame = true;
        OnGameGenerationLaunch?.Invoke();
    }

    public void Resume() {
        CloseAll();
        GameObject.Find("Player").GetComponent<PlayerControls>().ShowHideMenu(true, false);
    }

    public void CloseAll() {
        NewGameSheet.SetActive(false);
        SaveLoadSheet.SetActive(false);
        OptionsSheet.SetActive(false);
        continueAction = "";
    }

    public void Load(int slotIndex = 0) {
        SlotIndex = slotIndex;
        SaveLoadSheet.SetActive(true);
        continueAction = "Load";
        if (inGame) {
            ShowMessage("Do you really want to load?\nAll current progress will be lost.");
        } else {
            ContinueAction(true);
            Resume();
        }
    }

    public void Save(int slotIndex = 0) {
        //SlotIndex = slotIndex;
        SaveLoadSheet.SetActive(true);
        continueAction = "Save";
        Debug.Log("Saving.");
        //if(WM.SlotOccupied(slotIndex))
        //{
        //   ShowMessage("Overwrite this save file?");
        //}
        //else
        //{
        ContinueAction(true);
        //}
    }

    public void Options() {
        OptionsSheet.SetActive(true);
    }

    public void Quit() {
        ShowMessage("Quit to Title Screen?");
        continueAction = "Quit";
    }

    public void Exit() {
        ShowMessage("Exit The Game?");
        continueAction = "Exit";
    }

    public void ContinueAction(bool answer) {
        Messager.SetActive(false);
        if (!answer) {
            continueAction = "";
        } else {
            switch (continueAction) {
                case "Load":
                    //WM.Load(SlotIndex);
                    FlipMenu(false);
                    GameObject.Find("WorldManager").GetComponent<WorldManagement>().DeleteCorridor();
                    GameObject.Find("WorldManager").GetComponent<WorldManagement>().LoadGame();
                    inGame = true;
                    Resume();
                    break;
                case "Save":
                    GameObject.Find("WorldManager").GetComponent<WorldManagement>().SaveGame(); //SlotIndex
                    Debug.Log("Saved.");
                    break;
                case "Quit":
                    //WM.DestroyWorld();
                    FlipMenu(true);
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

    private void FlipMenu(bool mainMenu) {
        newGame.gameObject.SetActive(mainMenu);
        resume.gameObject.SetActive(!mainMenu);
        quit.gameObject.SetActive(!mainMenu);
        exit.gameObject.SetActive(mainMenu);
    }

    public bool Escape() {
        if (Messager.activeSelf) {
            Messager.SetActive(false);
            ContinueAction(false);
        } else {
            if (NewGameSheet.activeSelf || SaveLoadSheet.activeSelf || OptionsSheet.activeSelf) {
                CloseAll();
            } else {
                if (inGame) {
                    return true;
                } else {
                    Exit();
                }
            }
        }
        return false;
    }
}
