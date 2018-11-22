using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Entities;
using System.IO;

namespace SpaceTaxi_1.GameState {
    public class GameOver : IGameState {
        private static GameOver instance;
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

            menuButtons[0] = new Text("New Game", new Vec2F(0.3f, -0.05f), new Vec2F(0.5f, 0.5f));
            menuButtons[1] = new Text("Quit", new Vec2F(0.3f, -0.15f), new Vec2F(0.5f, 0.5f));
            titleLabel[0] = new Text("Game Over", new Vec2F(0.24f, 0.05f), new Vec2F(0.6f, 0.7f) );

            menuButtons[0].SetColor(white);
            menuButtons[1].SetColor(white);
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
            titleLabel[0].RenderText();
            
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();

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
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "MAP_OPTION",
                                ""));
                    } else {
                        eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
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


        public static GameOver GetInstance() {
            return GameOver.instance ?? (GameOver.instance =
                       new GameOver());
        }
    }
}