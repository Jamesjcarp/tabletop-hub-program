// <copyright file="OverlayScreen.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// The overlay screen for the hub. allows for image and video functionality. TODO.
    /// </summary>
    public partial class OverlayScreen : Window
    {
        private static bool editing = true;

        private static UIElement? screenElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayScreen"/> class.
        /// </summary>
        public OverlayScreen()
        {
            this.InitializeComponent();
            this.Topmost = true;

            this.ChangeWindowState();
        }

        /// <summary>
        /// Toggle class to switch between invisible and movable.
        /// </summary>
        public void ChangeWindowState()
        {
            this.Dispatcher.Invoke(() =>
            {
                // Lock in overlay window and hide it.
                if (editing == true)
                {
                    SolidColorBrush brush = new SolidColorBrush { Opacity = 0, Color = Colors.White };

                    this.grid.Children.Clear();

                    this.Background = brush;

                    this.WindowState = WindowState.Maximized;

                    if(screenElement != null)
                    {
                        this.grid.Children.Add(screenElement);
                    }

                    editing = false;
                }

                // Unlock overlay and allow for movement.
                else
                {
                    SolidColorBrush brush = new SolidColorBrush { Opacity = 1, Color = Colors.Blue };

                    this.Background = brush;

                    this.WindowState = WindowState.Normal;

                    this.grid.Children.Clear();

                    TextBlock text = new TextBlock { Text = "Move Me!", FontSize = 60, TextAlignment = TextAlignment.Center };

                    this.grid.Children.Add(text);

                    editing = true;
                }
            });
        }

        /// <summary>
        /// Begins showing the given overlay element.
        /// </summary>
        /// <param name="elementName">Name of the element to display.</param>
        public void EnableOverlayElement(string elementName)
        {

        }

        /// <summary>
        /// Cuts off the currently playing element.
        /// </summary>
        public void DisableOverlayElement()
        {
            this.grid.Children.Clear();
            screenElement = null;
        }

        private void OverlayMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && editing == true)
            {
                this.DragMove();
            }
        }
    }
}
