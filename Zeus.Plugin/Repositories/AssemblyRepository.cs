using System.Collections.Generic;
using System.Reflection;
using Zeus.Data;
using Zeus.Exceptions;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// Represent a repository mapped to an assembly.
    /// </summary>
    internal class AssemblyRepository : RepositoryBase
    {
        #region Constants

        /// <summary>
        /// The tag name of the assembly name data in the <see cref="DataStore"/> object that contains repository settings.
        /// </summary>
        private const string c_AssemblyNameTag = "AssemblyName";
        /// <summary>
        /// The tag name of the assembly data in the <see cref="DataStore"/> object that contains repository settings.
        /// </summary>
        private const string c_AssemblyTag = "Assembly";

        #endregion

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
        /// <param name="settings">The data needed to initialize the repository.</param>
        public override void Initialize(DataStore settings)
        {
            string assemblyName = settings.TryGet<string>(c_AssemblyNameTag, null);
            try
            {
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    m_Assembly = Assembly.Load(new AssemblyName(assemblyName));
                }
            }
            catch
            {
                throw new ZeusException(ErrorCodes.RepositoryPathNotExist, string.Format("Assembly {0} not found", assemblyName));
            }
            //if also an assembly has been provided it has priority over the assemly name
            m_Assembly = settings.TryGet<Assembly>(c_AssemblyTag, m_Assembly);
            if (m_Assembly == null)
            {
                throw new ZeusException(ErrorCodes.RepositoryPathNotExist, "No assembly provided for the repository");
            }
            m_Records = new List<RepositoryRecord>();
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

        /// <summary>
        /// Compare the current repository object with the settings provided to check if it is the same.
        /// </summary>
        /// <param name="settings">the repository settings that has to be checked.</param>
        /// <returns>Returns true if the repository are the same, false otherwise.</returns>
        public override bool EqualsTo(DataStore settings)
        {
            string assemblyName = settings.TryGet<string>(c_AssemblyNameTag, null);
            if (!string.IsNullOrEmpty(assemblyName))
            {
                return m_Assembly.GetName() == new AssemblyName(assemblyName);
            }
            //if an assembly has been provided
            Assembly asm = settings.TryGet<Assembly>(c_AssemblyTag, m_Assembly);
            return m_Assembly == asm;
        }

        #endregion
    }
}
