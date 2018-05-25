using System.Windows;
using System.Windows.Controls.Primitives;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="StatusBar"/>.
    /// </summary>
    public class ZeusStatusBar : StatusBar
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusStatusBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusStatusBar), new FrameworkPropertyMetadata(typeof(ZeusStatusBar)));
        }

        #endregion
    }
}
