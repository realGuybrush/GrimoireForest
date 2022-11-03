using System.Collections.Generic;
using UnityEngine;

namespace MyAssets.Scripts.Environment.Map.Biomes {

    [CreateAssetMenu(fileName = "Biome", menuName = "ScriptableObjects/ForestDTO", order = 1)]
    public class ForestData : BiomeData {
        public override void GenerateGround(List<List<BlockType>> blocks, int gridWidth, int gridHeight) {
            //self-documenting miserably failed...
            //making heights list on current tile
            List<int> heightsList = new List<int>();
            int middle = gridHeight / 2;
            int prevY = middle, currentY = middle, deltaY;
            int waveDirectionInhibitor = 0, chanceToGoUp = 50, thisStepChanceInhibition = 0;
            for (int x = 0; x < gridWidth; x++) {
                if (Random.Range(0, 100) < defaultWaveChance - thisStepChanceInhibition) {
                    deltaY = Random.Range(1, maxWaveInstantChange);
                    currentY += Random.Range(0, 100) < chanceToGoUp + waveDirectionInhibitor ? deltaY : -deltaY;
                    waveDirectionInhibitor = chanceToGoUp * (middle - currentY) / maxWaveHeight;
                    thisStepChanceInhibition = 100;
                } else {
                    if(thisStepChanceInhibition > 0)
                        thisStepChanceInhibition -= 50;
                }
                heightsList.Add(currentY);
            }
            //defining empty tile grid
            for (int i = 0; i < gridHeight; i++) {
                blocks.Add(new List<BlockType>());
                for (int j = 0; j < gridWidth; j++)
                    blocks[i].Add(BlockType.Empty);
            }
            //filling grid with ground
            for (int x = 0; x < gridWidth; x++) {
                //simple dirt blocks
                for (int y = 0; y <= heightsList[x]; y++) {
                    blocks[y][x] = BlockType.NoTop;
                    blocks[y + 1][x] = BlockType.Top;
                }
                //inclined dirt blocks
                if (x > 0 && heightsList[x - 1] - heightsList[x] == 1) {
                    blocks[heightsList[x] + 1][x] = BlockType.InclLeft;
                    blocks[heightsList[x] + 2][x] = BlockType.IncLeftGrassOnly;
                } else {
                    if (x < gridWidth - 1 && heightsList[x + 1] - heightsList[x] == 1) {
                        blocks[heightsList[x] + 1][x] = BlockType.InclRight;
                        blocks[heightsList[x] + 2][x] = BlockType.IncRightGrassOnly;
                    }
                }
            }
        }
    }
}
