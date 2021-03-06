﻿using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="Button"/>.
    /// </summary>
    public class ZeusButton : Button
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusButton), new FrameworkPropertyMetadata(typeof(ZeusButton)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusButton), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusButton"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusButton"/> border styles.
        /// </summary>
        public static readonly DependencyProperty IsBorderVisibleProperty = DependencyProperty.Register("IsBorderVisible", typeof(bool), typeof(ZeusButton));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusButton"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        /// <summary>
        /// Gets or sets <see cref="ZeusButton"/> border style.
        /// </summary>
        public bool IsBorderVisible
        {
            get { return (bool)GetValue(IsBorderVisibleProperty); }
            set { SetValue(IsBorderVisibleProperty, value); }
        }

        #endregion
    }
}
