using System;

namespace Zeus.Plugin
{
    /// <summary>
    /// Extension methods for <see cref="AppDomain"/> class.
    /// </summary>
    internal static class AppDomainExtension
    {
        #region Methods

        /// <summary>
        /// Create a new instance of the specified type in the caller <see cref="AppDomain"/> and unwrap it.
        /// </summary>
        /// <typeparam name="T">The type wich instance shall be created. Must inherit from <see cref="MarshalByRefObject"/>.</typeparam>
        /// <param name="domain">The <see cref="AppDomain"/> where the new instance will be created.</param>
        /// <returns>The unwrapped new instance.</returns>
        public static T CreateInstance<T>(this AppDomain domain) where T: MarshalByRefObject
        {
            Type instanceType = typeof(T);
            return (T)domain.CreateInstanceAndUnwrap(instanceType.Assembly.FullName, instanceType.FullName);
        }

        #endregion
    }
}
