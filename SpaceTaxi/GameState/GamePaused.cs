using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Entities;
using System.IO;

namespace SpaceTaxi_1.GameState {
    public class GamePaused : IGameState {
        private static GamePaused instance;
        private int activeMenuButton;

        
        private GameEventBus<object> eventBus;
        private readonly Vec3F gray = new Vec3F(0.5f, 0.5f, 0.5f);
        private readonly Vec3F white = new Vec3F(1f, 1f, 1f);
        private readonly Text[] menuButtons = new Text[2];
        private readonly Text[] titleLabel = new Text[1];
        
        private Entity backGroundImage;

        public void GameLoop() {
            // den er tom, og skal være det
        }


        public void InitializeGameState() {
            activeMenuButton = 0;

            menuButtons[0] = new Text("Resume", new Vec2F(0.3f, -0.05f), new Vec2F(0.5f, 0.5f));
            menuButtons[1] = new Text("Return to Main", new Vec2F(0.3f, -0.15f), new Vec2F(0.5f, 0.5f));
            titleLabel[0] = new Text("Game Paused", new Vec2F(0.24f, 0.05f), new Vec2F(0.6f, 0.7f) );

            menuButtons[0].SetColor(white);
            menuButtons[1].SetColor(gray);
            titleLabel[0].SetColor(white);
            
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1f , 1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );


            eventBus = GameBus.GetBus();
        }

        public void UpdateGameLogic() {
            // den er tom, og skal være det
        }

        public void RenderState() {

            backGroundImage.RenderEntity();
            
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
            titleLabel[0].RenderText();

            if (activeMenuButton == 0) {
                menuButtons[0].SetColor(white);
                menuButtons[1].SetColor(gray);
            } else {
                menuButtons[0].SetColor(gray);
                menuButtons[1].SetColor(white);
            }
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
            case "KEY_PRESS":
                switch (keyValue) {
                case "KEY_UP":
                    activeMenuButton = 0;
                    break;
                case "KEY_DOWN":
                    activeMenuButton = -1;
                    break;
                case "KEY_ENTER":
                    if (activeMenuButton == 0) {
                        eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_RUNNING",
                                ""));
                    } else {
                        eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "MAIN_MENU", ""));
                    }

                    break;
                }

                break;
            case "KEY_RELEASE":
                switch (keyValue) {
                case "Key_UP":
                    break;
                case "Key_DOWN":
                    break;
                }

                break;
            }
        }


        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance =
                       new GamePaused());
        }
    }
}