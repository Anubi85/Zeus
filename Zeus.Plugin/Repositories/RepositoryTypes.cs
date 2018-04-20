using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus.Patterns;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// Provides the avaialble plugin repository types using a Type Safe Enum pattern.
    /// </summary>
    public sealed class RepositoryTypes : TypeSafeEnumBase<RepositoryTypes>
    {
        #region Construcotr

        /// <summary>
        /// Initialize a new <see cref="RepositoryTypes"/> value.
        /// </summary>
        /// <param name="repositoryClassType">The <see cref="Type"/> of the repository class associated with the cirrent value.</param>
        /// <param name="description">Te description of the current value.</param>
        private RepositoryTypes(Type repositoryClassType, string description)
        {
            RepositoryClassType = repositoryClassType;
            Description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type that implement the current repository value.
        /// </summary>
        public Type RepositoryClassType { get; private set; }
        /// <summary>
        /// Gets the description of the current repository value.
        /// </summary>
        public string Description { get; private set; }

        #endregion

        #region Operators

        /// <summary>
        /// Converts the <see cref="RepositoryTypes"/> value into the type that implement the current value.
        /// </summary>
        /// <param name="value">The <see cref="RepositoryTypes"/> value to be converted.</param>
        public static implicit operator Type (RepositoryTypes value)
        {
            return value.RepositoryClassType;
        }
        /// <summary>
        /// Converts the <see cref="RepositoryTypes"/> value into into its description.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator string (RepositoryTypes value)
        {
            return value.Description;
        }

        #endregion

        #region Values

        /// <summary>
        /// The <see cref="DirectoryRepository"/> value.
        /// </summary>
        public static readonly RepositoryTypes Directory = new RepositoryTypes(typeof(DirectoryRepository), "Inspect a local computer directory to discover the avaialble plugins.");
        /// <summary>
        /// The <see cref="AssemblyRepository"/> value.
        /// </summary>
        public static readonly RepositoryTypes Assembly = new RepositoryTypes(typeof(AssemblyRepository), "Inspect an assembly to discover the avaialble plugins.");

        #endregion
    }
}
