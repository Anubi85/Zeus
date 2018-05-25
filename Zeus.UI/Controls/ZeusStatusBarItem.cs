using System.Windows;
using System.Windows.Controls.Primitives;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="StatusBarItem"/>.
    /// </summary>
    public class ZeusStatusBarItem : StatusBarItem
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusStatusBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusStatusBarItem), new FrameworkPropertyMetadata(typeof(ZeusStatusBarItem)));
        }

        #endregion
    }
}
