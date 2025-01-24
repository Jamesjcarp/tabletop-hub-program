﻿// <copyright file="FileManager.cs" company="StaticSnap">
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
    using Microsoft.Win32;

    /// <summary>
    /// File Manager class handles all of the IO associated with writing xaml data for resources and copying them to resources folder upon selection.
    /// </summary>
    internal static class FileManager
    {
        private static string currentFilePath = string.Empty;
        private static string currentIntroPath = string.Empty;

        /// <summary>
        /// Gets the current file path.
        /// </summary>
        public static string CurrentFilePath
        {
            get { return currentFilePath; }
        }

        /// <summary>
        /// Gets the current intro path.
        /// </summary>
        public static string CurrentIntroPath
        {
            get { return currentIntroPath; }
        }

        /// <summary>
        /// Sets the paths in storage to emptry string for resetting.
        /// </summary>
        public static void ClearBuffers()
        {
            currentFilePath = string.Empty;
            currentIntroPath = string.Empty;
        }

        /// <summary>
        /// Opens the file dialog and logs the result.
        /// </summary>
        /// <param name="type">the types of files allowed.</param>
        /// <returns>true on success.</returns>
        /// <exception cref="Exception">throw an exception when unsuported type is encountered.</exception>
        public static bool OpenFile(string type)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if(type == "icon")
            {
                dialog.Filter = "Image Files|*.bmp;*.jpeg;*.jpg;*.png;*.gif";
            }
            else if(type == "music" | type == "sound")
            {
                dialog.Filter = "Audio Files|*.mp3;*.wav;*.mp2";
            }
            else if(type == "overlay")
            {
                dialog.Filter = "Visual Files|*.bmp;*.jpeg;*.jpg;*.png;*.gif;*.mp4;*.mov";
            }
            else if(type == "map")
            {
                dialog.Filter = "Image Files|*.bmp;*.jpeg;*.jpg;*.png";
            }
            else
            {
                currentFilePath = string.Empty;
                throw new Exception("invalid file open option");
            }

            if(dialog.ShowDialog() == true)
            {
                currentFilePath = dialog.FileName;

                // no comas allowed punk
                if (currentFilePath.Contains(',')) 
                {
                    currentFilePath = string.Empty;
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Second version of the open file method used for only the intor file as you need tyo store two files.
        /// </summary>
        /// <returns>true if successful.</returns>
        public static bool OpenIntroFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Audio Files|*.mp3;*.wav;*.mp2";

            if (dialog.ShowDialog() == true)
            {
                currentIntroPath = dialog.FileName;

                // no comas allowed punk
                if (currentIntroPath.Contains(','))
                {
                    currentIntroPath = string.Empty;
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the file from it's origional location to the resources folder for later use.
        /// </summary>
        /// <param name="type">the type of resource determines where it will be placed.</param>
        public static bool CopyFile(string type)
        {
            string finalLocation = string.Empty;


            if (File.Exists(currentFilePath) || (File.Exists(currentIntroPath) && type == "intro"))
            {
                if (type == "music")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicFolder");
                }
                else if(type == "intro")
                {
                    if(CurrentIntroPath == string.Empty)
                    {
                        return true;
                    }

                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicIntroFolder");

                    for (int i = 0; i < Directory.GetFiles(finalLocation).Count(); i++)
                    {
                        if (Path.GetFileName(Directory.GetFiles(finalLocation)[i]) == Path.GetFileName(CurrentIntroPath))
                        {
                            return false;
                        }
                    }

                    finalLocation = Path.Combine(finalLocation, Path.GetFileName(currentIntroPath));

                    File.Copy(currentIntroPath, finalLocation);
                    currentIntroPath = string.Empty;
                    return true;
                }
                else if (type == "sound")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\soundEffectsFolder");
                }
                else if (type == "icon")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\icons");
                }
                else if (type == "overlay")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\overlays");
                }
                else if (type == "map")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps");
                }
                else
                {
                    throw new Exception("unimplemented type");
                }


                for(int i = 0; i < Directory.GetFiles(finalLocation).Count(); i++)
                {
                    if (Path.GetFileName(Directory.GetFiles(finalLocation)[i]) == Path.GetFileName(CurrentFilePath))
                    {
                        return false;
                    }
                }
                
                finalLocation = Path.Combine(finalLocation, Path.GetFileName(currentFilePath));

                File.Copy(currentFilePath, finalLocation);
                currentFilePath = string.Empty;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Takes file data and stores it in respective data text file.
        /// </summary>
        /// <param name="type">type of resource.</param>
        /// <param name="data">data associated with that resource.</param>
        /// <exception cref="Exception">passed type which doesn't exists.</exception>
        public static void AddData(string type, string[] data)
        {
            if (type == "music")
            {
                currentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\Tracklist.txt");
            }
            else if (type == "sound")
            {
                currentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\SoundEffectList.txt");
            }
            else if (type == "icon")
            {
                currentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\IconList.txt");
            }
            else if (type == "overlay")
            {
                currentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\OverlayList.txt");
            }
            else if (type == "map")
            {
                currentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\MapList.txt");
            }
            else
            {
                throw new Exception("unimplemented type");
            }

            StreamWriter writer = File.AppendText(currentFilePath);

            string formattedData = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                formattedData = formattedData + data[i] + ",";
            }

            writer.WriteLine(formattedData);

            writer.Close();
        }
    }
}
