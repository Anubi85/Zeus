using Zeus.Data;
using System.IO;

namespace Zeus.Config.Sources
{
    /// <summary>
    /// Interface that defines properties and method of a configuration source. This interface is meant for internal use only.
    /// </summary>
    public interface IConfigSource
    {
        #region Methods

        /// <summary>
        /// Open the config source.
        /// </summary>
        /// <returns>A <see cref="Stream"/> with the configuration data.</returns>
        Stream Open();
        /// <summary>
        /// Closes the configuration source.
        /// </summary>
        /// <returns>True if the operation succeed, false otherwise.</returns>
        bool Close();
        /// <summary>
        /// Initialize the configuration source.
        /// </summary>
        /// <param name="settings">The settings needed by the <see cref="IConfigSource"/> to initialize the source.</param>
        /// <param name="name">The name of the config source.</param>
        /// <returns>Returns true if the initialization succeeds, false otherwise.</returns>
        bool Initialize(DataStore settings, string name);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the config source.
        /// </summary>
        string Name { get; }

        #endregion
    }
}
