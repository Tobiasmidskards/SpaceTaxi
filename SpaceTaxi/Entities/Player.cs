using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1.Entities
{
    public class Player : IGameEventProcessor<object>
    {
        public Entity Entity { get; private set; }

        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private Orientation taxiOrientation;

        private const float ACCELERATION_X = 0.0003f;
        private const float ACCELERATION_Y = 0.00035f;
        private const float ACCELERATION_GRAVITY = -0.00006f;

        private float playerVelocityX = 0.0f;
        private float playerVelocityY = 0.0f;

        public bool Platform;

        private bool left;
        private bool right;
        private bool up;

        private readonly ImageStride leftStride;
        private readonly ImageStride rightStride;
        private readonly ImageStride upLeftStride;
        private readonly ImageStride upRightStride;
        private readonly ImageStride thrustBottom;
        private readonly ImageStride thrustBottomRight;


        public Player()
        {
            var imageDataBase = new ImageDatabase();

            taxiBoosterOffImageLeft = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            taxiBoosterOffImageRight = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

            leftStride = (ImageStride) imageDataBase.PlayerTaxiThrustBack;
            rightStride = (ImageStride) imageDataBase.PlayerTaxiThrustBackRight;
            upLeftStride = (ImageStride) imageDataBase.PlayerTaxiThrustBottomBack;
            upRightStride = (ImageStride) imageDataBase.PlayerTaxiThrustBottomBackRight;
            thrustBottom = (ImageStride) imageDataBase.PlayerTaxiThrustBottom;
            thrustBottomRight = (ImageStride) imageDataBase.PlayerTaxiThrustBottomRight;

            Entity = new Entity(new DynamicShape(new Vec2F(), new Vec2F(), new Vec2F()), taxiBoosterOffImageLeft);

        }

        public void SetPosition(float x, float y)
        {
            Entity.Shape.Position.X = x;
            Entity.Shape.Position.Y = y;
        }

        public void SetExtent(float width, float height)
        {
            Entity.Shape.Extent.X = width;
            Entity.Shape.Extent.Y = height;
        }

        public void RenderPlayer()
        {
            if (taxiOrientation == Orientation.Left)
            {
                if (up && left) {
                    Entity.Image = upLeftStride;
                } else if (up) {
                    Entity.Image = thrustBottom;
                } else if (left) {
                    Entity.Image = leftStride;
                } else {
                    Entity.Image = taxiBoosterOffImageLeft;
                }
            }
            else
            {
                if (up && right) {
                    Entity.Image = upRightStride;
                } else if (up) {
                    Entity.Image = thrustBottomRight;
                } else if (right) {
                    Entity.Image = rightStride;
                } else {
                    Entity.Image = taxiBoosterOffImageRight;
                }
            }

            Entity.RenderEntity();

        }


        private void MoveLeft() {
            playerVelocityX -= Player.ACCELERATION_X;
            Entity.Shape.AsDynamicShape().Direction.X = playerVelocityX;
        }

        private void MoveRight() {
            playerVelocityX += Player.ACCELERATION_X;
            Entity.Shape.AsDynamicShape().Direction.X = playerVelocityX;
        }

        private void MoveUp() {
            playerVelocityY = System.Math.Min(Player.ACCELERATION_Y,
                playerVelocityY + Player.ACCELERATION_Y);

            Entity.Shape.AsDynamicShape().Direction.Y += playerVelocityY;
        }

        public void CheckBoundaries()
        {
            if (Entity.Shape.Position.X <= 0.0f)
            {
                Entity.Shape.Position.X = 0.0f;
                StopMovingLeft();
            }
            else if (Entity.Shape.Position.X >= 1.0f - Entity.Shape.Extent.X)
            {
                Entity.Shape.Position.X = 1.0f - Entity.Shape.Extent.X;
                StopMovingRight();
            }
            if (Entity.Shape.Position.Y <= 0.0f) {
                Entity.Shape.Position.Y = 0.0f;
                StopMovingDown();
            }
            else if (Entity.Shape.Position.Y >= 1.0f - Entity.Shape.Extent.Y) {
                Entity.Shape.Position.Y = 1.0f - Entity.Shape.Extent.Y;
                StopMovingUp();
            }
        }


        public void MovePlayer() {

            if (left) {
                MoveLeft();
            }

            if (right) {
                MoveRight();
            }

            if (up) {
                MoveUp();
            }

            Entity.Shape.AsDynamicShape().Direction.Y += Player.ACCELERATION_GRAVITY;
            Entity.Shape.Move();
        }

        private void StopMoving() {
            Entity.Shape.AsDynamicShape().Direction.X = 0.0f;
            Entity.Shape.AsDynamicShape().Direction.Y = 0.0f;
            playerVelocityX = 0;
        }

        private void StopMovingLeft() {
            if (Entity.Shape.AsDynamicShape().Direction.X < 0.0f) {
                StopMoving();
            }
        }

        private void StopMovingRight() {
            if (Entity.Shape.AsDynamicShape().Direction.X > 0.0f) {
                StopMoving();
            }
        }

        private void StopMovingUp() {
            if (Entity.Shape.AsDynamicShape().Direction.Y > 0.0f) {
                StopMoving();
            }
        }

        private void StopMovingDown() {
            if (Entity.Shape.AsDynamicShape().Direction.Y < 0.0f) {
                StopMoving();
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {

            if (eventType == GameEventType.PlayerEvent)
            {
                switch (gameEvent.Message) {
                case "BOOSTER_TO_LEFT":
                    left = true;
                    taxiOrientation = Orientation.Left;
                    break;

                case "BOOSTER_TO_RIGHT":
                    right = true;
                    taxiOrientation = Orientation.Right;
                    break;

                case "BOOSTER_UPWARDS":
                    up = true;
                    Platform = false;
                    MoveUp();
                    break;

                case "STOP_ACCELERATE_UP":
                    up = false;
                    break;

                case "STOP_ACCELERATE_LEFT":
                    left = false;
                    break;

                case "STOP_ACCELERATE_RIGHT":
                    right = false;
                    break;
                }
            }
        }
    }
}
