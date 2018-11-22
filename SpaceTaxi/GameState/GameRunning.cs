using System;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Entities;
using System.IO;
using DIKUArcade.Physics;
using System.Collections.Generic;
using SpaceTaxi_1.Entities;
using SpaceTaxi_1.MapGeneration;

namespace SpaceTaxi_1.GameState {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Player player;
        private Effects effects;
        private GameEventBus<object> eventBus;

        private Entity backGroundImage;
        private List<EntityContainer<Entity>> mapObjects;

        private Vec2F offsetCol = new Vec2F(0.3f,0.3f);

        public void GameLoop() {
            // den er tom, og skal være det
        }

        public void InitializeGameState() {

            eventBus = GameBus.GetBus();

            // Game Entities
            player = new Player();

            effects = new Effects();

            // game assets
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1f , 1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );

            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            InitializeMapObjects();

        }
        
        public void InitializeMapObjects() {
            if (MapOption.activeMenuButton == 0) {
                mapObjects = MapGenerator.MapCreator(0, player);
            } else {
                mapObjects = MapGenerator.MapCreator(1, player);

            }
        }

        public void UpdateGameLogic() {
            if (!player.Platform) {
                player.MovePlayer();
            }
            CollisionDetector();
            player.CheckBoundaries();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            RenderMap();
            player.RenderPlayer();
            effects.RenderEffects();
        }

        private void RenderMap() {
            for (int i = 0; i < 3; i++) {
                mapObjects[i].RenderEntities();
            }
        }

        private void CollisionDetector() {

            // PLATFORM
            mapObjects[0].Iterate(delegate(Entity entity) {
                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), entity.Shape)
                    .Collision)
                {
                    LandPlatform();
                }
            });

            // OBSTACLE
            mapObjects[1].Iterate(delegate(Entity entity) {
                    if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), entity.Shape)
                        .Collision) {

                        eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_OVER",
                                ""));

                        effects.AddExplosion(player.Entity.Shape.Position, player.Entity.Shape.Extent);
                    }
            });

            // PORTAL
            mapObjects[2].Iterate(delegate(Entity entity) {
                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), entity.Shape)
                    .Collision)
                {
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "NEXT_LEVEL", "",
                            ""));

                    Console.WriteLine("PORTAL");
                }
            });


        }

        public void LandPlatform() {
            if (player.Entity.Shape.AsDynamicShape().Direction.Y < 0 &&
                player.Entity.Shape.AsDynamicShape().Direction.Y > -0.004)
            {
                Console.WriteLine("PLATFORM");
                player.Platform = true;
                player.Entity.Shape.Position.Y -= 0.02f;
                player.Entity.Shape.AsDynamicShape().Direction.Y = 0f;
                player.Entity.Shape.AsDynamicShape().Direction.X = 0f;
            } else {
                Console.WriteLine("CRASH");
                player.Platform = true;
                player.Entity.Shape.AsDynamicShape().Direction.Y = 0f;
                player.Entity.Shape.AsDynamicShape().Direction.X = 0f;
                effects.AddExplosion(player.Entity.Shape.Position, player.Entity.Shape.Extent);

            }

        }


        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
            case "KEY_PRESS":
                switch (keyValue) {
                case "KEY_ESCAPE":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_PAUSED", ""));
                    break;

                case "KEY_LEFT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
                    break;
                case "KEY_RIGHT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
                    break;
                case "KEY_UP":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
                    break;
                }

                break;

            case "KEY_RELEASE":
                switch (keyValue) {
                case "KEY_LEFT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_LEFT", "", ""));
                    break;
                case "KEY_RIGHT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_RIGHT", "", ""));
                    break;

                case "KEY_UP":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));
                    break;
                }

                break;
            }
        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance =
                       new GameRunning());
        }
    }
}