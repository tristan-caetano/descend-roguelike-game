﻿using System.Linq;
using UnityEngine;

namespace Edgar.Unity.Examples.Scripts
{
    #region codeBlock:2d_platformer1_postProcessing
    [CreateAssetMenu(menuName = "Edgar/Examples/Platformer 1/Post-processing", fileName = "Platformer1PostProcessing")]
    public class Platformer1PostProcessing : DungeonGeneratorPostProcessingGrid2D
    {
        public override void Run(DungeonGeneratorLevelGrid2D level)
        {
            RemoveWallsFromDoors(level);
        }

        private void RemoveWallsFromDoors(DungeonGeneratorLevelGrid2D level)
        {
            // Get the tilemap that we want to delete tiles from
            var walls = level.GetSharedTilemaps().Single(x => x.name == "Walls");

            // Go through individual rooms
            foreach (var roomInstance in level.RoomInstances)
            {
                // Go through individual doors
                foreach (var doorInstance in roomInstance.Doors)
                {
                    // Remove all the wall tiles from door positions
                    foreach (var point in doorInstance.DoorLine.GetPoints())
                    {
                        walls.SetTile(point + roomInstance.Position, null);
                    }
                }
            }
        }
    }
    #endregion
}
