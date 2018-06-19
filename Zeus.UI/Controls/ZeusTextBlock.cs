using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus frmaework <see cref="TextBlock"/> control.
    /// </summary>
    public class ZeusTextBlock : TextBlock
    {
        #region Routed Events

        /// <summary>
        /// Occours when the texblock is clicked.
        /// </summary>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(ZeusTextBlock));

        #endregion

        #region Events

        /// <summary>
        /// Occours when the texblock is clicked.
        /// </summary>
        public event EventHandler<RoutedEventArgs> Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        #endregion

        #region Fields

        /// <summary>
        /// A flag that indicates if the click event must be rised or not.
        /// </summary>
        private bool m_RaiseClickEvent;

        #endregion

        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusTextBlock), new FrameworkPropertyMetadata(typeof(ZeusTextBlock)));
        }
        /// <summary>
        /// Create a new instance of <see cref="ZeusTextBlock"/> and initialize its internal fields.
        /// </summary>
        public ZeusTextBlock()
        {
            Mouse.AddPreviewMouseUpOutsideCapturedElementHandler(this, ResetEventTriggerFlag);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle the mouse down event.
        /// </summary>
        /// <param name="e">Information about the occurred event.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (CaptureMouse())
            {
                m_RaiseClickEvent = e.ChangedButton == MouseButton.Left;
            }
        }
        /// <summary>
        /// Handle the mouse up event.
        /// </summary>
        /// <param name="e">Information about the occurred event.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (m_RaiseClickEvent)
            {
                m_RaiseClickEvent = false;
                RaiseEvent(new RoutedEventArgs(ClickEvent, this));
            }
            //clear the mouse capture
            Mouse.Capture(null);
        }
        /// <summary>
        /// Handle the mouse up event occurred outside from control limits.
        /// </summary>
        /// <param name="sender">The object that raise the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void ResetEventTriggerFlag(object sender, MouseButtonEventArgs e)
        {
            m_RaiseClickEvent = !(e.ChangedButton == MouseButton.Left);
            //clear the mouse capture
            Mouse.Capture(null);
        }

        #endregion
    }
}
