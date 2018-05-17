using System;

namespace Zeus.Extensions
{
    /// <summary>
    /// Provides extension methods for object that implements the <see cref="IComparable{T}"/> interface.
    /// </summary>
    public static class IComparableExtension
    {
        #region Methods

        /// <summary>
        /// Clamps the value between provided minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">The type of the object wich value has to be clamped.</typeparam>
        /// <param name="val">The value to be checked.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(this T val, T min, T max) where T: IComparable<T>
        {
            if (val.CompareTo(min) < 0)
            {
                return min;
            }
            else if (val.CompareTo(max) > 0)
            {
                return max;
            }
            else
            {
                return val;
            }
        }

        #endregion
    }
}
