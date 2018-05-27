using System.Windows;
using System.Windows.Controls;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="CheckBox"/>.
    /// </summary>
    public class ZeusCheckBox : CheckBox
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusCheckBox), new FrameworkPropertyMetadata(typeof(ZeusCheckBox)));
        }

        #endregion
    }
}
