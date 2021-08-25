﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SaintSender.DesktopUI.UserControls
{
    public class IconButton : Button
    {
        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }



        #region PathData
        public static readonly DependencyProperty PathDataProperty =
            DependencyProperty.Register(nameof(PathData), typeof(Geometry), typeof(IconButton), new PropertyMetadata(Geometry.Empty));

        public Geometry PathData
        {
            get { return (Geometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }
        #endregion



        #region TextProperty
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconButton),
                new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion



        #region Orientation
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(IconButton),
                new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion



        #region TextVisibility
        public static readonly DependencyProperty TextVisibilityProperty =
            DependencyProperty.Register(nameof(TextVisibility), typeof(Visibility), typeof(IconButton),
                new PropertyMetadata(default(Visibility)));

        public Visibility TextVisibility
        {
            get { return (Visibility)GetValue(TextVisibilityProperty); }
            set { SetValue(TextVisibilityProperty, value); }
        }
        #endregion



        #region IconVisibility
        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register(nameof(IconVisibility), typeof(Visibility), typeof(IconButton),
                new PropertyMetadata(default(Visibility)));

        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }
        #endregion


        #region HoverBackground
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register(nameof(HoverBackground), typeof(Color), typeof(IconButton), new PropertyMetadata(default(Color)));
        public Color HoverBackground
        {
            get { return (Color)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }
        #endregion


        #region HoverForeground
        public static readonly DependencyProperty HoverForegroundProperty =
            DependencyProperty.Register(nameof(HoverForeground), typeof(Color), typeof(IconButton), new PropertyMetadata(default(Color)));
        public Color HoverForeground
        {
            get { return (Color)GetValue(HoverForegroundProperty); }
            set { SetValue(HoverForegroundProperty, value); }
        }
        #endregion
    }
}