// <copyright file="HexGrid.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>
namespace TableTopHubApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// The class creating the files for the Hexagonal Grid.
    /// </summary>
    internal class HexGrid : Canvas
    {
        private double hexRadius;
        private int rows;
        private int cols;

        /// <summary>
        /// Initializes a new instance of the <see cref="HexGrid"/> class.
        /// </summary>
        public HexGrid()
        {
            this.rows = -1;
            this.cols = -1;
            this.hexRadius = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexGrid"/> class.
        /// </summary>
        /// <param name="canvas">The base canvas.</param>
        /// <param name="row">The row count from the map file.</param>
        /// <param name="col">The Coloumn count from the map file.</param>
        /// <param name="background">The Background Brush for the Map.</param>
        public HexGrid(int row, int col, ImageBrush background)
        {
            this.rows = row;
            this.cols = col;
            this.hexRadius = 50;

            this.CreateBackground(background);

            this.DrawGrid();
        }

        /// <summary>
        /// Function to draw the grid to the canvas.
        /// </summary>
        public void DrawGrid()
        {
            double hexHeight = this.hexRadius * 3/2;
            double hexWidth = this.hexRadius * Math.Sqrt(3);

            for (int row = 0; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols; col++)
                {
                    double x = row * hexWidth;
                    double y = col * hexHeight;

                    if (col % 2 == 1)
                    {
                        x += hexWidth / 2;
                    }

                    Polygon hexagon = this.CreateHex();

                    Canvas.SetLeft(hexagon, x);
                    Canvas.SetTop(hexagon, y);
                    this.Children.Add(hexagon);
                }
            }
        }

        private void CreateBackground(ImageBrush brush)
        {
            Rectangle backgroundRect = new Rectangle
            {
                Width = this.hexRadius * this.rows,
                Height = this.hexRadius * this.cols,
                Fill = brush,
            };
            this.Children.Add(backgroundRect);
        }

        private Polygon CreateHex()
        {
            Polygon hex = new Polygon();
            hex.Points = new PointCollection
            {
                new Point(0, this.hexRadius),
                new Point(this.hexRadius * (Math.Sqrt(3)/2), this.hexRadius / 2),
                new Point(this.hexRadius * (Math.Sqrt(3)/2), -(this.hexRadius / 2)),
                new Point(0, -this.hexRadius),
                new Point(-this.hexRadius * (Math.Sqrt(3)/2), -(this.hexRadius / 2)),
                new Point(-this.hexRadius * (Math.Sqrt(3)/2), this.hexRadius / 2),
            };
            hex.Fill = Brushes.Transparent;
            hex.Stroke = Brushes.Black;
            hex.StrokeThickness = 2;
            return hex;
        }
    }
}
