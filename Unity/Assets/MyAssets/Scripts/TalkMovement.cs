using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkMovement : MonoBehaviour
{
    bool playerOrNot = false;
    Talk talk;
    int curIndex = 0;
    int chosenLine = 0;
    int maxWait;
    int curWait = 0;
    int waitPerSymbol = 10;
    bool choosing = false;
    bool rotating = false;
    int rotateStep = 0;
    List<int> linesToChoose = new List<int>();
    GameObject playerTalkOrb;
    GameObject otherTalkOrb;

    public int maxOrbWidth = 240;
    int maxSymbolsInLine = 0;
    int fontSize = 14;
    // Start is called before the first frame update
    void Start()
    {
        fontSize = curIndex;
        playerTalkOrb = GameObject.Instantiate(GameObject.Find("WorldManager").GetComponent<WorldManagement>().TalkCloudPrefab);
        otherTalkOrb = GameObject.Instantiate(GameObject.Find("WorldManager").GetComponent<WorldManagement>().TalkCloudPrefab);
        fontSize = playerTalkOrb.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize;
        playerTalkOrb.transform.parent = this.transform;
        otherTalkOrb.transform.parent = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (talk == null)
            return;
        if (!rotating)
        {
            if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Space))
            {
                NextLine();
            }
            if (choosing)
            {
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                    //SwitchUp();
                }
                if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                {
                    //SwitchDown();
                }
            }
            else
            {
                if (curWait == 0)
                {
                    NextLine();
                }
                else
                {
                    curWait--;
                }
            }
        }
        else
        {
            if (rotateStep == 0)
            {
                rotating = false;
            }
            else
            {
                rotateStep--;
            }
        }
    }

    public void NextLine()
    {
        if (playerOrNot)
        {
            //let player choose line
            ResetPlayer(new List<string>(){ "asdfasdfa", "asdfasd asdfasdfasd asdfasdfasdfas fasdfas", "afdafsdfas"});
            choosing = true;
        }
        else
        {
            //choose randomly or by some system
            ResetOther("sdfasd fasdfasdfasdf dfasdfasd asdfasd asdfasdfa sdaf");
            //RandomChoose();
        }
        maxWait = waitPerSymbol * chosenLine;
        curWait = maxWait;
    }



    public void HideLine()
    {

    }

    public void SetTalk(Talk t)
    {
        talk = t;
    }
    public void ShowHide()
    {

    }
    public void ResetOther(string line)
    {
        int newWidth = 0;
        int newHeight = fontSize+4;
        string newLine = "";
        string newLinePart = "";
        int symbolsInLine = 0;
        maxSymbolsInLine = maxOrbWidth / ((fontSize) / 2) - 2;
        foreach (string part in line.Split(' '))
        {
            if ((newLinePart + part).Length < maxSymbolsInLine)
            {
                newLinePart += part+" ";
            }
            else
            {
                newLine += newLinePart + "\n";
                symbolsInLine = (symbolsInLine>newLinePart.Length)? symbolsInLine:newLinePart.Length;
                newLinePart = part;
                newHeight += fontSize + 2;
            }
        }
        newLine += newLinePart;
        if (newLine.Length < maxSymbolsInLine)
        {
            newWidth = (newLine.Length) * (fontSize/2);
        }
        else
        {
            newWidth = (symbolsInLine) * (fontSize/2);
        }
        otherTalkOrb.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth + 2*fontSize, newHeight + fontSize);
        otherTalkOrb.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = newLine;
        otherTalkOrb.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, newHeight);
        //otherTalkOrb.transform.GetChild(1).GetComponent<RectTransform>() = newWidth;
        //playerTalkOrb.transform.GetChild(1).GetComponent<RectTransform>() = new Vector3(newWidth, newHeight);
        //.Set(0,0,newWidth, newHeight);
        //otherTalkOrb.GetComponent<RectTransform>().anchorMin = new Vector2(-newWidth / 2, -newHeight / 2);
        //otherTalkOrb.GetComponent<RectTransform>() = newWidth;
    }


    public void ResetPlayer(List<string> line)
    {
        int newWidth = 0;
        int newHeight = fontSize*3 + 8;
        string newLine = "";
        string newLinePart = "";
        int symbolsInLine = 0;
        maxSymbolsInLine = maxOrbWidth / ((fontSize) / 2) - 2;
        foreach (string part in line[1].Split(' '))
        {
            if ((newLinePart + part).Length < maxSymbolsInLine)
            {
                newLinePart += part;
            }
            else
            {
                newLine += newLinePart + "\n";
                symbolsInLine = (symbolsInLine > newLinePart.Length) ? symbolsInLine : newLinePart.Length;
                newLinePart = part;
                newHeight += fontSize + 2;
            }
        }
        newLine += newLinePart;
        if (newLine.Length < maxSymbolsInLine)
        {
            newWidth = (newLine.Length) * ((fontSize)/2);
        }
        else
        {
            newWidth = (symbolsInLine) * ((fontSize)/2);
        }
        if (line[0].Length < maxSymbolsInLine)
        {
            playerTalkOrb.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = line[0];
        }
        else
        {
            playerTalkOrb.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = line[0].Substring(0, maxSymbolsInLine - 4) + "...";
        }
        playerTalkOrb.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, fontSize);
        playerTalkOrb.transform.GetChild(0).transform.position = new Vector3(playerTalkOrb.transform.GetChild(0).transform.position.x, -(newHeight/2+5+fontSize/2));
        playerTalkOrb.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = newLine;
        playerTalkOrb.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, newHeight);
        //otherTalkOrb.transform.GetChild(1).GetComponent<RectTransform>() = newWidth;
        if (line[2].Length < maxSymbolsInLine)
        {
            playerTalkOrb.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = line[2];
        }
        else
        {
            playerTalkOrb.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = line[2].Substring(0, maxSymbolsInLine - 4) + "...";
        }
        playerTalkOrb.transform.GetChild(2).transform.position = new Vector3(playerTalkOrb.transform.GetChild(2).transform.position.x, newHeight / 2 + 5 + fontSize / 2);
        playerTalkOrb.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, fontSize);
        playerTalkOrb.GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth+2*fontSize, newHeight+ fontSize);
        //otherTalkOrb.GetComponent<RectTransform>() = newWidth;
        playerTalkOrb.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, fontSize);
    }
}
