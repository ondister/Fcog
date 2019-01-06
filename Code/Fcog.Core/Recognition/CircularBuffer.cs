using System.Collections.Generic;

namespace Fcog.Core.Recognition
{
    public class CircularBuffer<T>
    {
        private readonly T[] buffer;
        private int nextFree;

        public CircularBuffer(int capacity)
        {
            Capacity = capacity;
            Count = 0;
            buffer = new T[capacity];
        }

        public int Capacity { get; }

        public int Count { get; private set; }

        public IEnumerable<T> Items => buffer;

        public void Add(T value)
        {
            buffer[nextFree] = value;
            nextFree = (nextFree+1) % buffer.Length;
            Count = System.Math.Min(Count+1, Capacity);
        }
    }
}