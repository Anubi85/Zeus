using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Zeus.Data;
using Zeus.Exceptions;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// Represent a repository mapped to a computer directory.
    /// </summary>
    internal class DirectoryRepository : RepositoryBase
    {
        #region Constants

        /// <summary>
        /// The tag name of the directory path data in the <see cref="DataStore"/> object that contains repository settings.
        /// </summary>
        private const string c_DirectoryPathTag = "Path";

        #endregion

        #region Fields

        /// <summary>
        /// The path of the directory to wich the repository is linked.
        /// </summary>
        private string m_directoryPath;

        #endregion

        #region RepositoryBase abstract methods

        /// <summary>
        /// Initialize the repository object.
        /// </summary>
        /// <param name="settings">The data needed to initialize the repository.</param>
        public override void Initialize(DataStore settings)
        {
            string repositoryPath = settings.TryGet<string>(c_DirectoryPathTag, null);
            if (string.IsNullOrEmpty(repositoryPath) || !Directory.Exists(repositoryPath))
            {
                throw new ZeusException(ErrorCodes.RepositoryPathNotExist, string.Format("Directory {0} does not exists", repositoryPath));
            }
            m_directoryPath = repositoryPath;
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
            m_Records = directoryInspector.Inspect(m_directoryPath);
            //destroy the inspector app domain
            AppDomain.Unload(inspectorDomain);
        }
        /// <summary>
        /// Compare the current repository object with the settings provided to check if it is the same.
        /// </summary>
        /// <param name="settings">the repository settings that has to be checked.</param>
        /// <returns>Returns true if the repository are the same, false otherwise.</returns>
        public override bool EqualsTo(DataStore settings)
        {
            return m_directoryPath == settings.TryGet<string>(c_DirectoryPathTag, null);
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
                foreach (string assemblyFileName in Directory.GetFiles(Path.GetFullPath(folder), "*.dll"))
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
