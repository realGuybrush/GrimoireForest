using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
    private readonly List<List<bool>> Tiles = new List<List<bool>>();
    private List<GameObject> LoadedPrefabs = new List<GameObject>();
    public List<GameObject> TilePrefabs;

    // Use this for initialization
    private void Start()
    {
        if (TilePrefabs == null)
        {
            TilePrefabs = new List<GameObject>();
        }

        LoadMap();
        //MakeLineLengthsEven();
        //UploadTilePrefabs();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void LoadMap(string filename = "Assets/Resources/default.txt")
    {
        var map = ReadMapStringFromFile(filename);
        if (map == "")
        {
            return;
        }

        Tiles.Add(new List<bool>());
        for (var i = 0; i < map.Length; i++)
        {
            switch (map[i])
            {
                case ' ':
                    Tiles[Tiles.Count - 1].Add(false);
                    break;
                case '.':
                    Tiles[Tiles.Count - 1].Add(true);
                    break;
                case '\n':
                    Tiles.Add(new List<bool>());
                    break;
            }
        }
    }

    private string ReadMapStringFromFile(string filename)
    {
        var map = "";
        try
        {
            var Reader = new StreamReader(filename);
            map = Reader.ReadToEnd();
            Reader.Close();
        }
        catch
        {
            Debug.Log("File \"'{filename}'\" do not exist, or can not be opened.");
        }

        return map;
    }

    private void MakeLineLengthsEven()
    {
        //In case somebody has screwed up with map.
        var curved = false;
        var sizeX = Tiles[0].Count;
        for (var i = 0; i < Tiles.Count; i++)
        {
            if (sizeX != Tiles[i].Count)
            {
                sizeX = Tiles[i].Count;
                curved = true;
            }
        }

        if (curved)
        {
            Debug.Log("Map lines have uneven lengths. Shorter lines will be filled up with walls.");
            for (var i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i].Count != sizeX)
                {
                    while (Tiles[i].Count < sizeX)
                    {
                        Tiles[i].Add(true);
                    }
                }
            }
        }
    }

    private void UploadTilePrefabs()
    {
        GameObject temp;
        for (var i = 0; i < Tiles.Count; i++)
        {
            for (var j = 0; j < Tiles[i].Count; j++)
            {
                if (Tiles[i][j])
                {
                    temp = Instantiate(TilePrefabs[0], new Vector3(j, -i), new Quaternion());
                }
            }
        }
    }
}