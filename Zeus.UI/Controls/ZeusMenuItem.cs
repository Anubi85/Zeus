using System.Windows;
using System.Windows.Controls;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="MenuItem"/>.
    /// </summary>
    public class ZeusMenuItem : MenuItem
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusMenuItem), new FrameworkPropertyMetadata(typeof(ZeusMenuItem)));
        }

        #endregion
    }
}
