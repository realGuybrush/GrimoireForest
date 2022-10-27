using System.Collections.Generic;
using UnityEngine;

namespace MyAssets.Scripts.Environment {

    public class EnvironmentFactory : MonoBehaviour {
        [SerializeField]
        private string _directory;

        public List<List<BiomeData>> BiomePrefabs = new List<List<BiomeData>>();

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
                BiomePrefabs.Add(new List<BiomeData>());
                for (int j = 0; j < BiomeData.BiomesAmount; j++) {
                    BiomePrefabs[i].Add(new BiomeData());
                }
            }
            BiomePrefabs[0][0].aggressive = false;
            BiomePrefabs[0][0].immortal = false;
            BiomePrefabs[0][0].Size = MaxBiomeSizeInPercents.Enormous;
            BiomePrefabs[0][0].Sky = (Sprite) Resources.Load(_directory + "stars.png");//don't + strings this way
            BiomePrefabs[0][0].Moon = (Sprite) Resources.Load("Pictures\\Environment\\Forest\\moon.png");
            BiomePrefabs[0][0].TilePrefab = (GameObject) Resources.Load("Prefabs\\Environment\\Forest\\ForestTile");
            BiomePrefabs[0][0].PlatformPrefab.Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\BigTree"));
            BiomePrefabs[0][0].PlatformPrefab
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\TreeBranch"));
            BiomePrefabs[0][0].PlatformPrefab
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\BushCover1"));
            BiomePrefabs[0][0].PlatformPrefab
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\BushCover2"));
            BiomePrefabs[0][0].PlatformPrefab
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\BushCover3"));
            BiomePrefabs[0][0].PlatformPrefab = RemoveAllNull(BiomePrefabs[0][0].PlatformPrefab);
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.FullTop"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleLeft"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleRight"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.Full"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.Full"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.FullTopBushU"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.FullTopBushD"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load("Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.FullTopBushUD"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleLeftBushU"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleLeftBushD"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleLeftBushUD"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleRightBushU"));
            BiomePrefabs[0][0].BlockPrefabs
                .Add((GameObject) Resources.Load(
                    "Prefabs\\Environment\\Forest\\Blocks\\Forest.Ground.TriangleRightBushD"));
            BiomePrefabs[0][0].BlockPrefabs
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
    }

}
