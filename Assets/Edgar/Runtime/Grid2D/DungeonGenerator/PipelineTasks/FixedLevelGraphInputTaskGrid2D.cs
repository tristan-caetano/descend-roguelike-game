﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Edgar.GraphBasedGenerator.Grid2D;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Creates an input for the generator from a given level graph.
    /// </summary>
    internal class FixedLevelGraphInputTaskGrid2D : PipelineTask<DungeonGeneratorPayloadGrid2D>
    {
        private readonly FixedLevelGraphConfigGrid2D config;

        public FixedLevelGraphInputTaskGrid2D(FixedLevelGraphConfigGrid2D config)
        {
            this.config = config;
        }

        public override IEnumerator Process()
        {
            if (config.LevelGraph == null)
            {
                throw new ConfigurationException("The LevelGraph field must not be null. Please assign a level graph in the Input config section of the generator component.");
            }

            if (config.LevelGraph.Rooms.Count == 0)
            {
                throw new ConfigurationException($"Each level graph must contain at least one room. Please add some rooms to the level graph called \"{config.LevelGraph.name}\".");
            }

            var levelDescription = new LevelDescriptionGrid2D();

            // Setup individual rooms
            foreach (var room in config.LevelGraph.Rooms)
            {
                var roomTemplates = GetRoomTemplates(room);

                if (roomTemplates.Count == 0)
                {
                    throw new ConfigurationException($"There are no room templates for the room \"{room.GetDisplayName()}\" and also no room templates in the default set of room templates. Please make sure that the room has at least one room template available.");
                }

                levelDescription.AddRoom(room, roomTemplates);
            }

            var typeOfRooms = config.LevelGraph.Rooms.First().GetType();

            // Add passages
            foreach (var connection in config.LevelGraph.Connections)
            {
                if (config.UseCorridors)
                {
                    var corridorRoom = (RoomBase) ScriptableObject.CreateInstance(typeOfRooms);

                    if (corridorRoom is Room basicRoom)
                    {
                        basicRoom.Name = "Corridor";
                    }

                    levelDescription.AddCorridorConnection(connection, corridorRoom,
                        GetRoomTemplates(config.LevelGraph.CorridorRoomTemplateSets, config.LevelGraph.CorridorIndividualRoomTemplates));
                }
                else
                {
                    levelDescription.AddConnection(connection);
                }
            }

            CheckIfDirected(levelDescription);

            Payload.LevelDescription = levelDescription;

            yield return null;
        }

        private void CheckIfDirected(LevelDescriptionGrid2D levelDescription)
        {
            if (config.LevelGraph.IsDirected)
            {
                return;
            }

            foreach (var roomTemplate in levelDescription.GetPrefabToRoomTemplateMapping().Values)
            {
                if (roomTemplate.Doors is ManualDoorModeGrid2D doorMode)
                {
                    var hasDirectedDoors = doorMode.Doors != null && doorMode.Doors.Any(x => x.Type != GraphBasedGenerator.Common.Doors.DoorType.Undirected);
                    var hasDirectedDoorLines = doorMode.DoorLines != null && doorMode.DoorLines.Any(x => x.Type != GraphBasedGenerator.Common.Doors.DoorType.Undirected);

                    if (hasDirectedDoors || hasDirectedDoorLines)
                    {
                        throw new ConfigurationException(
                            $"LevelGraph.IsDirected must be enabled when using entrance-only or exit-only doors. Level graph: \"{config.LevelGraph.name}\", room template: \"{roomTemplate.Name}\"");
                    }
                }
            }
        }

        private List<GameObject> GetRoomTemplates(List<RoomTemplatesSet> roomTemplatesSets, List<GameObject> individualRoomTemplates)
        {
            return individualRoomTemplates
                .Where(x => x != null)
                .Union(roomTemplatesSets
                    .Where(x => x != null)
                    .SelectMany(x => x.RoomTemplates))
                .Distinct()
                .ToList();
        }

        /// <summary>
        ///     Setups room shapes for a given room.
        /// </summary>
        /// <param name="room"></param>
        protected List<GameObject> GetRoomTemplates(RoomBase room)
        {
            var roomTemplates = room.GetRoomTemplates();

            if (roomTemplates == null || roomTemplates.Count == 0)
            {
                return GetRoomTemplates(config.LevelGraph.DefaultRoomTemplateSets, config.LevelGraph.DefaultIndividualRoomTemplates);
            }

            return roomTemplates;
        }
    }
}