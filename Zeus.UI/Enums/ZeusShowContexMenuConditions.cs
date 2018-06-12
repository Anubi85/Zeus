using System;

namespace Zeus.UI.Enums
{
    /// <summary>
    /// Context menu display mode.
    /// </summary>
    [Flags]
    public enum ZeusShowContexMenuConditions
    {
        /// <summary>
        /// Context menu will never automatically displayed.
        /// </summary>
        Never = 0x0,
        /// <summary>
        /// Context menu will automatically displayed on mouse right click.
        /// </summary>
        OnRightClick = 0x1,
        /// <summary>
        /// Context menu will automatically displayed on mouse left click.
        /// </summary>
        OnLeftClick = 0x2,
        /// <summary>
        /// Context menu will automatically displayed on mouse middle click.
        /// </summary>
        OnMiddleClick = 0x4,
    }
}
