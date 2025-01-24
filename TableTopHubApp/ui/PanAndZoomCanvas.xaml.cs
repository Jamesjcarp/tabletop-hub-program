// <copyright file="PanAndZoomCanvas.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    // This class was snatched from a project I found online.
    // This guy is a hero
    // https://github.com/SEilers/WpfPanAndZoom

    /// <summary>
    /// Pan and zoom class creates a canvas that allows the user to add objects to it and navigate them by zooming and panning.
    /// </summary>
    public partial class PanAndZoomCanvas : Canvas
    {
        private readonly MatrixTransform transform = new MatrixTransform();
        private Point initialMousePosition;

        private Color lineColor = Color.FromArgb(0xFF, 0x66, 0x66, 0x66);
        private Color backgroundColor = Color.FromArgb(0xFF, 0x33, 0x33, 0x33);
        private List<Line> gridLines = new List<Line>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanAndZoomCanvas"/> class.
        /// </summary>
        public PanAndZoomCanvas()
        {
            this.InitializeComponent();

            this.MouseDown += this.PanAndZoomCanvasMouseDown;
            this.MouseMove += this.PanAndZoomCanvasMouseMove;
            this.MouseWheel += this.PanAndZoomCanvasMouseWheel;

            this.BackgroundColor = this.backgroundColor;

            // draw lines
            for (int x = -4000; x <= 4000; x += 100)
            {
                Line verticalLine = new Line
                {
                    Stroke = new SolidColorBrush(this.lineColor),
                    X1 = x,
                    Y1 = -4000,
                    X2 = x,
                    Y2 = 4000,
                };

                if (x % 1000 == 0)
                {
                    verticalLine.StrokeThickness = 6;
                }
                else
                {
                    verticalLine.StrokeThickness = 2;
                }

                this.Children.Add(verticalLine);
                this.gridLines.Add(verticalLine);
            }

            for (int y = -4000; y <= 4000; y += 100)
            {
                Line horizontalLine = new Line
                {
                    Stroke = new SolidColorBrush(this.lineColor),
                    X1 = -4000,
                    Y1 = y,
                    X2 = 4000,
                    Y2 = y,
                };

                if (y % 1000 == 0)
                {
                    horizontalLine.StrokeThickness = 6;
                }
                else
                {
                    horizontalLine.StrokeThickness = 2;
                }

                this.Children.Add(horizontalLine);
                this.gridLines.Add(horizontalLine);
            }
        }

        /// <summary>
        /// Gets or sets the amount that the canvas zooms on each scroll.
        /// </summary>
        public float Zoomfactor { get; set; } = 1.1f;

        /// <summary>
        /// Gets or sets the color of the background grid lines.
        /// </summary>
        public Color LineColor
        {
            get
            {
                return this.lineColor;
            }

            set
            {
                this.lineColor = value;
                foreach (Line line in this.gridLines)
                {
                    line.Stroke = new SolidColorBrush(this.lineColor);
                }
            }
        }

        /// <summary>
        /// Gets or sets the background color of the grid.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }

            set
            {
                this.backgroundColor = value;
                this.Background = new SolidColorBrush(this.backgroundColor);
            }
        }

        /// <summary>
        /// Sets the opacity of the grid lines.
        /// </summary>
        /// <param name="value">the opacity value.</param>
        public void SetGridVisibility(Visibility value)
        {
            foreach (Line line in this.gridLines)
            {
                line.Visibility = value;
            }
        }

        private void PanAndZoomCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                this.initialMousePosition = this.transform.Inverse.Transform(e.GetPosition(this));
            }
        }

        private void PanAndZoomCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Point mousePosition = this.transform.Inverse.Transform(e.GetPosition(this));
                Vector delta = Point.Subtract(mousePosition, this.initialMousePosition);
                var translate = new TranslateTransform(delta.X, delta.Y);
                this.transform.Matrix = translate.Value * this.transform.Matrix;

                foreach (UIElement child in this.Children)
                {
                    child.RenderTransform = this.transform;
                }
            }
        }

        private void PanAndZoomCanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            float scaleFactor = this.Zoomfactor;
            if (e.Delta < 0)
            {
                scaleFactor = 1f / scaleFactor;
            }

            Point mousePostion = e.GetPosition(this);

            Matrix scaleMatrix = this.transform.Matrix;
            scaleMatrix.ScaleAt(scaleFactor, scaleFactor, mousePostion.X, mousePostion.Y);
            this.transform.Matrix = scaleMatrix;

            foreach (UIElement child in this.Children)
            {
                double x = Canvas.GetLeft(child);
                double y = Canvas.GetTop(child);

                double sx = x * scaleFactor;
                double sy = y * scaleFactor;

                Canvas.SetLeft(child, sx);
                Canvas.SetTop(child, sy);

                child.RenderTransform = this.transform;
            }
        }
    }
}
