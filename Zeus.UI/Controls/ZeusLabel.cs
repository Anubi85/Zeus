using System.Windows;
using System.Windows.Controls;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="Label"/>.
    /// </summary>
    public class ZeusLabel : Label
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusLabel), new FrameworkPropertyMetadata(typeof(ZeusLabel)));
        }

        #endregion
    }
}
