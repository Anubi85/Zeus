using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// defines methods and properties that a repository inspector must implements.
    /// </summary>
    public interface IRepository
    {
        #region Methods

        /// <summary>
        /// Initialize the repository object.
        /// </summary>
        /// <param name="repositoryPath">The path of the repository.</param>
        void Initialize(string repositoryPath);

        /// <summary>
        /// Inspects the repository and retrieve information about avaialble plugins.
        /// </summary>
        void Inspect();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the repository path.
        /// </summary>
        string Path { get; }

        #endregion
    }
}
