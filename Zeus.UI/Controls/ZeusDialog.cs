using System.Windows;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled dialog <see cref="Window"/>.
    /// </summary>
    public class ZeusDialog : ZeusWindowBase
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDialog), new FrameworkPropertyMetadata(typeof(ZeusDialog)));
        }

        #endregion
    }
}
