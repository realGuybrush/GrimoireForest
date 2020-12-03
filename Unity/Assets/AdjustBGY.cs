using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustBGY : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustBGPartPositions(List<List<int>> blocks, int BGPartNumberAsChild)
    {
        int midlevel = 5;
        float inclinedOffsety = 0.0f;
        int j = 0;
        int k = 0;
        int active = 0;
        if (transform.GetChild(BGPartNumberAsChild).GetChild(0).gameObject.activeSelf)
            active = 0;
        if (transform.GetChild(BGPartNumberAsChild).GetChild(1).gameObject.activeSelf)
            active = 1;
        for (int i = 0; i < transform.GetChild(BGPartNumberAsChild).GetChild(active).childCount; i++)//need children of active child of BGPartNumberAsChild
        {
            j = 0;
            k = ((int)(transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).transform.position.x / 1.5f) + blocks[0].Count / 2);
            if (k < 0)
                k = 0;
            if (k > blocks[0].Count - 1)
                k = blocks[0].Count - 1;
            for (j = 0; j < 9; j++)
            {
                if (blocks[j][k] != 4)
                    break;
            }
            /*if (GlobalMap.Tiles[y][x].blocks[j][k] == 1)
            {
                if (k != 0)
                {
                    inclinedOffsety = (Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x + TileWidth / 2 - TileWidth * xOffset) % k;
                    if (inclinedOffsety > 1.5f)
                    {
                        inclinedOffsety = (Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x+TileWidth/2 - TileWidth * xOffset) % (k - 1);
                    }
                }
                else
                {
                    inclinedOffsety = Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x+TileWidth/2-TileWidth*xOffset;
                }
                inclinedOffsety = 1.5f - inclinedOffsety;
            }
            if (GlobalMap.Tiles[y][x].blocks[j][k] == 2)
            {
                if (k != 0)
                {
                    inclinedOffsety = (Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x + TileWidth / 2 - TileWidth * xOffset) % k;
                    if (inclinedOffsety > 1.5f)
                    {
                        inclinedOffsety = (Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x + TileWidth / 2 - TileWidth * xOffset) % (k - 1);
                    }
                }
                else
                {
                    inclinedOffsety = Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x + TileWidth / 2 - TileWidth * xOffset;
                }
            }*/
            transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position = new Vector3(transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x,
                                                             transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.y + (midlevel - j - inclinedOffsety) * 1.5f,
                                                             transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.z + 0.0f);
        }
    }
}
