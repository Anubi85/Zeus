using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="TabControl"/>.
    /// </summary>
    public class ZeusTabControl : TabControl
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusTabControl), new FrameworkPropertyMetadata(typeof(ZeusTabControl)));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle tab closing.
        /// </summary>
        public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register("CloseTabCommand", typeof(ICommand), typeof(ZeusTabControl));

        #endregion

        #region Properties

        /// <summary>
        /// Get/sets the command that executes when a <see cref="ZeusTabItem"/> close button is clicked.
        /// </summary>
        public ICommand CloseTabCommand
        {
            get { return (ICommand)GetValue(CloseTabCommandProperty); }
            set { SetValue(CloseTabCommandProperty, value); }
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Provides the signature for the <see cref="TabItemClosing"/> event handlers.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">The object that contains detail about the event.</param>
        public delegate void TabItemClosingEventHandler(object sender, TabItemClosingEventArgs e);

        #endregion

        #region Events

        /// <summary>
        /// An event that is raised when a <see cref="ZeusTabItem"/> is closed.
        /// </summary>
        public event TabItemClosingEventHandler TabItemClosing;

        #endregion

        #region Event Args

        /// <summary>
        /// Event args that is created when a <see cref="ZeusTabItem"/> is closed.
        /// </summary>
        public class TabItemClosingEventArgs : CancelEventArgs
        {
            #region Constructor

            /// <summary>
            /// Initialize a new instance of <see cref="TabItemClosingEventArgs"/>.
            /// </summary>
            /// <param name="item">The <see cref="ZeusTabItem"/> that trigger the event.</param>
            internal TabItemClosingEventArgs(ZeusTabItem item)
            {
                ClosingTabItem = item;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the <see cref="ZeusTabItem"/> that will be closed.
            /// </summary>
            public ZeusTabItem ClosingTabItem { get; private set; }

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>A <see cref="ZeusTabItem"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZeusTabItem();
        }
        /// <summary>
        /// Raises the <see cref="TabItemClosing"/> event.
        /// </summary>
        /// <param name="closingItem">The <see cref="ZeusTabItem"/> that raise the event.</param>
        /// <returns>true if the event has been rised, false otherwise.</returns>
        internal bool RaiseTabItemClosingEvent(ZeusTabItem closingItem)
        {
            if (TabItemClosing != null)
            {
                foreach (TabItemClosingEventHandler subHandler in TabItemClosing.GetInvocationList().OfType<TabItemClosingEventHandler>())
                {
                    TabItemClosingEventArgs args = new TabItemClosingEventArgs(closingItem);
                    subHandler.Invoke(this, args);
                    if (args.Cancel)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Close the given <see cref="ZeusTabItem"/>.
        /// </summary>
        /// <param name="tabItem">The <see cref="ZeusTabItem"/> that has to be closed.</param>
        internal void CloseThisTabItem(ZeusTabItem tabItem)
        {
            if (tabItem == null)
            {
                throw new ArgumentNullException(nameof(tabItem));
            }

            if (CloseTabCommand != null)
            {
                object closeTabCommandParameter =  tabItem;
                if (CloseTabCommand.CanExecute(closeTabCommandParameter))
                {
                    CloseTabCommand.Execute(closeTabCommandParameter);
                }
            }
        }

        #endregion
    }
}
