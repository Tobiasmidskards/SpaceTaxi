using System.IO;
using DIKUArcade.Graphics;

namespace SpaceTaxi_1
{
    public class ImageDatabase
    {
        public readonly IBaseImage PlayerTaxiThrustBack = new ImageStride(1000,
            ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png")));
        public readonly IBaseImage PlayerTaxiThrustBackRight = new ImageStride(1000,
            ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png")));

        public readonly IBaseImage PlayerTaxiThrustBottom = new ImageStride(1000,
            ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png")));
        public readonly IBaseImage PlayerTaxiThrustBottomBack = new ImageStride(1000,
            ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png")));

        public readonly IBaseImage PlayerTaxiThrustBottomRight = new ImageStride(1000,
            ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png")));
        public readonly IBaseImage PlayerTaxiThrustBottomBackRight = new ImageStride(1000,
            ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png")));

        public readonly IBaseImage Explosion = new ImageStride(80,
            ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png")));

        public readonly IBaseImage CustomerStandLeft = new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));

        public readonly IBaseImage CustomerStandRight = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
    }
}