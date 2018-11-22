using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_1.Entities;
using SpaceTaxi_1.MapElements;

namespace SpaceTaxi_1.MapGeneration {
    public static class MapGenerator {

//        private const int MAP_WIDTH = 40;
//        private const int MAP_HEIGHT = 23;

        /// <summary>
        /// Method that creates the necessary Entitys for drawing a map,
        /// and adding them to a list of EntityContainers
        /// </summary>
        /// <param name="levelNumber"> Number of type int </param>
        /// <param name = "_player"> Generic object of type Player </param>
        /// <return name = "Map"> List of EntityContainers </return>
        public static List<EntityContainer<Entity>> MapCreator(int levelNumber, Player _player) {
            
            EntityContainer<Entity> obstacles = new EntityContainer<Entity>();
            EntityContainer<Entity> portals = new EntityContainer<Entity>();
            EntityContainer<Entity> platforms = new EntityContainer<Entity>();
            List<EntityContainer<Entity>> mapParts = new List<EntityContainer<Entity>>();
 
            MapReader.AddKey(levelNumber);

            for (int i = 0; i < MapReader.ASCIImap.Count; i++) {
                string line = MapReader.ASCIImap[i];
                for (int j = 0; j < line.Length; j++) {

                    if (line[j] == ' ') {continue; }

                    if (line[j] == '^') {
                        var Portal = new Portal(new StationaryShape(new Vec2F(j / 40f, 22/23f - i / 23f),
                            new Vec2F(1 / 40f, 1 / 23f)));
                        portals.AddStationaryEntity(Portal);
                    }
                    else if (line[j] == '>') {
                        _player.SetPosition(j / 40f, 1 - i / 23f);
                        _player.SetExtent(0.1f, 0.1f);
                    }
                    else if (MapReader.PlatformList.Contains(line[j]))
                    {
                        var platform = new Platform(
                            new StationaryShape(new Vec2F(j / 40f, 22/23f - i / 23f),
                                new Vec2F(1 / 40f, 1 / 23f)),
                            new Image(Path.Combine("Assets", "Images",
                                MapReader.KeyDirectory[line[j].ToString()]))
                        );
                        platforms.AddStationaryEntity(platform);
                    }
                    else {
                        var obstacle = new Obstacle(
                            new StationaryShape(new Vec2F(j / 40f, 22/23f - i / 23f),
                                new Vec2F(1 / 40f, 1 / 23f)),
                            new Image(Path.Combine("Assets", "Images",
                                MapReader.KeyDirectory[line[j].ToString()]))
                            );
                        obstacles.AddStationaryEntity(obstacle);

                    }

                }
            }

            mapParts.Add(platforms);
            mapParts.Add(obstacles);
            mapParts.Add(portals);

            return mapParts;
        }

    }
}