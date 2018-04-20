using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Patterns
{
    /// <summary>
    /// Provies base functionalities for a class that implement the Type Safe Enum pattern.
    /// </summary>
    /// <typeparam name="T">The type of the class that has to implement the Type Safe Enum pattern.</typeparam>
    public abstract class TypeSafeEnumBase<T> where T: TypeSafeEnumBase<T>
    {
        #region Fields

        /// <summary>
        /// The list of type safe enum instances.
        /// </summary>
        private static List<T> s_Instances;
        /// <summary>
        /// An instance counter. Provides defautl value for int conversion.
        /// </summary>
        private static int s_InstanceCounter;

        /// <summary>
        /// The ID of the current Type Safe Enunm value.
        /// </summary>
        protected int m_Id;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class static fields.
        /// </summary>
        static TypeSafeEnumBase()
        {
            s_Instances = new List<T>();
            s_InstanceCounter = 0;
            //force values creation
            foreach(FieldInfo fi in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                fi.GetValue(null);
            }
        }
        /// <summary>
        /// Initialize class fields.
        /// </summary>
        protected TypeSafeEnumBase()
        {
            m_Id = s_InstanceCounter++;
            s_Instances.Add(this as T);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the readonly list of all existing class intances.
        /// </summary>
        protected IReadOnlyList<T> Values { get { return s_Instances.AsReadOnly(); } }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the Type Safe Enum values.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> object that contains all the existing Type Safe Enum values.</returns>
        public static IEnumerable<T> GetValues()
        {
            return s_Instances.AsReadOnly();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Operators that convert the Type Safe Enum value into int.
        /// </summary>
        /// <param name="value">The Type Safe Enum value to be converted.</param>
        public static implicit operator int (TypeSafeEnumBase<T> value)
        {
            return value.m_Id;
        }

        #endregion
    }
}
