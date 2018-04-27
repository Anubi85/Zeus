using Zeus.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Zeus.Data
{
    /// <summary>
    /// Data table where records can be accessed by index or by tag.
    /// </summary>
    public sealed class DataStore
    {
        #region Record class

        /// <summary>
        /// Stores information about a <see cref="Data"/> record.
        /// </summary>
        private sealed class Record
        {
            #region Properties

            /// <summary>
            /// Gets the <see cref="Record"/> tag.
            /// </summary>
            public string Tag { get; private set; }
            /// <summary>
            /// Gets the <see cref="Record"/> index.
            /// </summary>
            public int Idx { get; private set; }
            /// <summary>
            /// Gets the <see cref="Record"/> value.
            /// </summary>
            public object Value { get; set; }
            /// <summary>
            /// Gets the <see cref="Type"/> of the <see cref="Record"/> value.
            /// </summary>
            public Type ValueType { get; private set; }

            #endregion

            #region Construcotr

            /// <summary>
            /// Initialize a new <see cref="Record"/> instance.
            /// </summary>
            /// <param name="tag">Record tag.</param>
            /// <param name="idx">Record index.</param>
            /// <param name="type">Record value <see cref="Type"/>.</param>
            /// <param name="value">Record initial value.</param>
            public Record(string tag, int idx, Type type, object value)
            {
                Tag = tag;
                Idx = idx;
                ValueType = type;
                Value = value;
            }

            #endregion
        }

        #endregion

        #region Fields

        /// <summary>
        /// List of data table records.
        /// </summary>
        private List<Record> m_RecordList;
        /// <summary>
        /// Dictionary of table records.
        /// </summary>
        private Dictionary<string, Record> m_RecordDic;
        /// <summary>
        /// Queue of the free idx in the <see cref="m_RecordList"/> field.
        /// </summary>
        private Queue<int> m_FreeIdx;
        /// <summary>
        /// Lock object used to handle concurrent operation.
        /// </summary>
        private object m_Lock;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of <see cref="DataStore"/>.
        /// </summary>
        public DataStore()
        {
            m_RecordList = new List<Record>();
            m_RecordDic = new Dictionary<string, Record>();
            m_FreeIdx = new Queue<int>();
            m_Lock = new object();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the given index is valid or not.
        /// </summary>
        /// <param name="idx">The index to be checked.</param>
        /// <returns>True if the index is valid, false otherwise.</returns>
        private bool IsValidIndex(int idx)
        {
            lock (m_Lock)
            {
                return idx >= 0 && idx < m_RecordList.Count && !m_FreeIdx.Contains(idx);
            }
        }
        /// <summary>
        /// Gets the first free idx if any.
        /// </summary>
        /// <returns>The first avaialble free idx if any, -1 otherwise.</returns>
        private int GetFreeIdx()
        {
            lock (m_Lock)
            {
                if (m_FreeIdx.Count > 0)
                {
                    return m_FreeIdx.Dequeue();
                }
                else
                {
                    return m_RecordList.Count;
                }
            }
        }
        /// <summary>
        /// Create a new record of <typeparamref name="T"/> <see cref="Type"/> in the data store.
        /// </summary>
        /// <typeparam name="T">The record <see cref="Type"/>.</typeparam>
        /// <param name="tag">The tag of the record that has to be created.</param>
        /// <param name="defaultValue">Default record value.</param>
        /// <returns>The index of the new <see cref="Record"/>.</returns>
        public int Create<T>(string tag = null, T defaultValue = default(T))
        {
            lock (m_Lock)
            {
                //check index
                int idx = GetFreeIdx();
                //check key
                if(string.IsNullOrEmpty(tag))
                {
                    //create a dummy tag
                    tag = string.Format("Tag{0}", idx);
                }
                else if (m_RecordDic.ContainsKey(tag)) //no need to check when a dummy tag is created
                {
                    throw new ZeusException(ErrorCodes.DuplicatedTag, string.Format("The tag {0} already exists in the data store", tag));
                }
                //create the new record
                Record newRecord = new Record(tag, idx, typeof(T), defaultValue);
                //add it to list and dictionary
                m_RecordDic.Add(newRecord.Tag, newRecord);
                if (newRecord.Idx < m_RecordList.Count)
                {
                    m_RecordList[newRecord.Idx] = newRecord;
                }
                else
                {
                    m_RecordList.Add(newRecord);
                }
                return idx;
            }
        }
        /// <summary>
        /// Remove all the records from the <see cref="DataStore"/>.
        /// </summary>
        public void Clear()
        {
            lock (m_Lock)
            {
                m_RecordList.Clear();
                m_RecordDic.Clear();
                m_FreeIdx.Clear();
            }
        }
        /// <summary>
        /// Remove the record at the given index from the <see cref="DataStore"/>.
        /// </summary>
        /// <param name="idx">The index of the record that has to be removed.</param>
        public void Remove(int idx)
        {
            lock(m_Lock)
            {
                if (!IsValidIndex(idx))
                {
                    throw new ZeusException(ErrorCodes.InvalidIndex, string.Format("The index {0} does not exists in the data store", idx));
                }
                m_FreeIdx.Enqueue(idx);
                m_RecordDic.Remove(m_RecordList[idx].Tag);
                m_RecordList[idx] = null;
            }
        }
        /// <summary>
        /// Remove the record with the given tag from the <see cref="DataStore"/>.
        /// </summary>
        /// <param name="tag">The tag of the record that has to be removed.</param>
        public void Remove(string tag)
        {
            lock(m_Lock)
            {
                if (!m_RecordDic.ContainsKey(tag))
                {
                    throw new ZeusException(ErrorCodes.InvalidTag, string.Format("The tag {0} does not exists in the data store", tag));
                }
                int idx = m_RecordDic[tag].Idx;
                m_RecordDic.Remove(tag);
                if (IsValidIndex(idx))
                {
                    m_RecordList[idx] = null;
                    m_FreeIdx.Enqueue(idx);
                }
            }
        }
        /// <summary>
        /// Set the value of an item in the data store.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the new value.</typeparam>
        /// <param name="record">The record which that has to be set.</param>
        /// <param name="value">The new record value.</param>
        private void Set<T>(Record record, T value)
        {
            if (!(record.Value is T))
            {
                Type inType = typeof(T);
                TypeConverter converter = TypeDescriptor.GetConverter(record.ValueType);
                if (converter.CanConvertFrom(inType))
                {
                    record.Value = converter.ConvertFrom(value);
                }
                else
                {
                    throw new ZeusException(ErrorCodes.TypeMismatch, string.Format("Type {0} is not compatible with stored type ({1})", inType.Name, record.ValueType.Name));
                }
            }
            else
            {
                record.Value = value;
            }
        }
        /// <summary>
        /// Set the value of an item in the data store.
        /// If the tag does not exists it will be created.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the new value.</typeparam>
        /// <param name="tag">The tag that has to be set.</param>
        /// <param name="value">The new record value.</param>
        public void Set<T>(string tag, T value)
        {
            if (!m_RecordDic.ContainsKey(tag))
            {
                Create<T>(tag, value);
            }
            else
            {
                Record record = m_RecordDic[tag];
                Set<T>(record, value);
            }
        }
        /// <summary>
        /// Set the value of an item in the data store.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the new value.</typeparam>
        /// <param name="idx">The index that has to be set.</param>
        /// <param name="value">The new record value.</param>
        public void Set<T>(int idx, T value)
        {
            if (!IsValidIndex(idx))
            {
                throw new ZeusException(ErrorCodes.InvalidIndex, string.Format("The index {0} is not present in the data store", idx));
            }
            else
            {
                Record record = m_RecordList[idx];
                Set<T>(record, value);
            }
        }
        /// <summary>
        /// Get the value of a record in the data store.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the value to be retrieved.</typeparam>
        /// <param name="record">The record which value has to be retrieved.</param>
        /// <returns>The stored value.</returns>
        private T Get<T>(Record record)
        {
            if (!(record.Value is T))
            {
                Type outType = typeof(T);
                TypeConverter converter = TypeDescriptor.GetConverter(outType);
                //check type compatibility
                if (!converter.CanConvertFrom(record.ValueType))
                {
                    throw new ZeusException(ErrorCodes.TypeMismatch, string.Format("Requested type {0} is incompatible with stored type {1}", outType.Name, record.ValueType.Name));
                }
                //everithing ok
                return (T)converter.ConvertFrom(record.Value);
            }
            else
            {
                return (T)record.Value;
            }
        }
        /// <summary>
        /// Get the value of an item in the data store.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the value to be retrieved.</typeparam>
        /// <param name="tag">The tag of the value to be retrieved.</param>
        /// <returns>The stored value.</returns>
        public T Get<T>(string tag)
        {
            //check tag
            if (!m_RecordDic.ContainsKey(tag))
            {
                throw new ZeusException(ErrorCodes.InvalidTag, string.Format("The tag {0} is not present in the data store", tag));
            }
            //get the record
            Record record = m_RecordDic[tag];
            return Get<T>(record);
        }
        /// <summary>
        /// Get the value of an item in the data store.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the value to be retrieved.</typeparam>
        /// <param name="idx">The index of the value to be retrieved.</param>
        /// <returns>The stored value.</returns>
        public T Get<T>(int idx)
        {
            //check idx
            if (!IsValidIndex(idx))
            {
                throw new ZeusException(ErrorCodes.InvalidIndex, string.Format("The requested index {0} is not present in the data store", idx));
            }
            //get the record
            Record record = m_RecordList[idx];
            return Get<T>(record);
        }
        /// <summary>
        /// Try to get the requested value from the data store.
        /// If the value is not found or requested type is not compatible the <paramref name="defaultValue"/> value is returned.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the value to be retrieved.</typeparam>
        /// <param name="tag">The tag of the value to be retrieved.</param>
        /// <param name="defaultValue">The default value returned in case of error.</param>
        /// <returns>The stored value if everithing is ok, the <paramref name="defaultValue"/> otherwise.</returns>
        public T TryGet<T>(string tag, T defaultValue)
        {
            try
            {
                return Get<T>(tag);
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// Try to get the requested value from the data store.
        /// If the value is not found or requested type is not compatible the <paramref name="defaultValue"/> value is returned.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the value to be retrieved.</typeparam>
        /// <param name="idx">The index of the value to be retrieved.</param>
        /// <param name="defaultValue">The default value returned in case of error.</param>
        /// <returns>The stored value if everithing is ok, the <paramref name="defaultValue"/> otherwise.</returns>
        public T TryGet<T>(int idx, T defaultValue)
        {
            try
            {
                return Get<T>(idx);
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// Gets the index of the record with the requested tag.
        /// </summary>
        /// <param name="tag">The tag of the record wich index has to be retrieved.</param>
        /// <returns>The index of the record associated with the requested tag.</returns>
        public int GetIdxFromTag(string tag)
        {
            //check tag
            if (!m_RecordDic.ContainsKey(tag))
            {
                throw new ZeusException(ErrorCodes.InvalidTag, string.Format("The requested tag {0} is not present in the data store.", tag));
            }
            //everithing ok
            return m_RecordDic[tag].Idx;
        }
        /// <summary>
        /// Gets the tag of the record with the requested index.
        /// </summary>
        /// <param name="idx">The index of the record wich tag has to be retrieved.</param>
        /// <returns>The tag of the record associated with the requested index.</returns>
        public string GetTagFromIdx(int idx)
        {
            //check index
            if (!IsValidIndex(idx))
            {
                throw new ZeusException(ErrorCodes.InvalidIndex, string.Format("The requested index {0} is not present in the data store.", idx));
            }
            //everithing ok
            return m_RecordList[idx].Tag;
        }
        /// <summary>
        /// Gets all the avaialble tags of the <see cref="DataStore"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of all the avaialble tags.</returns>
        public IEnumerable<string> GetTags()
        {
            return m_RecordList.Select(r => r.Tag);
        }
        /// <summary>
        /// Gets all the avaialble index of the <see cref="DataStore"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of all the available indexes.</returns>
        public IEnumerable<int> GetIdxs()
        {
            return m_RecordList.Select(r => r.Idx);
        }

        #endregion
    }
}
