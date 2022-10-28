using System.Collections.Generic;
using UnityEngine;

namespace MyAssets.Scripts.Environment {

    public class EnvironmentFactory : MonoBehaviour {
        [SerializeField]
        private string _directory;

        public List<List<BiomeData>> BiomeDTOs;

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
