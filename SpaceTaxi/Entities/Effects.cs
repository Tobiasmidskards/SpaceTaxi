using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1.Entities {
    public class Effects {
        ImageDatabase ImgData = new ImageDatabase();
        private ImageStride explosionStride;
        private Entity explosion;
        private EntityContainer<Entity> explosionContainer;
        private int counter;
        private int TIME = 2 * 60;

        public Effects() {
            explosionStride = (ImageStride) ImgData.Explosion;
            explosionContainer = new EntityContainer<Entity>();
            counter = TIME;
        }

        public void AddExplosion(Vec2F pos, Vec2F ext) {
            explosion = new Entity(new StationaryShape(pos, ext), explosionStride);
            explosionContainer.AddStationaryEntity(explosion);
            counter = TIME;
        }

        public void RenderEffects() {
            if (explosionContainer.CountEntities() > 0 && counter > 0) {
                explosionContainer.RenderEntities();
                counter--;
            } else {
                explosionContainer.ClearContainer();
                counter = 0;
            }
        }

    }
}