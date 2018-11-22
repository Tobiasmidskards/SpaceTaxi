using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using DIKUArcade;
using NUnit.Framework;
using SpaceTaxi_1;
using SpaceTaxi_1.GameState;
using SpaceTaxi_1.MapGeneration;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using SpaceTaxi_1.Entities;

namespace Tests {
    [SetUpFixture]
    public class MySetUpClass
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var dir = Path.GetDirectoryName(typeof(MySetUpClass).Assembly.Location);
            Environment.CurrentDirectory = dir;

            // or
            Directory.SetCurrentDirectory(dir);

        }
    }

    [TestFixture]
    public class Tests {

        public Tests()
        {
//            var dir = Path.GetDirectoryName(typeof(Tests).Assembly.Location);
//            Environment.CurrentDirectory = dir;

            // or
            //Directory.SetCurrentDirectory(dir);
        }

        [SetUp]
        public void SetUp() {
            Window.CreateOpenGLContext();
        }
    
        [Test]
        public void MapDataExtractorTest() {
            MapReader.MapDataExtractor(1);
            
            string[] testArrCompare = new[] {
                "%#%#%#%#%#%#%#%#%#^^^^^^%#%#%#%#%#%#%#%#",
                "#               JW      JW             %",
                "%      h2g                             #",
                string.Empty,
                "Name: SHORT -N- SWEET",
                "Platforms: 1",
                string.Empty,
                "%) white-square.png",
                "#) ironstone-square.png",
                string.Empty,
                "1) neptune-square.png",
                string.Empty,
                "Customer: Alice 10 1 ^J 10 100",
                "Customer: Alice 10 1 ^J 10 100",
                string.Empty
            };
            
            Assert.AreEqual(MapReader.MapData, testArrCompare);
        }

        [Test]
        public void MapDataSplitTest() {
            List<string> ASCIImap = new List<string>(new string[] {
                "%#%#%#%#%#%#%#%#%#^^^^^^%#%#%#%#%#%#%#%#",
                "#               JW      JW             %",
                "%      h2g                             #"
            });
            
            List<string> MetaData = new List<string>(new string[] {
                "Name: SHORT -N- SWEET",
                "Platforms: 1"
            });
            
            List<string> KeysData = new List<string>(new string[] {
                "%) white-square.png",
                "#) ironstone-square.png",
                "1) neptune-square.png"
            });
        
            MapReader.MapDataSplit(1);
            
            Assert.AreEqual(MapReader.ASCIImap, ASCIImap);
            Assert.AreEqual(MapReader.MetaData, MetaData);
            Assert.AreEqual(MapReader.KeysData, KeysData);
            
        }

        [Test]
        public void MapCustomerDataTest() {
            List<string> CustormerData = new List<string>(new string[] {
                "Customer: Alice 10 1 ^J 10 100",
                "Customer: Alice 10 1 ^J 10 100"
            });
            
            MapReader.MapCustomerData(1);
            
            Assert.AreEqual(MapReader.CustomerData, CustormerData);
        }

        [Test]
        public void AddKeyTest() {
          Dictionary<string, string> KeyDirectory = new Dictionary<string, string>();
            KeyDirectory.Add("%", "white-square.png");
            KeyDirectory.Add("#", "ironstone-square.png");
            KeyDirectory.Add("1", "neptune-square.png");
          
            MapReader.AddKey(1);
  
            Assert.AreEqual(MapReader.KeyDirectory, KeyDirectory);
        }

        [Test]
        public void MapGeneratorTest() {
            //Edit in the testMap, such that only 
            //3 enities are created - i.e one obstacle,
            //one portal, and one platform
            
            //Then this test will be easy to make
        }

        [Test]
        public void InitializeMapObjectsTest() {
            //Fill this list up with the correct entityContainers  from the testMap
//            List<EntityContainer<Entity>> mapParts = new List<EntityContainer<Entity>>();
//            GameRunning test = new GameRunning();
//            MapOption.activeMenuButton = 1;
//            test.InitializeMapObjects();
            
            //I cant reach the _mapObjects list of Entitycontainers since it is private...
            //The Assert.Areequal method is not going to work here
        }

        [Test]
        public void PlayerMoveLeftTest() {
            var _player = new Player();

            var pos1 = _player.Entity.Shape.Position.X;

            _player.ProcessEvent(GameEventType.PlayerEvent, GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));

            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();

            var pos2 = _player.Entity.Shape.Position.X;

            Assert.Greater(pos1,pos2);
        }

        [Test]
        public void PlayerMoveRightTest() {
            var _player = new Player();

            var pos1 = _player.Entity.Shape.Position.X;

            _player.ProcessEvent(GameEventType.PlayerEvent, GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));

            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();

            var pos2 = _player.Entity.Shape.Position.X;

            Assert.Greater(pos2,pos1);
        }

        [Test]
        public void PlayerMoveUpDirectionTest() {
            var _player = new Player();


            // POS1 is the startingpoint we want to relate to.
            var pos1 = _player.Entity.Shape.AsDynamicShape().Direction.Y;

            _player.ProcessEvent(GameEventType.PlayerEvent, GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));

            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();

            // POS2 is when we have accelerated up.
            var pos2 = _player.Entity.Shape.AsDynamicShape().Direction.Y;
            Assert.Greater(pos2,pos1);

            // Starting to descent.
            _player.ProcessEvent(GameEventType.PlayerEvent ,GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));


            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();

            // POS3 is when we havent pressed anything for a while. POS3 should be lower than POS2.
            var pos3 = _player.Entity.Shape.AsDynamicShape().Direction.Y;

            Assert.Greater(pos2,pos3);



        }

        [Test]
        public void PlayerGravityTest() {
            var _player = new Player();

            var pos1 = _player.Entity.Shape.Position.Y;

            _player.MovePlayer();
            _player.MovePlayer();
            _player.MovePlayer();

            var pos2 = _player.Entity.Shape.Position.Y;

            //Position must be less, than the startingpoint.
            Assert.Greater(pos1,pos2);
        }

        [Test]
        public void ChangeStateMainMenuToMapOptionTest() {
            var _stateMachine = new StateMachine();

            var state1 = _stateMachine.ActiveState;

            _stateMachine.ProcessEvent(GameEventType.GameStateEvent, GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent, this, "CHANGE_STATE", "MAP_OPTION",
                ""));

            var state2 = _stateMachine.ActiveState;

            Assert.AreEqual(state1, MainMenu.GetInstance());
            Assert.AreEqual(state2, MapOption.GetInstance());

        }


    }
}