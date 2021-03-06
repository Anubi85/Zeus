﻿using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DataGrid"/>.
    /// </summary>
    public class ZeusDataGrid : DataGrid
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDataGrid), new FrameworkPropertyMetadata(typeof(ZeusDataGrid)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusDataGrid), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>A <see cref="ZeusDataGridRow"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZeusDataGridRow();
        }
        /// <summary>
        /// Sets the <see cref="ZeusDataGrid"/> in autoscroll mode.
        /// </summary>
        private void SetAutoScroll()
        {
            (Items as INotifyCollectionChanged).CollectionChanged += AutoScrollHandler;
            ScrollToEnd();
        }
        /// <summary>
        /// Unsets the <see cref="ZeusDataGrid"/> from autoscroll mode.
        /// </summary>
        private void UnsetAutoScroll()
        {
            (Items as INotifyCollectionChanged).CollectionChanged -= AutoScrollHandler;
        }
        /// <summary>
        /// Srolls the <see cref="ZeusDataGrid"/> to its last row.
        /// </summary>
        private void ScrollToEnd()
        {
            if (Items.Count != 0)
            {
                ScrollIntoView(Items[Items.Count - 1]);
            }
        }
        /// <summary>
        /// Handle the datagrid items collection changing events.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Information about the event.</param>
        private void AutoScrollHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            ScrollToEnd();
        }
        /// <summary>
        /// Callback invoked when the <see cref="AutoScrollProperty"/> value changes.
        /// </summary>
        /// <param name="d">Teh <see cref="DependencyObject"/> which property changed value.</param>
        /// <param name="e">Information abou the event.</param>
        private static void AutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                (d as ZeusDataGrid)?.SetAutoScroll();
            }
            else
            {
                (d as ZeusDataGrid)?.UnsetAutoScroll();
            }
        }
        /// <summary>
        /// Handle the datagrid items selection changed events.
        /// </summary>
        /// <param name="e">Information about the event.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (!IsSelectable)
            {
                UnselectAllCells();
            }
            else
            {
                base.OnSelectionChanged(e);
            }
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGrid"/> autoscroll.
        /// </summary>
        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.Register("AutoScroll", typeof(bool), typeof(ZeusDataGrid), new PropertyMetadata(false, AutoScrollChanged));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGrid"/> selection behaviour.
        /// </summary>
        public static readonly DependencyProperty IsSelectableProperty = DependencyProperty.Register("IsSelectable", typeof(bool), typeof(ZeusDataGrid), new PropertyMetadata(true));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGrid"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusDataGrid"/> autoscroll flag.
        /// </summary>
        public bool AutoScroll
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }
        /// <summary>
        /// Gets or sets <see cref="ZeusDataGrid"/> is selectable flag.
        /// </summary>
        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }
        /// <summary>
        /// Gets or sets <see cref="ZeusDataGrid"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
