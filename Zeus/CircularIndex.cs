using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus
{
    /// <summary>
    /// Implements an index that automatically wrap around a maximum capacity.
    /// </summary>
    public sealed class CircularIndex
    {
        #region Fields

        /// <summary>
        /// The actual index value.
        /// </summary>
        private int m_Idx;
        /// <summary>
        /// The maximum size value.
        /// </summary>
        private int m_Capacity;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the index to zero and set its maximum capacity.
        /// </summary>
        /// <param name="capacity">The maximum index capacity.</param>
        public CircularIndex(int capacity) : this(capacity, 0)
        {
        }
        /// <summary>
        /// Initialize the index to the initial value and set its maximum capacity.
        /// </summary>
        /// <param name="capacity">The maximum index capacity.</param>
        /// <param name="initialValue">The index initial value.</param>
        public CircularIndex(int capacity, int initialValue)
        {
            m_Capacity = capacity;
            m_Idx = Mod(initialValue, capacity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="CircularIndex"/> capacity.
        /// </summary>
        public int Capacity { get { return m_Capacity; } }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the modulus of the given number using <paramref name="mod"/> as base.
        /// </summary>
        /// <param name="val">The number to wich the modulus must be computed.</param>
        /// <param name="mod">The modulus value.</param>
        /// <returns>The modulus of the given number.</returns>
        private static int Mod(int val, int mod)
        {
            int tmp = val % mod;
            return tmp < 0 ? tmp + mod: tmp;
        }

        #endregion

        #region Operators

        public static implicit operator int(CircularIndex me)
        {
            return me.m_Idx;
        }

        public static CircularIndex operator +(CircularIndex me, int incr)
        {
            CircularIndex res = new CircularIndex(me.m_Capacity);
            res.m_Idx = Mod(me.m_Idx + incr, me.m_Capacity);
            return res;
        }

        public static CircularIndex operator ++(CircularIndex me)
        {
            return me + 1;
        }

        public static CircularIndex operator -(CircularIndex me, int incr)
        {
            return me + (-incr);
        }

        public static CircularIndex operator --(CircularIndex me)
        {
            return me - 1;
        }

        #endregion
    }
}
