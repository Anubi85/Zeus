using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zeus.Exceptions;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// Represent a repository mapped to an assembly.
    /// </summary>
    internal class AssemblyRepository : RepositoryBase
    {
        #region Fields

        /// <summary>
        /// The Assembly associated with the repository.
        /// </summary>
        private Assembly m_Assembly;

        #endregion

        #region RepositoryBase abstract methods

        /// <summary>
        /// Initialize the repository object.
        /// </summary>
        /// <param name="repositoryPath">The path of the repository.</param>
        public override void Initialize(string repositoryPath)
        {
            try
            {
                m_Assembly = Assembly.Load(new AssemblyName(repositoryPath));
                Path = repositoryPath;
                m_Records = new List<RepositoryRecord>();
            }
            catch
            {
                throw new ZeusException(ErrorCodes.RepositoryPathNotExist, string.Format("Assembly {0} not found", repositoryPath));
            }
        }

        /// <summary>
        /// Inspects the repository and retrieve information about avaialble plugins.
        /// </summary>
        public override void Inspect()
        {
            //clear the old records
            m_Records.Clear();
            //inspect the assembly
            m_Records.AddRange(InspectAssembly(m_Assembly));
        }

        #endregion
    }
}
