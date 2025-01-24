// <copyright file="OverlayManager.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Data management class for overlay objects.
    /// </summary>
    internal static class OverlayManager
    {
        //name -> [name,path,format,loop,green screen value]
        //format can be IMAGE, VIDEO, GIF
        //green screen value is either a hex code for the color to remove or NULL 
        //by default images and gifs remain on screen but a video will play only once if the loop value is not set to TRUE
        private static readonly Dictionary<string, string[]> OverlayObjects = new Dictionary<string, string[]>();

        /// <summary>
        /// Read all data for overlay assets.
        /// </summary>
        public static void InitAssets()
        {
            OverlayObjects.Clear();

            string[] assetContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\OverlayList.txt"));

            for (int i = 0; i < assetContent.Length; i++)
            {
                string[] split = assetContent[i].Split(',');

                OverlayObjects[split[0]] = [split[0], split[1], split[2], split[3], split[4]];
            }

            OverlayObjects.TrimExcess();
        }

        /// <summary>
        /// Gets the dictionary of overlay data.
        /// </summary>
        /// <returns>Key value pair of name to data.</returns>
        public static Dictionary<string, string[]> GetOverlayObjects()
        {
            return OverlayObjects;
        }

        /// <summary>
        /// Formats path for program without having to make it worry about IO.
        /// </summary>
        /// <param name="name">name of the overlay asset.</param>
        /// <returns>formatted file path.</returns>
        public static string GetOverlayPath(string name)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\overlays\\", OverlayObjects[name][1]);
        }

        /// <summary>
        /// Gets all the names of the assets.
        /// </summary>
        /// <returns>Array of overlay names for ui to read.</returns>
        public static string[] GetOverlayTitles()
        {
            return OverlayObjects.Keys.ToArray();
        }
    }
}
