using System.Collections.Generic;
using UnityEngine;

namespace MyAssets.Scripts.Environment {

    public class EnvironmentFactory : MonoBehaviour {
        [SerializeField]
        private string _directory;

        public List<List<BiomeData>> BiomeDTOs = new List<List<BiomeData>>();

        public static EnvironmentFactory GetInstance;

        private void Awake() {
            if (GetInstance == null) GetInstance = this;
            else {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            //todo: make it load sprites and other stuff using _directory; fix this crap
            _directory = "Pictures\\Environment\\Forest\\";
            for (int i = 0; i < BiomeData.BiomesAmount; i++) {
                BiomeDTOs.Add(new List<BiomeData>());
                for (int j = 0; j < BiomeData.BiomesAmount; j++) {
                    BiomeDTOs[i].Add(new BiomeData());
                }
            }
            BiomeDTOs[0][0].aggressive = false;
            BiomeDTOs[0][0].immortal = false;
            BiomeDTOs[0][0].maxPercentSize = MaxBiomeSizeInPercents.Enormous;
            BiomeDTOs[0][0].Sky = (Sprite) Resources.Load(_directory + "stars.png"); //don't + strings this way
            BiomeDTOs[0][0].Moon = (Sprite) Resources.Load("Pictures\\Environment\\Forest\\moon.png");
            BiomeDTOs[0][0].TilePrefab = (GameObject) Resources.Load("Prefabs\\Environment\\Forest\\ForestTile");
            BiomeDTOs[0][0].PlatformPrefab.Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\BigTree"));
            BiomeDTOs[0][0].PlatformPrefab
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\TreeBranch"));
            BiomeDTOs[0][0].PlatformPrefab
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\BushCover1"));
            BiomeDTOs[0][0].PlatformPrefab
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\BushCover2"));
            BiomeDTOs[0][0].PlatformPrefab
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\BushCover3"));
            BiomeDTOs[0][0].PlatformPrefab = RemoveAllNull(BiomeDTOs[0][0].PlatformPrefab);
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.FullTop"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleLeft"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleRight"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.Full"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.Full"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.FullTopBushU"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.FullTopBushD"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.FullTopBushUD"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleLeftBushU"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleLeftBushD"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleLeftBushUD"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleRightBushU"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleRightBushD"));
            BiomeDTOs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleRightBushUD"));
            /*BiomePrefabs[0][0].EntitiesPrefabs.Add(1);
            BiomePrefabs[0][0].EntitiesAmounts.Add(2);
            BiomePrefabs[0][0].EntitiesPrefabs.Add(2);
            BiomePrefabs[0][0].EntitiesAmounts.Add(2);
            BiomePrefabs[0][0].EntitiesPrefabs.Add(3);
            BiomePrefabs[0][0].EntitiesAmounts.Add(2);
            BiomePrefabs[0][0].EntitiesPrefabs.Add(4);
            BiomePrefabs[0][0].EntitiesAmounts.Add(2);*/
            //fix add somewhere predefinition of tile monster spawn pattern
        }

        public List<GameObject> RemoveAllNull(List<GameObject> L) {
            for (int i = 0; i < L.Count; i++) {
                if (L[i] == null) {
                    L.RemoveAt(i);
                    i--;
                }
            }
            return L;
        }

        public int GetBiomeRadius(BiomeType biomeType, int maxSize) {
            int biomeIndex = (int) biomeType;
            if (biomeIndex < BiomeDTOs.Count) {
                var biome = BiomeDTOs[biomeIndex][biomeIndex];
                return Random.Range((int) biome.minPercentSize, maxSize * (int) biome.maxPercentSize / 100);
            }
            return maxSize;
        }

        public int GetBiomeGrowth(BiomeType biomeType) {
            int type = (int) biomeType;
            return BiomeDTOs[type][type].growthPower;
        }

        public int GetBiomeResist(BiomeType biomeType) {
            int type = (int) biomeType;
            return BiomeDTOs[type][type].resistancePower;
        }

        public Sprite GetTileSpriteByBiome() {
            return null;
        }

        public Sprite GetBG1SpriteByBiome() {
            return null;
        }

        public Sprite GetBG2SpriteByBiome() {
            return null;
        }

        public Sprite GetBG3SpriteByBiome() {
            return null;
        }

        public int GetBiomePlatformCount(BiomeType biome1, BiomeType biome2) {
            return BiomeDTOs[(int) biome1][(int) biome2].PlatformPrefab.Count;
        }

        public int BiomeAmount => BiomeDTOs.Count;

    }

}
