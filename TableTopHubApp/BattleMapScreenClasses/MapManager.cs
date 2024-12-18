﻿// <copyright file="MapManager.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Manager class to open map and icon data files.
    /// </summary>
    internal class MapManager
    {
        //name -> [name,path,x,y]
        private static readonly Dictionary<string, string[]> Maps = new Dictionary<string, string[]>();

        //name -> [name,path,animated?]
        private static readonly Dictionary<string, string[]> Icons = new Dictionary<string, string[]>();

        /// <summary>
        /// Open up the map data file and read all the data inside.
        /// </summary>
        public static void InitMaps()
        {
            string[] mapContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\MapList.txt"));

            for (int i = 0; i < mapContent.Length; i++)
            {
                string[] split = mapContent[i].Split(',');
                Maps[split[0]] = [split[0], split[1], split[2], split[3]];
            }

            string[] iconContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\IconList.txt"));

            for(int i = 0; i < iconContent.Length; i++)
            {
                string[] split = iconContent[i].Split(",");
                Icons[split[0]] = [split[0],split[1],split[2]];
            }
        }

        /// <summary>
        /// Gets the dictionary of map name to data. 
        /// </summary>
        /// <returns>array of map data.</returns>
        public static Dictionary<string, string[]> GetMaps()
        {
            return Maps;
        }

        /// <summary>
        /// Gets the path associated with a specific icon.
        /// </summary>
        /// <param name="name">name of the path.</param>
        /// <returns>string path.</returns>
        public static string GetIconPath(string name)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\icons\\", Icons[name][1]);
        }

        /// <summary>
        /// Bool stating whether icon is a gif or png.
        /// </summary>
        /// <param name="name">name of icon.</param>
        /// <returns>true for gif, false for png.</returns>
        public static bool IsAnimated(string name)
        {
            if (Icons[name][2] == "ANIMATED")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
