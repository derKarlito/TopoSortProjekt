using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TopoSort.Controller
{
    public class ArchiveManager
    {
 
        private static Dictionary<string, bool> DiscoveredPlanets = new Dictionary<string, bool>();
        private static Dictionary<string, bool> DiscoveredAtmosphere = new Dictionary<string, bool>();
        
        private static string SaveFile = "discoveries";


        static ArchiveManager()
        {
            foreach (string type in Planet.AvailablePlanets)
            {
                DiscoveredPlanets[type] = type == "Default";
            }

            foreach (string type in Atmosphere.AvailableAtomspheres)
            {
                DiscoveredAtmosphere[type] = false;
            }
            
            LoadDataFromFile();
        }




        public static bool IsPlanetDiscovered(string name)
        {
            if (DiscoveredPlanets.ContainsKey(name))
            {
                return DiscoveredPlanets[name];
            }
            return false;
        }
        
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

        public static void checkResult(string planetType, string atmosphereType)
        {
            if (checkPlanet(planetType) || checkAtmosphere(atmosphereType))
            {
                WriteDataToFile();
            }
        }
        
        
        public static void LoadDataFromFile()
        {
            if (File.Exists(SaveFile))
            {
                BinaryReader reader = new BinaryReader(File.Open(SaveFile, FileMode.Open, FileAccess.Read));

                LoadPlanets(reader.ReadBytes(4));
                LoadAtmosphere(reader.ReadBytes(2));
            
                reader.Close();
            }
            else
            {
                File.Create(SaveFile);
            }
        }
        
        
        
        public static void WriteDataToFile()
        {
            
            if (! File.Exists(SaveFile))
            {
                File.Create(SaveFile);
            }
            
            BinaryWriter writer = new BinaryWriter(File.Open(SaveFile, FileMode.Open, FileAccess.Write));
            
            writer.Write(StorePlanets());
            writer.Write(StoreAtmosphere());
            
            writer.Close();
            
            Debug.Log("Discoveries Saved!");
        }
        
        

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