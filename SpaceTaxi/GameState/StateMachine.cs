using System;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using SpaceTaxi_1.GameState;

namespace SpaceTaxi_1.GameState {
    public class StateMachine: IGameEventProcessor<object> {
        public StateMachine() {
            GameBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GameBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            ActiveState = MainMenu.GetInstance();
            ActiveState.InitializeGameState();

        }

        public IGameState ActiveState { get; private set; }

        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                ActiveState.InitializeGameState();
                break;
            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                ActiveState.InitializeGameState();
                break;
            case GameStateType.MapOption:
                ActiveState = MapOption.GetInstance();
                ActiveState.InitializeGameState();
                break;
            case GameStateType.GameOver:
                ActiveState = GameOver.GetInstance();
                ActiveState.InitializeGameState();
                break;
            case GameStateType.GameRunning:
                if (Object.ReferenceEquals(ActiveState, GamePaused.GetInstance())) {
                    ActiveState = GameRunning.GetInstance();
                } else {
                    ActiveState = GameRunning.GetInstance();
                    ActiveState.InitializeGameState();

                }

                break;

            }
        }
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            switch (eventType)
            {
                case GameEventType.InputEvent:
                    ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                    break;
                
                case GameEventType.GameStateEvent:
                    if (gameEvent.Message == "CHANGE_STATE") 
                    {
                        SwitchState(StateTransformer.TransformStringToState(gameEvent.Parameter1));
                    }
                    break;
            }
        }
    }
}