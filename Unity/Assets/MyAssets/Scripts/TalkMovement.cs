using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkMovement : MonoBehaviour
{
    bool playerOrNot = false;
    Talk talk;
    int curIndex = -1;
    int chosenLine = 0;
    int maxWait=200;
    int curWait = 0;
    int waitPerSymbol = 20;
    bool choosing = false;
    List<int> linesToChoose = new List<int>();
    GameObject playerTalkOrb;
    GameObject otherTalkOrb;
    Vector3 otherPos;
    Vector3 playerPos;

    bool rotating = false;
    int rotateStep = 0;
    float rotateDelta = 0.0f;
    int maxRotateSteps = 30;

    public int maxOrbWidth = 200;
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
        playerTalkOrb.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0.0f, 0, false);
        playerTalkOrb.transform.GetChild(4).localScale = new Vector3(0.0f, 0.0f, 0.0f);
        playerTalkOrb.transform.GetChild(5).localScale = new Vector3(0.0f, 0.0f, 0.0f);
        otherTalkOrb.transform.GetChild(4).localScale = new Vector3(0.0f, 0.0f, 0.0f);
        otherTalkOrb.transform.GetChild(5).localScale = new Vector3(0.0f, 0.0f, 0.0f);
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
                if (!choosing)
                {
                    NextLine();
                }
                else
                {
                    //ResetPlayer(new List<string>() { talk.Phrases[linesToChoose[linesToChoose.Count - 1]].BeautifulLine, talk.Phrases[linesToChoose[0]].BeautifulLine, talk.Phrases[linesToChoose[1 % linesToChoose.Count]].BeautifulLine }, linesToChoose.Count == 1);
                    chosenLine = linesToChoose[0];
                    playerOrNot = !playerOrNot;
                    //NextLine();
                    ResetPlayer(new List<string>() { talk.Phrases[linesToChoose[0]].BeautifulLine }, true);
                    choosing =false;
                    Act();
                }
            }
            if (choosing)
            {
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                    SwitchUp();
                }
                if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                {
                    SwitchDown();
                }
            }
            else
            {
                if (curWait == 0)
                {
                    NextLine();
                    curWait--;
                }
                else
                {
                    if(curWait>0)
                    curWait--;
                }
            }
        }
        else
        {
            if (rotateStep == 0)
            {
                EndStep();
                UpdateIcons();
            }
            else
            {
                CommenceRotateStep();
            }
        }
    }

    void EndStep()
    {
        int temp = linesToChoose[0];
        rotating = false;
        playerTalkOrb.transform.GetChild(0).transform.position += new Vector3(0.0f, -rotateDelta* maxRotateSteps, 0.0f);
        playerTalkOrb.transform.GetChild(1).transform.position += new Vector3(0.0f, -rotateDelta* maxRotateSteps, 0.0f);
        playerTalkOrb.transform.GetChild(2).transform.position += new Vector3(0.0f, -rotateDelta* maxRotateSteps, 0.0f);
        if (rotateDelta > 0)
        {
            for (int i = 1; i < linesToChoose.Count; i++)
            {
                linesToChoose[i - 1] = linesToChoose[i];
            }
            linesToChoose[linesToChoose.Count - 1] = temp;
        }
        else
        {
            temp = linesToChoose[linesToChoose.Count - 1];
            for (int i = linesToChoose.Count-2; i > -1; i--)
            {
                linesToChoose[i+1] = linesToChoose[i];
            }
            linesToChoose[0] = temp;
        }
        rotateDelta = 0.0f;
        ResetPlayer(new List<string>() { talk.Phrases[linesToChoose[linesToChoose.Count - 1]].BeautifulLine, talk.Phrases[linesToChoose[0]].BeautifulLine, talk.Phrases[linesToChoose[1 % linesToChoose.Count]].BeautifulLine }, linesToChoose.Count == 1);
        playerTalkOrb.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(1.0f, 0, false);
        playerTalkOrb.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(1.0f, 0, false);
        playerTalkOrb.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0.0f, 0, false);
        UpdateIcons();
    }

    /*void UpdateIconsTransparency(float tradeTransp, float exitTransp, int tradeTime, int exitTime)
    {
        for (int i = 0; i < 4; i++)
        {
            if (talk.Phrases[linesToChoose[i]].action == 't')
            {
                playerTalkOrb.transform.GetChild(4).transform.position = new Vector3(playerTalkOrb.transform.GetChild(i).transform.position.x+ playerTalkOrb.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.x/2, playerTalkOrb.transform.GetChild(i).transform.position.y, 0.0f);
            }
            if (talk.Phrases[linesToChoose[i]].action == 'e')
            {
                playerTalkOrb.transform.GetChild(5).transform.position = new Vector3(playerTalkOrb.transform.GetChild(i).transform.position.x + playerTalkOrb.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.x / 2, playerTalkOrb.transform.GetChild(i).transform.position.y, 0.0f);
            }
        }
    }*/
    void UpdateIcons()//Positions
    {
        float a;
        bool zeroT = true;
        bool zeroE = true;
        float posDelta = (playerTalkOrb.GetComponent<RectTransform>().sizeDelta.x - fontSize) / 2;
        playerTalkOrb.transform.GetChild(4).localScale = new Vector3(1.0f, 1.0f, 0.0f);
        playerTalkOrb.transform.GetChild(5).localScale = new Vector3(1.0f, 1.0f, 0.0f);
        if (talk.Phrases[linesToChoose[linesToChoose.Count-1]].action == 't')
        {
            playerTalkOrb.transform.GetChild(4).transform.position = new Vector3(playerTalkOrb.transform.GetChild(0).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(0).transform.position.y, 0.0f);
            if (rotateDelta > 0)
            {
                a = (float)rotateStep/ (float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(4).transform.localScale = new Vector3(a, a, 0.0f);
            }
            zeroT = false;
        }
        if (talk.Phrases[linesToChoose[linesToChoose.Count - 1]].action == 'e')
        {
            playerTalkOrb.transform.GetChild(5).transform.position = new Vector3(playerTalkOrb.transform.GetChild(0).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(0).transform.position.y, 0.0f);
            if (rotateDelta > 0)
            {
                a = (float)rotateStep / (float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(5).transform.localScale = new Vector3(a, a, 0.0f);
            }
            zeroE = false;
        }

        if (talk.Phrases[linesToChoose[0]].action == 't')
        {
            playerTalkOrb.transform.GetChild(4).transform.position = new Vector3(playerTalkOrb.transform.GetChild(1).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(1).transform.position.y, 0.0f);
            zeroT = false;
        }
        if (talk.Phrases[linesToChoose[0]].action == 'e')
        {
            playerTalkOrb.transform.GetChild(5).transform.position = new Vector3(playerTalkOrb.transform.GetChild(1).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(1).transform.position.y, 0.0f);
            zeroE = false;
        }

        if (talk.Phrases[linesToChoose[1% linesToChoose.Count]].action == 't')
        {
            playerTalkOrb.transform.GetChild(4).transform.position = new Vector3(playerTalkOrb.transform.GetChild(2).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(2).transform.position.y, 0.0f);
            if (rotateDelta < 0)
            {
                a = (float)rotateStep / (float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(4).transform.localScale = new Vector3(a, a, 0.0f);
            }
            zeroT = false;
        }
        if (talk.Phrases[linesToChoose[1% linesToChoose.Count]].action == 'e')
        {
            playerTalkOrb.transform.GetChild(5).transform.position = new Vector3(playerTalkOrb.transform.GetChild(2).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(2).transform.position.y, 0.0f);
            if (rotateDelta < 0)
            {
                a = (float)rotateStep / (float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(5).transform.localScale = new Vector3(a, a, 0.0f);
            }
            zeroE = false;
        }
        if (rotateDelta < 0)
        {
            if ((talk.Phrases[linesToChoose[2 % linesToChoose.Count]].action == 't')&&(zeroT))
            {
                playerTalkOrb.transform.GetChild(4).transform.position = new Vector3(playerTalkOrb.transform.GetChild(3).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(3).transform.position.y, 0.0f);
                a = 1.0f - (float)rotateStep / (float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(4).transform.localScale = new Vector3(a, a, 0.0f);
                zeroT = false;
            }
            if ((talk.Phrases[linesToChoose[2 % linesToChoose.Count]].action == 'e')&&(zeroE))
            {
                playerTalkOrb.transform.GetChild(5).transform.position = new Vector3(playerTalkOrb.transform.GetChild(3).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(3).transform.position.y, 0.0f);

                a = 1.0f - (float)rotateStep / (float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(5).transform.localScale = new Vector3(a, a, 0.0f);
                zeroE = false;
            }
        }
        if (rotateDelta > 0)
        {
            if ((talk.Phrases[linesToChoose[(linesToChoose.Count -2) % linesToChoose.Count]].action == 't') && (zeroT))
            {
                playerTalkOrb.transform.GetChild(4).transform.position = new Vector3(playerTalkOrb.transform.GetChild(3).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(3).transform.position.y, 0.0f);
                a = 1.0f - (float)rotateStep / (float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(4).transform.localScale = new Vector3(a, a, 0.0f);
                zeroT = false;
            }
            if ((talk.Phrases[linesToChoose[(linesToChoose.Count -2) % linesToChoose.Count]].action == 'e') && (zeroE))
            {
                playerTalkOrb.transform.GetChild(5).transform.position = new Vector3(playerTalkOrb.transform.GetChild(3).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(3).transform.position.y, 0.0f);

                a = 1.0f - (float)rotateStep / (float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(5).transform.localScale = new Vector3(a, a, 0.0f);
                zeroE = false;
            }
        }

        /*
        List<int> corr = new List<int> { 1, 2, 0, linesToChoose.Count-1 };
        for (int i = 0; i < 4; i++)
        {
            if (i == 2)
                continue;
            if (talk.Phrases[linesToChoose[i]].action == 't')
            {
                playerTalkOrb.transform.GetChild(4).transform.position = new Vector3(playerTalkOrb.transform.GetChild(corr[i]).transform.position.x+ playerTalkOrb.transform.GetChild(corr[i]).GetComponent<RectTransform>().sizeDelta.x/2, playerTalkOrb.transform.GetChild(corr[i]).transform.position.y, 0.0f);
                playerTalkOrb.transform.GetChild(4).localScale = new Vector3(1.0f, 1.0f, 0.0f);
                zeroT = false;
            }
            if (talk.Phrases[linesToChoose[i]].action == 'e')
            {
                a = 1.0f-(float)(maxRotateSteps - rotateStep)/(float)maxRotateSteps;
                playerTalkOrb.transform.GetChild(5).transform.position = new Vector3(playerTalkOrb.transform.GetChild(corr[i]).transform.position.x + playerTalkOrb.transform.GetChild(corr[i]).GetComponent<RectTransform>().sizeDelta.x / 2, playerTalkOrb.transform.GetChild(corr[i]).transform.position.y, 0.0f);
                playerTalkOrb.transform.GetChild(5).localScale = new Vector3(1.0f, 1.0f, 0.0f);
                zeroE = false;
            }
        }
        /*if (rotating)
        {
            if (rotateDelta > 0)
            {
                a = 1.0f - (float)(maxRotateSteps - rotateStep) / (float)maxRotateSteps;
                if (talk.Phrases[linesToChoose[linesToChoose.Count-1]].action == 't')
                {
                    playerTalkOrb.transform.GetChild(4).localScale = new Vector3(a, a, 0.0f);
                }
                if (talk.Phrases[linesToChoose[linesToChoose.Count - 1]].action == 'e')
                {
                    playerTalkOrb.transform.GetChild(5).localScale = new Vector3(a, a, 0.0f);
                }
            }
            else
            {
                a = (float)(maxRotateSteps - rotateStep) / (float)maxRotateSteps;
                if (talk.Phrases[linesToChoose[1 % linesToChoose.Count]].action == 't')
                {
                    playerTalkOrb.transform.GetChild(4).localScale = new Vector3(a, a, 0.0f);
                }
                if (talk.Phrases[linesToChoose[1 % linesToChoose.Count]].action == 'e')
                {
                    playerTalkOrb.transform.GetChild(5).localScale = new Vector3(a, a, 0.0f);
                }
            }
        }*/
        if (zeroT)
            playerTalkOrb.transform.GetChild(4).localScale = new Vector3(0.0f, 0.0f, 0.0f);
        if (zeroE)
            playerTalkOrb.transform.GetChild(5).localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
    void CommenceRotateStep()
    {
        if(rotateDelta<0)
        playerTalkOrb.transform.GetChild(0).transform.position += new Vector3(0.0f, rotateDelta, 0.0f);
        playerTalkOrb.transform.GetChild(1).transform.position += new Vector3(0.0f, rotateDelta, 0.0f);
        if(rotateDelta>0)
        playerTalkOrb.transform.GetChild(2).transform.position += new Vector3(0.0f, rotateDelta, 0.0f);
        UpdateIcons();
        rotateStep--;
    }
    void SwitchUp()
    {
        rotating = true;
        rotateStep = maxRotateSteps;
        rotateDelta = (playerTalkOrb.transform.GetChild(1).transform.position.y - playerTalkOrb.transform.GetChild(0).transform.position.y)/maxRotateSteps;
        playerTalkOrb.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = talk.Phrases[linesToChoose[2%linesToChoose.Count]].BeautifulLine;
        playerTalkOrb.transform.GetChild(3).transform.position = playerTalkOrb.transform.GetChild(0).transform.position;
        playerTalkOrb.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = playerTalkOrb.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        playerTalkOrb.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0.0f, (float)maxRotateSteps/50.0f, false);
        playerTalkOrb.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(1.0f, (float)maxRotateSteps / 50.0f, false);
    }
    void SwitchDown()
    {
        rotating = true;
        rotateStep = maxRotateSteps;
        rotateDelta = (playerTalkOrb.transform.GetChild(1).transform.position.y - playerTalkOrb.transform.GetChild(2).transform.position.y)/ maxRotateSteps;
        playerTalkOrb.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = talk.Phrases[linesToChoose[2 % linesToChoose.Count]].BeautifulLine;
        playerTalkOrb.transform.GetChild(3).transform.position = playerTalkOrb.transform.GetChild(2).transform.position;
        playerTalkOrb.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = playerTalkOrb.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta;
        playerTalkOrb.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0.0f, (float)maxRotateSteps / 50.0f, false);
        playerTalkOrb.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(1.0f, (float)maxRotateSteps / 50.0f, false);
    }
    public void NextLine()
    {
        HideLine();
        if (playerOrNot)
        {
            playerTalkOrb.SetActive(true);
            playerTalkOrb.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0.0f, 0, false);
            //let player choose line
            ConstructLineList();
            ResetPlayer(new List<string>(){ talk.Phrases[linesToChoose[linesToChoose.Count-1]].BeautifulLine, talk.Phrases[linesToChoose[0]].BeautifulLine, talk.Phrases[linesToChoose[1% linesToChoose.Count]].BeautifulLine }, linesToChoose.Count == 1);
            choosing = true;
        }
        else
        {
            otherTalkOrb.SetActive(true);
            //choose randomly or by some system
            ConstructLineList();
            RandomChoose();
            ResetOther(talk.Phrases[chosenLine].BeautifulLine);//("sdfasd fasdfasdfasdf dfasdfasd asdfasd asdfasdfa sdaf");
            playerOrNot = !playerOrNot;
            Act();
        }
        UpdateIcons();
        maxWait = waitPerSymbol * talk.Phrases[chosenLine].BeautifulLine.Length;
        curWait = maxWait;
    }
    void RandomChoose()
    {
        chosenLine = linesToChoose[Random.Range(0, linesToChoose.Count)];
    }
    void ConstructLineList()
    {
        curIndex = talk.Phrases[chosenLine].changing_to;
        linesToChoose = new List<int>();
        for (int i = 0; i < talk.Phrases.Count; i++)
        {
            if(talk.Phrases[i].index == curIndex)
                linesToChoose.Add(i);
        }
    }

    public void HideLine()
    {
        if (!playerOrNot)
        {
            playerTalkOrb.SetActive(false);
        }
        else
        {
            otherTalkOrb.SetActive(false);
        }
    }

    public void SetTalk(Talk t, Vector3 OtherPos, Vector3 PlayerPos)
    {
        talk = t;
        otherPos = OtherPos;
        playerPos = PlayerPos;
    }
    public void Act()
    {
        for (int i = 0; i < talk.Phrases[chosenLine].eve.Count; i++)
        {
            talk.Phrases[chosenLine].eve[i].DoEvent();
        }
        switch (talk.Phrases[chosenLine].action)
        {
            case 't':
                //trade
                break;
            case 'e':
                //exit
                Exit();
                GameObject.Find("Player").GetComponent<PlayerControls>().ShowHideMenu(true, false, false, false, false, false, false);
                break;
            default:
                break;
        }
    }
    public void Exit()
    {
        chosenLine = 0;
        maxWait = 200;
        curWait = 0;
        waitPerSymbol = 10;
        choosing = false;
        playerOrNot = false;
        curIndex = -1;
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
        newWidth = maxOrbWidth;
        /*if (newLine.Length < maxSymbolsInLine)
        {
            newWidth = (newLine.Length) * (fontSize/2);
        }
        else
        {
            newWidth = (symbolsInLine) * (fontSize/2);
        }*/
        otherTalkOrb.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth + 2*fontSize, newHeight + fontSize);
        otherTalkOrb.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = newLine;
        otherTalkOrb.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, newHeight);
        otherTalkOrb.transform.position = Camera.main.WorldToScreenPoint(otherPos + new Vector3(0.0f, 10.0f, 0.0f));
    }


    public void ResetPlayer(List<string> line, bool onlyOne = false)
    {
        int newWidth = 0;
        int newHeight = fontSize + 4;
        string newLine = "";
        string newLinePart = "";
        int symbolsInLine = 0;
        int index = onlyOne ? 0 : 1;
        maxSymbolsInLine = maxOrbWidth / ((fontSize) / 2) - 2;
        foreach (string part in line[index].Split(' '))
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
        newWidth = maxOrbWidth;
        /*if (newLine.Length < maxSymbolsInLine)
        {
            newWidth = (newLine.Length) * ((fontSize)/2);
        }
        else
        {
            newWidth = (symbolsInLine) * ((fontSize)/2);
        }*/
        if (!onlyOne)
        {
            if (line[0].Length < maxSymbolsInLine)
            {
                playerTalkOrb.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = line[0];
            }
            else
            {
                playerTalkOrb.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = line[0].Substring(0, maxSymbolsInLine - 4) + "...";
            }
            playerTalkOrb.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, fontSize + 4);
            playerTalkOrb.transform.GetChild(0).transform.position = new Vector3(playerTalkOrb.transform.GetChild(0).transform.position.x, playerTalkOrb.transform.GetChild(1).transform.position.y + (newHeight / 2 + 5 + fontSize / 2));
        }
        playerTalkOrb.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = newLine;
        playerTalkOrb.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, newHeight);
        if (!onlyOne)
        {
            if (line[2].Length < maxSymbolsInLine)
            {
                playerTalkOrb.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = line[2];
            }
            else
            {
                playerTalkOrb.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = line[2].Substring(0, maxSymbolsInLine - 4) + "...";
            }
            playerTalkOrb.transform.GetChild(2).transform.position = new Vector3(playerTalkOrb.transform.GetChild(2).transform.position.x, playerTalkOrb.transform.GetChild(1).transform.position.y - (newHeight / 2 + 5 + fontSize / 2));
            playerTalkOrb.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, fontSize + 4);
        }
        if (!onlyOne)
            newHeight+=fontSize * 2 + 8;
        playerTalkOrb.GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth+2*fontSize, newHeight+ fontSize);
        playerTalkOrb.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, newHeight);
        playerTalkOrb.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Player").transform.position+new Vector3(0.0f, 10.0f, 0.0f));
    }
}
