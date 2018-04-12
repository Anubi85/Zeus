using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus.Plugin.Repositories;

namespace Zeus.Plugin
{
    /// <summary>
    /// Loads plugin from configured repositories.
    /// </summary>
    public static class PluginLoader
    {
        #region Constructor

        /// <summary>
        /// Initialize class fields.
        /// </summary>
        static PluginLoader()
        {
            s_Repositories = new List<IRepository>();
        }

        #endregion

        #region Fields

        /// <summary>
        /// The list of the configured repositories.
        /// </summary>
        private static List<IRepository> s_Repositories;

        #endregion

        #region Methods

        /// <summary>
        /// Add a new repository to the <see cref="PluginLoader"/>.
        /// </summary>
        /// <param name="repositoryPath">The path of the repository that has to be added.</param>
        public static void AddRepository<T>(string repositoryPath) where T: IRepository, new ()
        {
            //check if a repository for the same path already exists
            if (!s_Repositories.Any(r => r.Path == repositoryPath))
            {
                IRepository newRepo = new T();
                newRepo.Initialize(repositoryPath);
                newRepo.Inspect();
                s_Repositories.Add(newRepo);
            }
        }

        /// <summary>
        /// Remove a repository from the <see cref="PluginLoader"/>.
        /// </summary>
        /// <param name="repositoryPath">The path of the repository that has to be removed.</param>
        public static void RemoveRepository(string repositoryPath)
        {
            IRepository toRemove = s_Repositories.FirstOrDefault(r => r.Path == repositoryPath);
            if (toRemove != null)
            {
                s_Repositories.Remove(toRemove);
            }
        }

        /// <summary>
        /// Re-perform an inspection of all the configured repositories and update plugins data.
        /// </summary>
        public static void UpdateRepositorties()
        {
            foreach(IRepository rep in s_Repositories)
            {
                rep.Inspect();
            }
        }

        #endregion
    }
}
