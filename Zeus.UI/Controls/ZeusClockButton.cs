using System.Windows;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Button used in the <see cref="ZeusClock"/> control.
    /// </summary>
    public class ZeusClockButton : ZeusButton
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusClockButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusClockButton), new FrameworkPropertyMetadata(typeof(ZeusClockButton)));
        }

        #endregion
    }
}
