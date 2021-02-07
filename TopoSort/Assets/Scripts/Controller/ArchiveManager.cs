using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



namespace TopoSort.Controller
{
    
    /*
     * manages the archive and the file handling
     *
     * the save file is structured as follows:
     *
     * Planets take up 4 and atmospheres 2 bytes of space.
     * every bit represents either a planet or an atmosphere (hereinafter referred to as an "entity").
     * An entity is considered discovered iff its respective bit is set to 1.
     *
     * currently the file can store up to 48 entities (32 planets / 16 atmospheres)
     */
    public class ArchiveManager
    {
 
        // both dictionaries hold the sprites for their category respectively
        private static Dictionary<string, bool> DiscoveredPlanets = new Dictionary<string, bool>();
        private static Dictionary<string, bool> DiscoveredAtmosphere = new Dictionary<string, bool>();
        
        // Name of the save file; can be altered in the unity inspector
        private static string SaveFile = "discoveries";

        
        /*
         * used to initialize the dictionaries with keys and default values
         * and loading the current save data afterwards
         */
        static ArchiveManager()
        {
            InitDictionaries();
            
            LoadDataFromFile();
        }


        /*
         * used to initialize the dictionaries with default values;
         */
        private static void InitDictionaries()
        {
            foreach (string type in Planet.AvailablePlanets)
            {
                DiscoveredPlanets[type] = type == "Default";        // The planet type "Default" is set to true by default
            }

            foreach (string type in Atmosphere.AvailableAtomspheres)
            {
                DiscoveredAtmosphere[type] = false;
            }
        }


        public static void ResetDiscoveries()
        {
            InitDictionaries();
            WriteDataToFile();
        }
        

        /*
         * checks whether or not a planet is currently discovered.
         */
        public static bool IsPlanetDiscovered(string name)
        {
            if (DiscoveredPlanets.ContainsKey(name))
            {
                return DiscoveredPlanets[name];
            }
            return false;
        }
        
        /*
         * checks whether or not an atmosphere is currently discovered.
         */
        public static bool IsAtmosphereDiscovered(string name)
        {
            if (DiscoveredAtmosphere.ContainsKey(name))
            {
                return DiscoveredAtmosphere[name];
            }
            return false;
        }
        
        
        /*
         * checks if the planet is new.
         * returns true, if it is, false, if not
         */
        public static bool checkPlanet(string planetType)
        {
            if (IsPlanetDiscovered(planetType) || !DiscoveredPlanets.ContainsKey(planetType))
            {
                return false;
            }

            DiscoveredPlanets[planetType] = true;

            return true;
        }
        
        
        /*
         * checks if the atmosphere is discovered
         * returns true, if it is, false if not
         */
        public static bool checkAtmosphere(string atmosphereType)
        {
            if (atmosphereType == null)
            {
                return false;
            }
            if (IsAtmosphereDiscovered(atmosphereType) || !DiscoveredAtmosphere.ContainsKey(atmosphereType))
            {
                return false;
            }

            DiscoveredAtmosphere[atmosphereType] = true;
            
            return true;
        }

        /*
         * this method firstly checks whether or not a new planet or a new atmosphere is discovered
         * in the case of this being true, the new save data is written to the save file specified by the name above.
         */
        public static void checkResult(string planetType, string atmosphereType)
        {
            if (checkPlanet(planetType) | checkAtmosphere(atmosphereType))
            {
                WriteDataToFile();
            }
        }
        
        /*
         * obtains data from the save file or creates it if it's absent in the first place.
         */
        public static void LoadDataFromFile()
        {
            if (File.Exists(SaveFile))
            {
                FileStream file = File.Open(SaveFile, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(file);

                LoadPlanets(reader.ReadBytes(4));       
                LoadAtmosphere(reader.ReadBytes(2));
            
                reader.Close();
                file.Close();
                
                Debug.Log("Discoveries loaded");
            }
        }
        
        
        /*
         * writes data to the save file according to the layout described beforehand
         */
        public static void WriteDataToFile()
        {

            FileStream file;
            
            if (! File.Exists(SaveFile))
            {
                file = File.Create(SaveFile);
            }
            else
            {
                file = File.Open(SaveFile, FileMode.Open, FileAccess.Write);
            }
            
            BinaryWriter writer = new BinaryWriter(file);
            
            writer.Write(StorePlanets());
            writer.Write(StoreAtmosphere());
            
            writer.Close();
            file.Close();
            
            Debug.Log("Discoveries Saved!");
        }
        
        
        /*
         * populates a byte Array with a length of 4 with the given planet data
         */
        private static byte[] StorePlanets()
        {
            string tmp;
            int bytePos, bitPos;
            byte[] result = new byte[4];
            byte value;
            
            for (int i = 0; i < Planet.AvailablePlanets.Count; i++)
            {
                tmp = Planet.AvailablePlanets[i];
                bytePos = i / 8;
                bitPos = i % 8;
                value = (byte) (DiscoveredPlanets[tmp] ? 0x01 : 0x00);
                result[bytePos] |= (byte) (value << bitPos);
            }
            
            return result;
        }
        
        /*
         * populates a byte Array with a length of 2 with the given atmosphere data
         */
        private static byte[] StoreAtmosphere()
        {
            string tmp;
            int bytePos, bitPos;
            byte[] result = new byte[2];
            byte value;
            
            for (int i = 0; i < Atmosphere.AvailableAtomspheres.Count; i++)
            {
                tmp = Atmosphere.AvailableAtomspheres[i];
                bytePos = i / 8;
                bitPos = i % 8;
                value = (byte) (DiscoveredAtmosphere[tmp] ? 0x01 : 0x00);
                result[bytePos] |= (byte) (value << bitPos);
            }
            
            return result;
        }
        
        

        /*
         * extracts the seeked information out of the given byte Array and sets the Dictionary's values.
         */
        private static void LoadPlanets(byte[] data)
        {
            int index;
            string tmp;
            for (int bytePos = 0; bytePos < data.Length; bytePos++)
            {
                for (int bitPos = 0; bitPos < 8; bitPos++)
                {
                    index = bytePos * 8 + bitPos;
                    if (index < Planet.AvailablePlanets.Count)
                    {
                        tmp = Planet.AvailablePlanets[index];
                        DiscoveredPlanets[tmp] = (data[bytePos] & (0x01 << bitPos)) > 0 ? true : false;
                    }
                }
            }
        }
        
        /*
         * works like LoadPlanets but operates on the Atmosphere-Dictionary
         */
        private static void LoadAtmosphere(byte[] data)
        {
            int index;
            string tmp;
            for (int bytePos = 0; bytePos < data.Length; bytePos++)
            {
                
                for (int bitPos = 0; bitPos < 8; bitPos++)
                {
                    index = bytePos * 8 + bitPos;
                    if (index < Atmosphere.AvailableAtomspheres.Count)
                    {
                        tmp = Atmosphere.AvailableAtomspheres[index];
                        DiscoveredAtmosphere[tmp] = (data[bytePos] & (0x01 << bitPos)) > 0 ? true : false;
                    }
                }
            }
        }
        
        
        
    }
}