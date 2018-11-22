using System;

namespace SpaceTaxi_1.GameState {

    public enum GameStateType {
        GameRunning,
        GamePaused,
        GameOver,
        MainMenu,
        MapOption
    }

    public static class StateTransformer {
        
        public static GameStateType TransformStringToState(string state) {
            switch (state) {
            case "GAME_RUNNING":
                return GameStateType.GameRunning;
            case "GAME_PAUSED":
                return GameStateType.GamePaused;
            case "MAIN_MENU":
                return GameStateType.MainMenu;
            case "MAP_OPTION":
                return GameStateType.MapOption;
            case "GAME_OVER":
                return GameStateType.GameOver;
            default:
                throw new ArgumentException("An error occoured, could not parse string {0}", state);
            }
        }

        public static string TransformStateToString(GameStateType state) {
            switch (state) {
            case GameStateType.GameRunning:
                return "GAME_RUNNING";
            case GameStateType.GamePaused:
                return "GAME_PAUSED";
            case GameStateType.MainMenu:
                return "MAIN_MENU";
            case GameStateType.MapOption:
                return "MAP_OPTION";
            case GameStateType.GameOver:
                return "GAME_OVER";
            default:
                throw new ArgumentException("An error occoured, could not parse GameStateType");
            }
        }
    }
}