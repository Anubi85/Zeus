using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus.Extensions;

namespace Zeus
{
    /// <summary>
    /// Represents a strongly typed circular buffer.
    /// </summary>
    /// <typeparam name="T">The type of the objects stored in the buffer.</typeparam>
    public class CircularBuffer<T> : ICollection, IEnumerable<T>
    {
        #region Fields

        /// <summary>
        /// The syncronization object used by the buffer.
        /// </summary>
        private object m_SyncRoot;
        /// <summary>
        /// The array that stores the collection data;
        /// </summary>
        private T[] m_DataStore;
        /// <summary>
        /// The circular index that point to the head element.
        /// </summary>
        private CircularIndex m_Head;

        #endregion

        #region Constructor

        /// <summary>
        /// creates a new <see cref="CircularBuffer{T}"/> and initialize its internal fields.
        /// </summary>
        /// <param name="capacity">The <see cref="CircularBuffer{T}"/> capacity.</param>
        public CircularBuffer(int capacity)
        {
            m_SyncRoot = new object();
            m_Head = new CircularIndex(capacity);
            m_DataStore = new T[capacity];
            Count = 0;
        }

        #endregion

        #region ICollection<T> interface

        /// <summary>
        /// Gets the number of elements contained in the <see cref="CircularBuffer{T}"/> .
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        ///  Gets a value indicating whether access to the <see cref="CircularBuffer{T}"/> is synchronized (thread safe)
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="CircularBuffer{T}"/>.
        /// </summary>
        public object SyncRoot
        {
            get { return m_SyncRoot; }
        }
        /// <summary>
        /// Copies the elements of the <see cref="CircularBuffer{T}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The <see cref="Array"/> where the <see cref="CircularBuffer{T}"/> elements will be copy.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            //check array size
            if (array.Length - index < Count)
            {
                throw new ArgumentException("Provided array has not enough space.");
            }
            if (Count != 0)
            {
                int tail = m_Head - Count;
                if (m_Head > tail)
                {
                    Array.Copy(m_DataStore, tail, array, index, Count);
                }
                else
                {
                    Array.Copy(m_DataStore, tail, array, index, Count - tail);
                    Array.Copy(m_DataStore, 0, array, index + Count - tail, m_Head);
                }
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new element to the <see cref="CircularBuffer{T}"/>.
        /// </summary>
        /// <param name="element">The new element that has to be added.</param>
        public void Add(T element)
        {
            m_DataStore[m_Head++] = element;
            Count = (++Count).Clamp(0, Capacity);
        }
        /// <summary>
        /// Updates the buffer capacity to the new value.
        /// </summary>
        /// <param name="newCapacity">The new buffer capacity.</param>
        public void ChangeCapacity(int newCapacity)
        {
            if (newCapacity == m_DataStore.Length)
            {
                return;
            }
            T[] tmp = new T[newCapacity];
            int elemToCopy = Math.Min(Count, newCapacity);
            int tail = m_Head - elemToCopy;
            Array.Copy(m_DataStore, tail, tmp, 0, Count - tail);
            if (Count - tail < elemToCopy)
            {
                Array.Copy(m_DataStore, 0, tmp, Count - tail, m_Head);
            }
            m_Head = new CircularIndex(newCapacity, elemToCopy);
            Count = elemToCopy;
            m_DataStore = tmp;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a flag that tell if the buffer is full.
        /// </summary>
        public bool IsFull { get { return Count == m_DataStore.Length; } }
        /// <summary>
        /// Gets the buffer capacity.
        /// </summary>
        public int Capacity { get { return m_DataStore.Length; } }

        #endregion

        #region Enumerator class

        /// <summary>
        /// Enumerator class used to iterate throught the element of the collection.
        /// </summary>
        private class Enumerator : IEnumerator<T>
        {
            #region Fields

            /// <summary>
            /// The buffer instance that has to be enumerated.
            /// </summary>
            private CircularBuffer<T> m_Buffer;
            /// <summary>
            /// The index of the current item in the buffer.
            /// </summary>
            private CircularIndex m_CurrentIdx;
            /// <summary>
            /// Count the number of iterated items;
            /// </summary>
            private int m_ItemCount;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a new instance of the <see cref="Enumerator"/> for the <see cref="CircularBuffer{T}"/> provided.
            /// </summary>
            /// <param name="buffer">The <see cref="CircularBuffer{T}"/> object that has to be enumerated.</param>
            public Enumerator(CircularBuffer<T> buffer)
            {
                m_Buffer = buffer;
                m_CurrentIdx = new CircularIndex(buffer.Capacity, buffer.m_Head - buffer.Count) - 1;
                m_ItemCount = 0;
            }

            #endregion

            #region IEnumerator<T> interface

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                get { return m_Buffer.m_DataStore[m_CurrentIdx]; }
            }
            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            object IEnumerator.Current
            {
                get { return Current; }
            }
            /// <summary>
            /// Release <see cref="Enumerator"/> resources.
            /// </summary>
            public void Dispose()
            {
            }
            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                m_CurrentIdx++;
                m_ItemCount++;
                return m_ItemCount <= m_Buffer.Count;
            }
            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                m_CurrentIdx = new CircularIndex(m_Buffer.Capacity, m_Buffer.m_Head - m_Buffer.Count);
                m_ItemCount = 0;
            }

            #endregion
        }

        #endregion
    }
}
