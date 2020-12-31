using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float xmin = -35;
    public float xmax = 35;
    public float ymin = 2;
    public float ymax = 2;
    public GameObject subPlatforms;
    public int MaxAmount = 5;
    public int numberInBiome = 0;
    public bool sub = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    public List<EnvironmentStuffingValues> GeneratePlatforms(List<List<int>> blocks, bool This = true, Vector3 offset = new Vector3())
    {
        List<EnvironmentStuffingValues> P = new List<EnvironmentStuffingValues>();
        int amount = Random.Range(0, MaxAmount);
        for (int i = 0; i < amount; i++)
        {
            if (This)
            {
                P.Add(new EnvironmentStuffingValues(UpdateRandomPlaceWithBlocks(SetRandomPlace(), blocks), numberInBiome));
                if (subPlatforms != null)
                {
                    P.AddRange(subPlatforms.GetComponent<Platform>().GeneratePlatforms(blocks, false, P[P.Count-1].location.ToV3()));
                }
            }
            else
            {
                P.Add(new EnvironmentStuffingValues(UpdateRandomPlaceWithBlocks(SetRandomPlace(), blocks) + offset, numberInBiome));
            }
        }
        return P;
    }
    public Vector3 UpdateRandomPlaceWithBlocks(Vector3 randomPlace, List<List<int>> blocks)
    {
        int i;
        int k = (int)(randomPlace.x / 1.5f) + blocks[0].Count / 2 + blocks[0].Count % 2;
        for (i = 0; i < blocks.Count; i++)
        {
            if ((blocks[i][k] == (int)BlockType.NoTop) || (blocks[i][k] == (int)BlockType.Top)|| (blocks[i][k] == (int)BlockType.TopBushD)||
                (blocks[i][k] == (int)BlockType.TopBushU) || (blocks[i][k] == (int)BlockType.TopBushUD))
                break;
        }
        return new Vector3(randomPlace.x, randomPlace.y+(4-i), randomPlace.z);
    }
    public Vector3 SetRandomPlace()
    {
        return new Vector3(Random.Range(xmin, xmax), Random.Range(ymin, ymax), 0.0f);
    }
}
