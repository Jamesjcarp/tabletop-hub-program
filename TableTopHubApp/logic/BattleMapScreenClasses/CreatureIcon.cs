﻿// <copyright file="CreatureIcon.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.IO;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using WpfAnimatedGif;

    /// <summary>
    /// Creature Icon class contains the methods required to create a circular creature for the board. 
    /// </summary>
    internal class CreatureIcon
    {
        private ImageBrush iconImage = new ImageBrush();
        private VisualBrush iconGif = new VisualBrush();
        private Ellipse fullIcon = new Ellipse();
        private string iconName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatureIcon"/> class.
        /// </summary>
        public CreatureIcon()
        {
            this.fullIcon.Stroke = Brushes.Black;
            this.fullIcon.StrokeThickness = 3;
        }

        /// <summary>
        /// Take in the name of an icon and open the correct file and set it to be in the ellipse.
        /// </summary>
        /// <param name="iconName">name of the image.</param>
        public void ChangeIcon(string iconName)
        {
            this.iconName = iconName;

            BitmapImage uri = new BitmapImage();
            uri.BeginInit();

            uri.UriSource = new Uri(MapManager.GetIconPath(iconName));

            uri.EndInit();

            if (MapManager.IsAnimated(iconName))
            {
                Image temp = new Image();

                ImageBehavior.SetAnimatedSource(temp, uri);

                this.iconGif.Visual = temp;

                this.fullIcon.Fill = this.iconGif;
            }
            else
            {
                this.iconImage.ImageSource = uri;

                this.fullIcon.Fill = this.iconImage;
            }
        }

        /// <summary>
        /// Gets the ellipse to display on the map.
        /// </summary>
        /// <returns>an Ellipse object.</returns>
        public Ellipse GetIcon()
        {
            return this.fullIcon;
        }

        /// <summary>
        /// Gets the width in cells that the icon takes up.
        /// </summary>
        /// <returns>Integer width of icon.</returns>
        public int GetIconWidth()
        {
            return MapManager.GetIconWidth(this.iconName);
        }

        /// <summary>
        /// Gets the height in cells that the icon takes up.
        /// </summary>
        /// <returns>Integer height of icon.</returns>
        public int GetIconHeight()
        {
            return MapManager.GetIconHeight(this.iconName);
        }
    }
}
