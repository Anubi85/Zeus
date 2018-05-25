using System.Windows;
using System.Windows.Controls;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="TextBox"/>.
    /// </summary>
    public class ZeusTextBox : TextBox
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusTextBox), new FrameworkPropertyMetadata(typeof(ZeusTextBox)));
        }

        #endregion
    }
}
