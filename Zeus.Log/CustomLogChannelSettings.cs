using System;
using System.Collections.Generic;
using System.Linq;

namespace Zeus.Log
{
    /// <summary>
    /// This class contains all the information for configure and initialize a log channel.
    /// Only basic types are allowed for settings.
    /// </summary>
    public class CustomLogChannelSettings :
        CustomLogChannelSettings.ICustomSettings<string>,
        CustomLogChannelSettings.ICustomSettings<int>,
        CustomLogChannelSettings.ICustomSettings<uint>,
        CustomLogChannelSettings.ICustomSettings<short>,
        CustomLogChannelSettings.ICustomSettings<ushort>,
        CustomLogChannelSettings.ICustomSettings<long>,
        CustomLogChannelSettings.ICustomSettings<ulong>,
        CustomLogChannelSettings.ICustomSettings<float>,
        CustomLogChannelSettings.ICustomSettings<double>,
        CustomLogChannelSettings.ICustomSettings<char>,
        CustomLogChannelSettings.ICustomSettings<bool>,
        CustomLogChannelSettings.ICustomSettings<byte>,
        CustomLogChannelSettings.ICustomSettings<sbyte>,
        CustomLogChannelSettings.ICustomSettings<decimal>
    {
        #region Helper interface

        /// <summary>
        /// Helper interface used to limit the possible settings types.
        /// </summary>
        /// <typeparam name="T">The type of the settings that can be get or set using this interface.</typeparam>
        internal interface ICustomSettings<T>
        {
            /// <summary>
            /// Gets a settings value.
            /// </summary>
            /// <param name="key">The settings key.</param>
            /// <returns>The settings value.</returns>
            T GetValue(string key);
            /// <summary>
            /// Sets a settings value.
            /// </summary>
            /// <param name="key">The settings key.</param>
            /// <param name="value">The settings value.</param>
            void SetValue(string key, T value);
        }

        #endregion

        #region Fields

        /// <summary>
        /// The dictionary that stores the custom settings.
        /// </summary>
        private Dictionary<string, object> m_CutomSettingsStore;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomLogChannelSettings"/> class.
        /// </summary>
        public CustomLogChannelSettings()
        {
            m_CutomSettingsStore = new Dictionary<string, object>();
        }

        #endregion

        #region ICustomSettings<string> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        string ICustomSettings<string>.GetValue(string key)
        {
            return m_CutomSettingsStore[key].ToString();
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<string>.SetValue(string key, string value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<int> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        int ICustomSettings<int>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return int.Parse((string)value);
            }
            else
            {
                return (int)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<int>.SetValue(string key, int value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<uint> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        uint ICustomSettings<uint>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return uint.Parse((string)value);
            }
            else
            {
                return (uint)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<uint>.SetValue(string key, uint value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<short> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        short ICustomSettings<short>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return short.Parse((string)value);
            }
            else
            {
                return (short)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<short>.SetValue(string key, short value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<ushort> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        ushort ICustomSettings<ushort>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return ushort.Parse((string)value);
            }
            else
            {
                return (ushort)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<ushort>.SetValue(string key, ushort value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<long> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        long ICustomSettings<long>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return long.Parse((string)value);
            }
            else
            {
                return (long)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<long>.SetValue(string key, long value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<ulong> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        ulong ICustomSettings<ulong>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return ulong.Parse((string)value);
            }
            else
            {
                return (ulong)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<ulong>.SetValue(string key, ulong value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<float> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        float ICustomSettings<float>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return float.Parse((string)value);
            }
            else
            {
                return (float)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<float>.SetValue(string key, float value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<double> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        double ICustomSettings<double>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return double.Parse((string)value);
            }
            else
            {
                return (double)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<double>.SetValue(string key, double value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<char> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        char ICustomSettings<char>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return char.Parse((string)value);
            }
            else
            {
                return (char)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<char>.SetValue(string key, char value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<bool> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        bool ICustomSettings<bool>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return bool.Parse((string)value);
            }
            else
            {
                return (bool)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<bool>.SetValue(string key, bool value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<byte> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        byte ICustomSettings<byte>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return byte.Parse((string)value);
            }
            else
            {
                return (byte)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<byte>.SetValue(string key, byte value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<sbyte> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        sbyte ICustomSettings<sbyte>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return sbyte.Parse((string)value);
            }
            else
            {
                return (sbyte)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<sbyte>.SetValue(string key, sbyte value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region ICustomSettings<decimal> interface

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The settings value.</returns>
        decimal ICustomSettings<decimal>.GetValue(string key)
        {
            object value = m_CutomSettingsStore[key];
            if (value is string)
            {
                return decimal.Parse((string)value);
            }
            else
            {
                return (decimal)value;
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        void ICustomSettings<decimal>.SetValue(string key, decimal value)
        {
            m_CutomSettingsStore[key] = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <typeparam name="T">The type of the settings value.</typeparam>
        /// <returns>The settings value.</returns>
        public T GetValue<T>(string key)
        {
            ICustomSettings<T> getter = this as ICustomSettings<T>;
            if (getter == null)
            {
                //if cast fails throw an exception
                throw new NotSupportedException(string.Format("Settings type {0} is not supported", typeof(T).Name));
            }
            else
            {
                return getter.GetValue(key);
            }
        }

        /// <summary>
        /// Sets a settings value.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <param name="value">The settings value.</param>
        /// <typeparam name="T">The type of the settings value.</typeparam>
        public void SetValue<T>(string key, T value)
        {
            ICustomSettings<T> setter = this as ICustomSettings<T>;
            if (setter == null)
            {
                //if cast fails throw an exception
                throw new NotSupportedException(string.Format("Settings type {0} is not supported", typeof(T).Name));
            }
            else
            {
                setter.SetValue(key, value);
            }
        }

        /// <summary>
        /// Gets the avaialble settings keys in alphabetical order.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{string}"/> of the avaialble settings keys.</returns>
        public IEnumerable<string> GetKeys()
        {
            return m_CutomSettingsStore.Keys.OrderBy(k => k);
        }

        #endregion
    }
}
