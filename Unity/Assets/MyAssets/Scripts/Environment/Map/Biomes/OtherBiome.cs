namespace MyAssets.Scripts.Environment.Map.Biomes {

    public class OtherBiome: BiomeData {


    /*void FromCenterRow(int middle, int maxDiffs) {
        int temp;
        for (int i = 0; i < gridWidth; i++) {
            temp = Random.Range(-maxDiffs, (maxDiffs));
            for (int j = 0; j < gridHeight; j++) {
                blocks[j][i] = j < middle + temp ? BlockType.Empty : BlockType.NoTop;
                //4-empty; 3-solid without topping
            }
        }
    }

    void FromCenterRowWavy(int middle, int maxDiffs) {
        if (maxDiffs + middle > 8)
            maxDiffs = 8 - middle;
        if (maxDiffs > middle)
            maxDiffs = middle;
        int temp = 0;
        int addt = 1;
        for (int i = gridWidth / 2; i < gridWidth; i++) {
            if ((temp < maxDiffs) && (addt == 1)) {
                temp += addt;
            } else {
                addt = -1;
            }
            if ((temp > -maxDiffs) && (addt == -1)) {
                temp += addt;
            } else {
                addt = 1;
            }
            for (int j = 0; j < gridHeight; j++) {
                blocks[j][i] = ((j < middle + temp) ? BlockType.Empty : BlockType.NoTop);
            }
        }
        for (int i = gridWidth / 2 - 1; i > -1; i--) {
            if ((temp < maxDiffs) && (addt == 1)) {
                temp += addt;
            } else {
                addt = -1;
            }
            if ((temp > -maxDiffs) && (addt == -1)) {
                temp += addt;
            } else {
                addt = 1;
            }
            for (int j = 0; j < gridHeight; j++) {
                blocks[j][i] = ((j < middle + temp) ? BlockType.Empty : BlockType.NoTop);
            }
        }
    }

    void FromCenterAndEdges(int middle, int maxDiffs) {
        int temp = middle;
        for (int i = 0; i < (gridWidth / 4 + 1); i++) {
            temp += Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < gridHeight; j++) {
                blocks[j][i] = ((j < temp) ? BlockType.Empty : BlockType.NoTop);
            }*/
            /*temp = UnityEngine.Random.Range(-maxDiffs, (maxDiffs+1));
            for (int j = 4 + temp; j != 4- (int)Mathf.Sign(temp); j -= (int)Mathf.Sign(temp))
            {
                MT.blocks[j][i] = ((j < (4-(int)Mathf.Sign(temp))) ? 3 : 4);//4-empty; 3-solid without topping
            }*//*
        }
        temp = 4;
        for (int i = gridWidth / 2; i > (gridWidth / 4); i--) {
            temp += Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < gridHeight; j++) {
                blocks[j][i] = ((j < temp) ? BlockType.Empty : BlockType.NoTop);
            }
        }
        temp = 4;
        for (int i = gridWidth / 2 + 1; i < (3 * (gridWidth / 4) + 1); i++) {
            temp += Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < gridHeight; j++) {
                blocks[j][i] = ((j < temp) ? BlockType.Empty : BlockType.NoTop);
            }
        }
        temp = 4;
        for (int i = gridWidth - 1; i > (3 * (gridWidth / 4)); i--) {
            temp += Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < gridHeight; j++) {
                blocks[j][i] = ((j < temp) ? BlockType.Empty : BlockType.NoTop);
            }
        }
    }*/

    /*void GenerateAllTilesStuffing(List<AllBiomePrefabs> PrefabsOfAllBiomes)
    {
        //fix; both this parameters must be calculated, considering biome and some diapasone of values
        int MaxBGObjectAmount = 10;
        int MaxBarricadesAmount = 10;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                //Tiles.Tiles [y] [x].FrontalBackgroundIndex = Random.Range (0, PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].FrontalBackground.Count);
                //Tiles.Tiles [y] [x].RoadIndex = Random.Range (0, PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].Road.Count);
                for (int BGObjectAmount = 0; BGObjectAmount < MaxBGObjectAmount; BGObjectAmount++)
                {
                    Tiles.Tiles [y] [x].TileBackgroundObjects [BGObjectAmount] = PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].BackgroundObjects[Random.Range (0, PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].BackgroundObjects.Count)];
                    //fix; choose their coordinates somehow
                }
                for (int BarricadesAmount = 0; BarricadesAmount < MaxBarricadesAmount; BarricadesAmount++)
                {
                    Tiles.Tiles [y] [x].TileBarrcicades [BarricadesAmount] = PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].Barricades[Random.Range (0, PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].Barricades.Count)];
                    //fix; choose their coordinates somehow; maybe add point array, or make new class int+point; maybe make list of objects instead of list of ints
                }
            }
        }
    }*/
    }

}
