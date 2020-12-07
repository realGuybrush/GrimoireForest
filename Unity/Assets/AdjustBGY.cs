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

    public void AdjustBGPartPositions(List<List<int>> blocks, int BGPartNumberAsChild, float yDiff = 0.0f)
    {
        int midlevel = 4;
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
            inclinedOffsety = 0;
            k = ((int)(transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).transform.localPosition.x / 1.5f) + blocks[0].Count / 2);
            if (k < 0)
                k = 0;
            if (k > blocks[0].Count - 1)
                k = blocks[0].Count - 1;
            for (j = 0; j < 9; j++)
            {
                if (blocks[j][k] != 4)
                    break;
            }
            /*if (blocks[j][k] == 1)
            {
                inclinedOffsety = 1.0f;
            }
            if (blocks[j][k] == 2)
            {
                inclinedOffsety = 1.0f;
            }
            if (blocks[j][k] == 1)
            {
                if (k != 0)
                {
                    inclinedOffsety = (transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x + 1.5f* blocks[0].Count / 2) % k;
                    if (inclinedOffsety > 1.5f)
                    {
                        inclinedOffsety = (transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x+1.5f* blocks[0].Count / 2) % (k - 1);
                    }
                }
                else
                {
                    inclinedOffsety = transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x+1.5f* blocks[0].Count / 2;
                }
                inclinedOffsety = 1.5f - inclinedOffsety;
            }
            if (blocks[j][k] == 2)
            {
                if (k != 0)
                {
                    inclinedOffsety = (transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x + 1.5f*blocks[0].Count / 2) % k;
                    if (inclinedOffsety > 1.5f)
                    {
                        inclinedOffsety = (transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x + 1.5f* blocks[0].Count / 2) % (k - 1);
                    }
                }
                else
                {
                    inclinedOffsety = transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x + 1.5f* blocks[0].Count / 2;
                }
            }*/
            transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).localPosition = new Vector3(transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).localPosition.x,
                                                             transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).localPosition.y + (midlevel - j) * 1.5f - inclinedOffsety + yDiff,
                                                             transform.GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).localPosition.z + 0.0f);
        }
    }
}
