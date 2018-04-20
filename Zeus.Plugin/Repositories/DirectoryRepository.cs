using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Zeus.Exceptions;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// Represent a repository mapped to a computer directory.
    /// </summary>
    internal class DirectoryRepository : RepositoryBase
    {
        #region RepositoryBase abstract methods

        /// <summary>
        /// Initialize the repository object.
        /// </summary>
        /// <param name="repositoryPath">The path of the repository.</param>
        public override void Initialize(string repositoryPath)
        {
            if (string.IsNullOrEmpty(repositoryPath) || !Directory.Exists(repositoryPath))
            {
                throw new ZeusException(ErrorCodes.RepositoryPathNotExist, string.Format("Directory {0} does not exists", repositoryPath));
            }
            Path = repositoryPath;
            m_Records = new List<RepositoryRecord>();
        }

        /// <summary>
        /// Inspects the repository and retrieve information about avaialble plugins.
        /// </summary>
        public override void Inspect()
        {
            //create a new app domain for directory inspection
            AppDomain inspectorDomain = AppDomain.CreateDomain("InspectorAppDomain");
            //create an inspector object in the new app domain
            Inspector directoryInspector = inspectorDomain.CreateInstance<Inspector>();
            m_Records = directoryInspector.Inspect(Path);
            //destroy the inspector app domain
            AppDomain.Unload(inspectorDomain);
        }

        #endregion

        #region Helper class

        /// <summary>
        /// Helper class that allow to perform directory inspectio into another <see cref="AppDomain"/>.
        /// </summary>
        class Inspector : MarshalByRefObject
        {
            #region Methods

            /// <summary>
            /// Inspect the given directory and discover all the possible plugins.
            /// </summary>
            /// <param name="folder">The directory to be inspected.</param>
            /// <returns>The list of the discovered records.</returns>
            public List<RepositoryRecord> Inspect(string folder)
            {
                List<RepositoryRecord> records = new List<RepositoryRecord>();
                //gets all the files with .dll extension contained in the inspected directory
                foreach (string assemblyFileName in Directory.GetFiles(folder, "*.dll"))
                {
                    //load the dll
                    Assembly asm = Assembly.LoadFile(assemblyFileName);
                    records.AddRange(InspectAssembly(asm));
                }
                return records;
            }

            #endregion
        }

        #endregion
    }
}
