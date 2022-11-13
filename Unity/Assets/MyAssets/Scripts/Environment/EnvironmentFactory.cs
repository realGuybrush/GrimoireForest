using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyAssets.Scripts.Environment {

    public class EnvironmentFactory : MonoBehaviour {

        [SerializeField]
        private List<ListBiomeData> BiomeDTOs = new List<ListBiomeData>();

        public static EnvironmentFactory GetInstance;

        private void Awake() {
            if (GetInstance == null) GetInstance = this;
            else {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            //WARNING! BIOMES MUST BE PUT IN List<List<BiomesData>> MANUALLY, BECAUSE IT IS A SQUARE MATRIX!
        }

        public int GetBiomeRadius(BiomeType biomeType, int maxSize) {
            int biomeIndex = (int) biomeType;
            if (biomeIndex < BiomeDTOs.Count) {
                var biome = BiomeDTOs[biomeIndex].list[biomeIndex];
                return Random.Range((int) biome.minPercentSize, maxSize * (int) biome.maxPercentSize / 100);
            }
            return maxSize;
        }

        public int GetBiomeGrowth(BiomeType biomeType) {
            int type = (int) biomeType;
            return BiomeDTOs[type].list[type].growthPower;
        }

        public int GetBiomeResist(BiomeType biomeType) {
            int type = (int) biomeType;
            return BiomeDTOs[type].list[type].resistancePower;
        }

        public List<BackgroundMovement> GetBGListByBiome(BiomeType type1, BiomeType type2) {
            return BiomeDTOs[(int)type1].list[(int)type2].BackgroundPrefab;
        }

        public SerializableDictionaryBase<BlockType, TileBase> GetBiomeBlockPrefabs(BiomeType biome1, BiomeType biome2) {
            return BiomeDTOs[(int) biome1].list[(int) biome2].BlockPrefabs;
        }

        public List<GameObject> GetBiomePlatformPrefabs(BiomeType biome1, BiomeType biome2) {
            return BiomeDTOs[(int) biome1].list[(int) biome2].PlatformPrefab;
        }

        public List<EnemyMovement> GetBiomeEntitiesPrefabs(BiomeType biome1, BiomeType biome2) {
            return BiomeDTOs[(int) biome1].list[(int) biome2].EntitiesPrefabs;
        }

        public List<int> GetBiomeEntitiesAmounts(BiomeType biome1, BiomeType biome2) {
            return BiomeDTOs[(int) biome1].list[(int) biome2].EntitiesAmounts;
        }

        public int GetBiomePlatformCount(BiomeType biome1, BiomeType biome2) {
            return BiomeDTOs[(int) biome1].list[(int) biome2].PlatformPrefab.Count;
        }

        public void GenerateGroundByBiome(BiomeType biome1, BiomeType biome2, List<List<BlockType>>blocks,
            int gridWidth, int gridHeight) {
                BiomeDTOs[(int) biome1].list[(int) biome2].GenerateGround(blocks, gridWidth, gridHeight);
        }

        public int BiomeAmount => BiomeDTOs.Count;

    }

}
