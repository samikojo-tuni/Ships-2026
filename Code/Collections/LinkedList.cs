using System.Collections;
using System.Collections.Generic;

namespace GA.Collections
{
	public class LinkedList<T> : ICollection<T>
	{
		protected class Node
		{
			public T Value { get; set; }
			public Node Next { get; set; }

			public Node() : this(default(T))
			{
			}

			public Node(T value, Node next = null)
			{
				Value = value;
				Next = next;
			}
		}

		/// <summary>
		/// The head of the linked list. When the list is empty, this will be null.
		/// </summary>
		protected Node Head { get; set; } = null;

		public int Count => throw new System.NotImplementedException();

		public bool IsReadOnly => throw new System.NotImplementedException();

		public void Add(T item)
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(T item)
		{
			throw new System.NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}