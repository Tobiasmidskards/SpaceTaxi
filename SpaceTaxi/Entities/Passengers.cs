using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using SpaceTaxi_1;

namespace SpaceTaxi_1.Entities {
    public class Passengers {
        private static Entity passenger;
        private static IBaseImage passengerImage;
        private static EntityContainer<Entity> passengerContainer;

        public Passengers() {
            var imgData = new ImageDatabase();

            Passengers.passengerImage = imgData.CustomerStandLeft;
            Passengers.passengerContainer = new EntityContainer<Entity>();
        }

        public static void UpdatePassenger(Player player) {
            Passengers.passengerContainer.Iterate(delegate(Entity entity) {
                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), entity.Shape)
                    .Collision)
                {


                } else

                {

                }
            });
        }

        public static void SpawnPassenger(Vec2F pos) {
            Passengers.passenger = new Entity(new DynamicShape(pos, new Vec2F(0.1f,0.1f)), Passengers.passengerImage);
            Passengers.passengerContainer.AddDynamicEntity(Passengers.passenger);
        }

        public static void RenderPassenger() {
            Passengers.passengerContainer.RenderEntities();
        }
    }
}