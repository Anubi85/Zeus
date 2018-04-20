using System;

namespace Zeus.Patterns
{
    /// <summary>
    /// Provides a base implmenetation for the singleton pattern.
    /// </summary>
    /// <typeparam name="T">The singleton type.</typeparam>
    public static class Singleton<T> where T: class
    {
        #region Singleton pattern

        #region Fields

        /// <summary>
        /// The one and only instance of the singleton class.
        /// </summary>
        private static readonly Lazy<T> s_Instance;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the one and only singleton instance.
        /// </summary>
        public static T Instance { get { return s_Instance.Value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class fields.
        /// </summary>
        static Singleton()
        {
            s_Instance = new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);
        }

        #endregion

        #endregion
    }
}
