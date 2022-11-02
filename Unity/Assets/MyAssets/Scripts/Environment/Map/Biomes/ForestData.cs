using System.Collections.Generic;
using UnityEngine;

namespace MyAssets.Scripts.Environment.Map.Biomes {

    [CreateAssetMenu(fileName = "Biome", menuName = "ScriptableObjects/ForestDTO", order = 1)]
    public class ForestData : BiomeData {
        public override void GenerateGround(List<List<BlockType>> blocks, int gridWidth, int gridHeight) {
            base.GenerateGround(blocks, gridWidth, gridHeight);
            int middle = gridHeight / 2;
            int prevY = middle, currentY = middle, deltaY;
            int waveDirectionInhibitor = 0, chanceToGoUp = 50;
            for (int x = 0; x < gridWidth; x++) {
                if (Random.Range(0, 100) < defaultWaveChance) {
                    deltaY = Random.Range(1, maxWaveInstantChange);
                    prevY = currentY;
                    deltaY *= Random.Range(0, 100) < chanceToGoUp + waveDirectionInhibitor ? 1 : -1;
                    currentY += deltaY;
                    UpdateRowsToNewHeight(blocks, x, currentY, prevY, deltaY);
                    waveDirectionInhibitor = chanceToGoUp * (middle - currentY) / maxWaveHeight;
                }
            }
        }

        private void UpdateRowsToNewHeight(List<List<BlockType>> blocks, int x, int newY, int prevY, int deltaY) {
            switch (deltaY) {
                case 1: blocks[prevY][x] = BlockType.InclRight;
                    break;
                case -1: blocks[prevY][x] = BlockType.InclRight;
                    break;
                default:
                    int minY = Mathf.Min(newY, prevY);
                    int maxY = Mathf.Max(newY, prevY);
                    BlockType newType = deltaY > 0 ? BlockType.NoTop : BlockType.Empty;
                    for(int y = newY; y < prevY; y ++)
                        blocks[y][x] = newType;
                    blocks[newY][x] = BlockType.Top;
                    break;
            }
        }
    }

}
