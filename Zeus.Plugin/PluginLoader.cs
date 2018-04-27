using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.Config;
using Zeus.Data;
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
            s_Repositories = new List<RepositoryBase>();
            //load settings from configuration
            PluginSettings settings = ConfigManager.LoadSection<PluginSettings>();
            if (settings != null)
            {
                foreach (KeyValuePair<string, DataStore> repoSettings in settings.RepositorySettings)
                {
                    AddRepository(repoSettings.Key, repoSettings.Value);
                }
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// The list of the configured repositories.
        /// </summary>
        private static List<RepositoryBase> s_Repositories;

        #endregion

        #region Methods

        /// <summary>
        /// Add a new repository to the <see cref="PluginLoader"/>.
        /// </summary>
        /// <param name="type">The type of the repository that has to be added.</param>
        /// <param name="settings">The data needed to initialize the repository.</param>
        public static void AddRepository(RepositoryTypes type, DataStore settings)
        {
            //check if a repository for the same path already exists
            if (!s_Repositories.Any(r => r.EqualsTo(settings)))
            {
                RepositoryBase newRepo = Activator.CreateInstance(type) as RepositoryBase;
                newRepo.Initialize(settings);
                newRepo.Inspect();
                s_Repositories.Add(newRepo);
            }
        }

        /// <summary>
        /// Re-perform an inspection of all the configured repositories and update plugins data.
        /// </summary>
        public static void UpdateRepositorties()
        {
            foreach (RepositoryBase rep in s_Repositories)
            {
                rep.Inspect();
            }
        }

        /// <summary>
        /// Gets all the avaialble plugin <see cref="PluginFactory{T}"/> for the requested plugin type.
        /// </summary>
        /// <typeparam name="T">The type of the requested plugin.</typeparam>
        /// <returns>A collection of all the <see cref="PluginFactory{T}"/> capable to instantiate a plugin of the requested type.</returns>
        public static IEnumerable<PluginFactory<T>> GetFactories<T>() where T : class
        {
            return s_Repositories.SelectMany(rep => rep.GetFactories<T>());
        }

        /// <summary>
        /// Gets all the avaialble plugin <see cref="PluginFactory{T, TMetaData}"/> for the requsted plugin type and that satisfy the filtering function.
        /// </summary>
        /// <typeparam name="T">The type of the requestd plugin.</typeparam>
        /// <typeparam name="TMetaData">The type of the plugin metadata.</typeparam>
        /// <param name="filter">The filtering funtion used to filter the available plugins.</param>
        /// <returns>A collection of all the <see cref="PluginFactory{T, TMetaData}"/> capable to instantiate a plgin of the request type and which metadata satisfy the filtering function.</returns>
        public static IEnumerable<PluginFactory<T>> GetFactories<T, TMetaData>(Func<TMetaData, bool> filter) where T : class where TMetaData : class
        {
            return s_Repositories.SelectMany(rep => rep.GetFactories<T, TMetaData>(filter));
        }

        #endregion
    }
}
