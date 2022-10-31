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
    Vector3 playerOrbOffset;
    Vector3 otherOrbOffset;
    float playerTalkWidth;
    float playerTalkHeight;
    float otherTalkWidth;
    float otherTalkHeight;

    bool appearing = false;
    int appearStep = 0;
    float appearDelta = 0.0f;
    int maxAppearSteps = 15;

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
        playerTalkOrb.transform.position = playerPos;
        otherTalkOrb.transform.position = otherPos;
        otherTalkOrb.transform.GetChild(4).localScale = new Vector3(0.0f, 0.0f, 0.0f);
        otherTalkOrb.transform.GetChild(5).localScale = new Vector3(0.0f, 0.0f, 0.0f);
        playerTalkWidth = 300;//playerTalkOrb.GetComponent<RectTransform>().sizeDelta.x;
        playerTalkHeight = 50;//playerTalkOrb.GetComponent<RectTransform>().sizeDelta.y;
        otherTalkWidth = 300;//otherTalkOrb.GetComponent<RectTransform>().sizeDelta.x;
        otherTalkHeight = 50;//otherTalkOrb.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (talk == null)
            return;
        if (!appearing)
        {
            if (!rotating)
            {
                if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Space))
                {
                    if (!choosing)
                    {
                        Act();
                        NextLine();
                    }
                    else
                    {
                        //ResetPlayer(new List<string>() { talk.Phrases[linesToChoose[linesToChoose.Count - 1]].BeautifulLine, talk.Phrases[linesToChoose[0]].BeautifulLine, talk.Phrases[linesToChoose[1 % linesToChoose.Count]].BeautifulLine }, linesToChoose.Count == 1);
                        chosenLine = linesToChoose[0];
                        playerOrNot = !playerOrNot;
                        //NextLine();
                        playerTalkOrb.transform.GetChild(4).localScale = new Vector3(0.0f, 0.0f, 0.0f);
                        playerTalkOrb.transform.GetChild(5).localScale = new Vector3(0.0f, 0.0f, 0.0f);
                        ResetPlayer(new List<string>() { talk.Phrases[linesToChoose[0]].BeautifulLine }, true);
                        choosing = false;
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
                        Act();
                        NextLine();
                        curWait--;
                    }
                    else
                    {
                        if (curWait > 0)
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
        else
        {
            if (appearStep > -1)
            {
                CommenceAppearStep();
            }
            else
            {
                EndAppearing();
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

    void UpdateIcons()
    {
        List<char> simp1 = new List<char>() { 't', 'e' };
        List<int> simp2 = new List<int>() { 4, 5 };
        float a;
        bool zero = true;
        float posDelta = ((playerTalkOrb.GetComponent<RectTransform>().sizeDelta.x - fontSize) / 2)*playerTalkOrb.transform.localScale.y;
        for (int i = 0; i < simp1.Count; i++)
        {
            zero = true;
            playerTalkOrb.transform.GetChild(simp2[i]).localScale = new Vector3(1.0f, 1.0f, 0.0f);
            if (talk.Phrases[linesToChoose[linesToChoose.Count - 1]].action == simp1[i])
            {
                playerTalkOrb.transform.GetChild(simp2[i]).transform.position = new Vector3(playerTalkOrb.transform.GetChild(0).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(0).transform.position.y, 0.0f);
                if (rotateDelta > 0)
                {
                    a = (float)rotateStep / (float)maxRotateSteps;
                    playerTalkOrb.transform.GetChild(simp2[i]).transform.localScale = new Vector3(a, a, 0.0f);
                }
                zero = false;
            }

            if (talk.Phrases[linesToChoose[0]].action == simp1[i])
            {
                playerTalkOrb.transform.GetChild(simp2[i]).transform.position = new Vector3(playerTalkOrb.transform.GetChild(1).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(1).transform.position.y, 0.0f);
                zero = false;
            }

            if (talk.Phrases[linesToChoose[1 % linesToChoose.Count]].action == simp1[i])
            {
                playerTalkOrb.transform.GetChild(simp2[i]).transform.position = new Vector3(playerTalkOrb.transform.GetChild(2).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(2).transform.position.y, 0.0f);
                if (rotateDelta < 0)
                {
                    a = (float)rotateStep / (float)maxRotateSteps;
                    playerTalkOrb.transform.GetChild(simp2[i]).transform.localScale = new Vector3(a, a, 0.0f);
                }
                zero = false;
            }
            if (rotateDelta < 0)
            {
                if ((talk.Phrases[linesToChoose[2 % linesToChoose.Count]].action == simp1[i]) && (zero))
                {
                    playerTalkOrb.transform.GetChild(simp2[i]).transform.position = new Vector3(playerTalkOrb.transform.GetChild(3).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(3).transform.position.y, 0.0f);
                    a = 1.0f - (float)rotateStep / (float)maxRotateSteps;
                    playerTalkOrb.transform.GetChild(simp2[i]).transform.localScale = new Vector3(a, a, 0.0f);
                    zero = false;
                }
            }
            if (rotateDelta > 0)
            {
                if ((talk.Phrases[linesToChoose[(linesToChoose.Count - 2) % linesToChoose.Count]].action == simp1[i]) && (zero))
                {
                    playerTalkOrb.transform.GetChild(simp2[i]).transform.position = new Vector3(playerTalkOrb.transform.GetChild(3).transform.position.x + posDelta, playerTalkOrb.transform.GetChild(3).transform.position.y, 0.0f);
                    a = 1.0f - (float)rotateStep / (float)maxRotateSteps;
                    playerTalkOrb.transform.GetChild(simp2[i]).transform.localScale = new Vector3(a, a, 0.0f);
                    zero = false;
                }
            }
            if (zero)
                playerTalkOrb.transform.GetChild(simp2[i]).localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }
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
            playerTalkOrb.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            playerTalkOrb.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0.0f, 0, false);
            //let player choose line
            ConstructLineList();
            ResetPlayer(new List<string>(){ talk.Phrases[linesToChoose[linesToChoose.Count-1]].BeautifulLine, talk.Phrases[linesToChoose[0]].BeautifulLine, talk.Phrases[linesToChoose[1% linesToChoose.Count]].BeautifulLine }, linesToChoose.Count == 1);
            choosing = true;
        }
        else
        {
            otherTalkOrb.SetActive(true);
            otherTalkOrb.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            //choose randomly or by some system
            ConstructLineList();
            RandomChoose();
            ResetOther(talk.Phrases[chosenLine].BeautifulLine);
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
    public void CalculateOffsets()
    {
        float x, y;
        float w = Camera.main.scaledPixelWidth;
        float h = Camera.main.scaledPixelHeight;
        Vector3 playerPos2 = Camera.main.WorldToScreenPoint(playerPos);
        Vector3 otherPos2 = Camera.main.WorldToScreenPoint(otherPos);
        //float playerTalkWidth = playerTalkOrb.GetComponent<RectTransform>().rect.width;// Mathf.Abs(Camera.main.WorldToScreenPoint(new Vector3(0.0f, 0.0f, 0.0f)).x - Camera.main.WorldToScreenPoint(new Vector3(playerTalkOrb.GetComponent<RectTransform>().rect.width, 0.0f, 0.0f)).x);
        //float playerTalkHeight = playerTalkOrb.GetComponent<RectTransform>().rect.height;// Mathf.Abs(Camera.main.WorldToScreenPoint(new Vector3(0.0f, 0.0f, 0.0f)).y - Camera.main.WorldToScreenPoint(new Vector3(0.0f, playerTalkOrb.GetComponent<RectTransform>().rect.height, 0.0f)).y);
        //float otherTalkWidth = otherTalkOrb.GetComponent<RectTransform>().rect.width;// Mathf.Abs(Camera.main.WorldToScreenPoint(new Vector3(0.0f, 0.0f, 0.0f)).x - Camera.main.WorldToScreenPoint(new Vector3(otherTalkOrb.GetComponent<RectTransform>().rect.width, 0.0f, 0.0f)).x);
        //float otherTalkHeight = otherTalkOrb.GetComponent<RectTransform>().rect.height;// Mathf.Abs(Camera.main.WorldToScreenPoint(new Vector3(0.0f, 0.0f, 0.0f)).y - Camera.main.WorldToScreenPoint(new Vector3(0.0f, otherTalkOrb.GetComponent<RectTransform>().rect.height, 0.0f)).y);
        float posDif = Mathf.Sign(playerPos2.x - otherPos2.x);
        if ((playerPos2.x + posDif * playerTalkWidth/2.0f) > w)
            x = w - playerTalkWidth/2.0f;
        else
        if ((playerPos2.x - posDif * playerTalkWidth/2.0f) < 0)
            x = playerTalkWidth/2.0f;
        else
            x = 0.0f;
        if ((playerPos2.y + 150.0f + playerTalkHeight) > h)
            y = h - 1.5f*playerTalkHeight - playerPos2.y;
        else
            y = 150.0f;
        //x = 0.0f;
        //y = 10.0f;
        playerOrbOffset = new Vector3(x, y, 0.0f);// Camera.main.ScreenToWorldPoint();
        if ((otherPos2.x - posDif * otherTalkWidth/2.0f) > w)
            x = otherTalkWidth/2.0f;
        else
        if ((otherPos2.x + posDif * otherTalkWidth/2.0f) < 0)
            x = w - otherTalkWidth/2.0f;
        else
            x = 0.0f;
        if ((otherPos2.y + 150.0f + otherTalkHeight) > h)
            y = h - 2*otherTalkHeight - otherPos2.y;
        else
            y = 150.0f;
        //x = 0.0f;
        //y = 10.0f;
        otherOrbOffset = new Vector3(x, y, 0.0f);// Camera.main.ScreenToWorldPoint();
    }
    public void HideLine()
    {
        if (!playerOrNot)
        {
            playerTalkOrb.SetActive(false);
            playerTalkOrb.transform.position = playerPos;
        }
        else
        {
            otherTalkOrb.SetActive(false);
            otherTalkOrb.transform.position = otherPos;
        }
        CalculateOffsets();
        appearStep = maxAppearSteps;
        appearing = true;
    }
    void CommenceAppearStep()
    {
        if(choosing)
        {
            //playerOrbOffset = Camera.main.WorldToScreenPoint(new Vector3(0.0f, 10.0f, 0.0f));
            Vector3 playerPos2 = Camera.main.WorldToScreenPoint(playerPos);
            appearDelta = (((float)maxAppearSteps - (float)appearStep) / (float)maxAppearSteps);
            playerTalkOrb.transform.position = playerPos2 + new Vector3(appearDelta * playerOrbOffset.x, appearDelta * playerOrbOffset.y, 0.0f);
            playerTalkOrb.transform.localScale = new Vector3(appearDelta * 1.0f, appearDelta * 1.0f, 0.0f);
            ResetPlayer(new List<string>() { talk.Phrases[linesToChoose[linesToChoose.Count - 1]].BeautifulLine, talk.Phrases[linesToChoose[0]].BeautifulLine, talk.Phrases[linesToChoose[1 % linesToChoose.Count]].BeautifulLine }, linesToChoose.Count == 1);
        }
        else
        {
            //otherOrbOffset = Camera.main.WorldToScreenPoint(new Vector3(0.0f, 10.0f, 0.0f));
            Vector3 otherPos2 = Camera.main.WorldToScreenPoint(otherPos);
            appearDelta = (((float)maxAppearSteps - (float)appearStep) / (float)maxAppearSteps);
            otherTalkOrb.transform.position = otherPos2 + new Vector3(appearDelta * otherOrbOffset.x, appearDelta * otherOrbOffset.y, 0.0f);
            otherTalkOrb.transform.localScale = new Vector3(appearDelta * 1.0f, appearDelta * 1.0f, 0.0f);
        }
        UpdateIcons();
        appearStep--;
    }

    public void EndAppearing()
    {
        appearing = false;
    }

    public void SetTalk(Talk t, Vector3 OtherPos, Vector3 PlayerPos)
    {
        talk = t;
        otherPos = OtherPos;
        playerPos = PlayerPos;
    }
    public void Act()
    {
        if ((chosenLine > talk.Phrases.Count) || (chosenLine < -1))
            return;
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
        GameObject.Find("Player").GetComponent<PlayerControls>().UpdateGlobalTalks();
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
        otherTalkOrb.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth + 2*fontSize, newHeight + fontSize);
        otherTalkOrb.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = newLine;
        otherTalkOrb.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, newHeight);
        otherTalkWidth = newWidth;
        otherTalkHeight = newHeight;
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
        newWidth = (int)(((float)maxOrbWidth)*playerTalkOrb.transform.localScale.x);
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
            playerTalkOrb.transform.GetChild(0).transform.position = new Vector3(playerTalkOrb.transform.GetChild(0).transform.position.x, playerTalkOrb.transform.GetChild(1).transform.position.y + (newHeight / 2 + 5 + fontSize / 2) * playerTalkOrb.transform.localScale.y);
        }
        else
        {
            playerTalkOrb.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector3(0.0f, 0.0f, 0.0f);
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
            playerTalkOrb.transform.GetChild(2).transform.position = new Vector3(playerTalkOrb.transform.GetChild(2).transform.position.x, playerTalkOrb.transform.GetChild(1).transform.position.y - (newHeight / 2 + 5 + fontSize / 2) * playerTalkOrb.transform.localScale.y);
            playerTalkOrb.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, fontSize + 4);
        }
        else
        {
            playerTalkOrb.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector3(0.0f, 0.0f, 0.0f);
        }
        if (!onlyOne)
            newHeight+=fontSize * 2 + 8;
        playerTalkOrb.GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth+2*fontSize, newHeight+ fontSize);
        playerTalkOrb.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector3(newWidth, newHeight);
        playerTalkWidth = newWidth;
        playerTalkHeight = newHeight;
    }
}
