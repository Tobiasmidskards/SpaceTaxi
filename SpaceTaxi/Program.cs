using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using SpaceTaxi_1.MapGeneration;

namespace SpaceTaxi_1
{
    internal class Program
    {
        public static void Main(string[] args) {
            
//            MapReader.MapExtractor();
//
//            for (int i = 0; i < MapReader.MapPaths.Length; i++) {
//                Console.WriteLine(MapReader.MapPaths[i]);
//            }
            
            Game game = new Game();
            game.GameLoop();

//MapReader.MapDataExtractor(0);
//            for (int i = 0; i < MapReader.MapData.Length; i++) {
//                Console.WriteLine("k" + MapReader.MapData[i]);
//            }

        }
    }
}
