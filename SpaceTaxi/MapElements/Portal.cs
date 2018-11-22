using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace SpaceTaxi_1.MapElements {
    public class Portal: Entity {
        public Portal(Shape shape) : base(shape, new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))) {
            //ggg
        }
    }
}