using System.Windows;
using System.Windows.Controls;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="Menu"/>.
    /// </summary>
    public class ZeusMenu : Menu
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusMenu), new FrameworkPropertyMetadata(typeof(ZeusMenu)));
        }

        #endregion
    }
}
