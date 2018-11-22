using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxi_1.GameState;

namespace SpaceTaxi_1
{
    public class Game : IGameEventProcessor<object>
    {
        private Window _win;
        private GameEventBus<object> _eventBus;
        private static GameTimer _gameTimer;
        public StateMachine StateMachine;
 
        public Game()
        {
            // window
            _win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);

            // event bus
            _eventBus = GameBus.GetBus();

            _eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,      // key press / key release
                GameEventType.WindowEvent,     // messages to the window, e.g. CloseWindow()
                GameEventType.PlayerEvent,     // commands issued to the player object, e.g. move, destroy, receive health, etc.
                GameEventType.GameStateEvent   // for changing states.
            });
            _win.RegisterEventBus(_eventBus);

            // game timer
            _gameTimer = new GameTimer(60,60); // 60 UPS, 60 FPS limit

            // StateMachine
            StateMachine = new StateMachine();
           
            // event delegation
            _eventBus.Subscribe(GameEventType.WindowEvent, this);
        }

        public void GameLoop()
        {
            while (_win.IsRunning())
            {
                _gameTimer.MeasureTime();

                while (_gameTimer.ShouldUpdate())
                {
                    _win.PollEvents();
                    _eventBus.ProcessEvents();
                    StateMachine.ActiveState.UpdateGameLogic();
                }

                if (_gameTimer.ShouldRender())
                {
                    _win.Clear();
                    StateMachine.ActiveState.RenderState();
                    _win.SwapBuffers();
                }

                if (_gameTimer.ShouldReset())
                {
                    // 1 second has passed - display last captured ups and fps from the timer
                    _win.Title = "Space Taxi | UPS: " + _gameTimer.CapturedUpdates + ", FPS: " +
                                _gameTimer.CapturedFrames;
                }
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            if (eventType == GameEventType.WindowEvent)
            {
                switch (gameEvent.Message)
                {
                    case "CLOSE_WINDOW":
                        _win.CloseWindow();
                        break;
                }
            }
        }
    }
}
