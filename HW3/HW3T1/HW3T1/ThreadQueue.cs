using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace HW3T1
{
    /// <summary>
    /// Safe queue for thread pool class.
    /// </summary>
    public class ThreadQueue<T>
    {
        public ThreadQueue()
        {
            this.queue = new Queue<T>();
        }

        private Queue<T> queue;

        /// <summary>
        /// Add element to the beginning.
        /// </summary>
        /// <param name="element"></param>
        public void Enqueue(T element)
        {
            lock(this.queue)
            {
                queue.Enqueue(element);
            }
        }

        /// <summary>
        /// Delete element from head.
        /// </summary>
        /// <returns>Deleted data.</returns>
        public T Dequeue()
        {
            lock (this.queue)
            {
                return queue.Dequeue();
            }
        }

        /// <summary>
        /// Check that queue is empty.
        /// </summary>
        public bool IsEmpty()
        {
            lock(this.queue)
            {
                return this.queue.Count == 0;
            }
        }
    }
}
