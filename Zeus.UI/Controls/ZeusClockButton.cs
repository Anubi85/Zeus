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

        #region Depndenct properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusClockButton"/> is selected flag.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(ZeusClockButton), new PropertyMetadata(false));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a flag that indicates if the button is currently selected.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        #endregion
    }
}
