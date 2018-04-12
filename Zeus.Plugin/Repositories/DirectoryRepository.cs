using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zeus.Exceptions;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// Represent a repository mapped to a computer directory.
    /// </summary>
    public class DirectoryRepository : IRepository
    {
        #region Fields

        /// <summary>
        /// The path to the repository directory.
        /// </summary>
        private string m_RepositoryPath;
        /// <summary>
        /// The list of the possible plugins found in the repository.
        /// </summary>
        private List<RepositoryRecord> m_Records;

        #endregion

        #region IRepository interface

        /// <summary>
        /// Initialize the repository object.
        /// </summary>
        /// <param name="repositoryPath">The path of the repository.</param>
        public void Initialize(string repositoryPath)
        {
            if (string.IsNullOrEmpty(repositoryPath) || !Directory.Exists(repositoryPath))
            {
                throw new ZeusException(ErrorCodes.RepositoryPathNotExist, string.Format("Repository {0} does not exists", repositoryPath));
            }
            m_RepositoryPath = repositoryPath;
            m_Records = new List<RepositoryRecord>();
        }

        /// <summary>
        /// Inspects the repository and retrieve information about avaialble plugins.
        /// </summary>
        public void Inspect()
        {
            //create a new app domain for directory inspection
            AppDomain inspectorDomain = AppDomain.CreateDomain("InspectorAppDomain");
            //create an inspector object in the new app domain
            Inspector directoryInspector = inspectorDomain.CreateInstance<Inspector>();
            m_Records = directoryInspector.Inspect(m_RepositoryPath);
            //destroy the inspector app domain
            AppDomain.Unload(inspectorDomain);
        }

        /// <summary>
        /// Gets the repository path.
        /// </summary>
        public string Path { get { return m_RepositoryPath; } }

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
                    //loop over all the public types of the assembly
                    foreach (Type t in asm.GetExportedTypes())
                    {
                        //check if at least one type attrbute is an instance of ExportPluginAttribute or at least inherit from it
                        foreach (ExportPluginAttribute epa in t.GetCustomAttributes<ExportPluginAttribute>(true))
                        {
                            //add a new record to the results list
                            records.Add(new RepositoryRecord(asm.Location, t.FullName, epa, epa.GetType(), epa.PluginType));
                        }
                    }
                }
                return records;
            }

            #endregion
        }

        #endregion
    }
}
