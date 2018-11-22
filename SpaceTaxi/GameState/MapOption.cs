using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DIKUArcade.State;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace SpaceTaxi_1 {
    public class MapOption: IGameState {
        
        private static MapOption instance;
        public static int activeMenuButton;
        private Entity _backGroundImage;
        private Text[] menuButtons = new Text[3];
        private GameEventBus<object> _eventBus;
        private Vec3F White = new Vec3F(1f, 1f, 1f);
        private Vec3F Grey = new Vec3F(0.5f, 0.5f, 0.5f);

        public void GameLoop() {
            //den er tom, og det skal den være 
        }
        
        public void InitializeGameState() {
            //game assets
            _backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );

            activeMenuButton = 0;
            
            menuButtons[0] = new Text("The beach", new Vec2F(0.25f, 0.05f), new Vec2F(0.5f, 0.5f));
            menuButtons[1] = new Text("Short n sweet", new Vec2F(0.25f, -0.05f), new Vec2F(0.5f, 0.5f));
            menuButtons[2] = new Text("Return to Main", new Vec2F(0.25f, -0.15f), new Vec2F(0.5f, 0.5f));

            
            menuButtons[0].SetColor(White);
            menuButtons[1].SetColor(Grey);
            menuButtons[2].SetColor(Grey);

            _eventBus = GameBus.GetBus();
        }
        
        public void UpdateGameLogic() {
          
        }
        
        public void RenderState() {
            _backGroundImage.RenderEntity();
            
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
            menuButtons[2].RenderText();

            if (activeMenuButton == 0) {
                menuButtons[0].SetColor(White);
                menuButtons[1].SetColor(Grey);
                menuButtons[2].SetColor(Grey);
            } 
            else if (activeMenuButton == -1) {
                menuButtons[0].SetColor(Grey);
                menuButtons[1].SetColor(White);
                menuButtons[2].SetColor(Grey);
            } 
            else {
                menuButtons[0].SetColor(Grey);
                menuButtons[1].SetColor(Grey);
                menuButtons[2].SetColor(White);
            }
        }
        
        public void HandleKeyEvent(string keyValue, string keyAction) {
            // laves om til den samme som der samme som MainMenu
            switch (keyAction) {
            case "KEY_PRESS":
                switch (keyValue) {

                case "KEY_UP":
                    activeMenuButton += 1;
                    break;

                case "KEY_DOWN":
                    activeMenuButton -= 1;
                    break;

                case "KEY_ENTER":

                    if (activeMenuButton == 0) {
                        _eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_RUNNING",
                                ""));
                    } 
                    
                    else if (activeMenuButton == -1) {
                        _eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_RUNNING", 
                                ""));
                    } 
                    
                    else {
                        _eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "MAIN_MENU", 
                                ""));
                    }
                

                    break;
                }


                break;

            }
        }
        
        public static MapOption GetInstance() {
            return MapOption.instance ?? (MapOption.instance =
                       new MapOption());
        }
        
        
    }
}