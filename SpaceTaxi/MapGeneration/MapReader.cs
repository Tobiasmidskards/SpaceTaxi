using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceTaxi_1.MapGeneration {
    public static class MapReader {
        
        public static string[] MapPaths = new string[0];
        public static string[] MapData = new string[0];
        
        public static List<string> ASCIImap = new List<string>();
        public static List<string> MetaData = new List<string>();
        public static List<string> KeysData = new List<string>();
        public static List<string> CustomerData = new List<string>();
        
        public static Dictionary<string, string> KeyDirectory = new Dictionary<string, string>();
        public static List<char> PlatformList = new List<char>();

        public static void Clear() {
            MapReader.MetaData.Clear();
            MapReader.ASCIImap.Clear();
            MapReader.KeysData.Clear();
            MapReader.PlatformList.Clear();
        }

        /// <summary>
        /// Method that stores map file paths from the map directory in an array 
        /// </summary>
        /// <param> () </param>        
        /// <return> void </return>
        public static void MapExtractor() {
            Array.Clear(MapReader.MapPaths, 0, MapPaths.Length);
            string CurrentDirectory = Directory.GetCurrentDirectory();
            string MapDirectory = Path.Combine(CurrentDirectory, "Levels");
            MapPaths = Directory.GetFiles(MapDirectory);  
        }

        /// <summary>
        /// Method that stores all map data from a specfic map file,
        /// from the map directory, in an array.  
        /// </summary>
        /// <param name = MapNumber> number of type int  </param>        
        /// <return> void </return> 
        public static void MapDataExtractor(int MapNumber) {
            Array.Clear(MapData, 0, MapData.Length);
            
            MapExtractor();
            string Map = MapPaths[MapNumber];
            MapData = File.ReadAllLines(Map);
        }

        /// <summary>
        /// Method that stores different kinds of map data, from a specfic map file,
        /// from the map directory, in different lists.  
        /// </summary>
        /// <param name = MapNumber> number of type int  </param>        
        /// <return> void </return> 
        public static void MapDataSplit(int MapNumber) {
          MapReader.Clear();

            int newLines = 0;
            MapDataExtractor(MapNumber);
            foreach (string line in MapData) {
                if (newLines == 0 && line != "") {
                    ASCIImap.Add(line);
                }
                else if (newLines == 1 && line != "" ) {
                    MetaData.Add(line);
                }
                else if (newLines > 1 && line != "") {
                    if (line.Substring(0,8) == "Customer") {
                        break;
                    }
                    KeysData.Add(line);
                }
                else if (line == "") {
                    newLines++;
                }
            }   
        }

        /// <summary>
        /// Method that stores the customer data, from a specific map file,
        /// from the map directory, in a list.
        /// </summary>
        /// <param name = MapNumber> number of type int </param>        
        /// <return> void </return>
        public static void MapCustomerData(int MapNumber) {
            //CustomerData.Clear();

            string Customer = "Customer";
            MapDataExtractor(MapNumber);
            foreach (string line in MapData) {
                if (line.Contains(Customer)) {
                    CustomerData.Add(line);
                }
            }
        }
        
        /// <summary>
        /// Method that adds keys and values to a directory,
        /// by iterating through the Keys list
        /// </summary>
        /// <param name = MapNumber> number of type int </param>        
        /// <return> void </return> 
        public static void AddKey(int MapNumber) {
            KeyDirectory.Clear();

            MapDataSplit(MapNumber);
            for (int i = 0; i < KeysData.Count; i++) {
                List<string> tmp = new List<string>();
                tmp.Add(KeysData[i].Substring(0, 1));
                tmp.Add(KeysData[i].Substring(3, KeysData[i].Length -3));
                KeyDirectory.Add(tmp[0], tmp[1]);
            }

            for (int i = 0; i < MetaData.Count; i++) {
                if (MapReader.MetaData[i].Substring(0, 8) == "Platform") {
                    var tmpString = MapReader.MetaData[i]
                        .Substring(11, MapReader.MetaData[i].Length - 11);

                    foreach (var j in tmpString) {
                        if (j != ',' && j != ' ') {
                            MapReader.PlatformList.Add(j);
                        }
                    }
                }

            }
        }
    }
}