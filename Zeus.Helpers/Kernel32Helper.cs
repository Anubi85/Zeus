using System;
using System.Runtime.InteropServices;

namespace Zeus.Helpers
{
    /// <summary>
    /// Helper class for native methods from Kernel32 dll.
    /// </summary>
    public static class Kernel32Helper
    {
        #region Methods

        /// <summary>
        /// Gets the handle of the module identified by the given name.
        /// </summary>
        /// <param name="lpModuleName">The name of the module wich handle has to be retieved.</param>
        /// <returns>The handle of the module if succeed, <see cref="IntPtr.Zero"/> if fails.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion
    }
}
