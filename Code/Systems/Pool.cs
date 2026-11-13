using System;
using System.Collections.Generic;
using Godot;

namespace GA.Ships
{
	public class Pool<T> : IPool<T>, IDisposable
		where T : Node
	{
		private PackedScene _sourceScene = null;
		private bool _canGrow = false;
		private Queue<T> _availableItems = null;
		private HashSet<T> _activeItems = null;

		public Pool(PackedScene sourceScene, int capacity, bool canGrow = false)
		{
			_sourceScene = sourceScene;
			_canGrow = canGrow;

			if (capacity < 0)
			{
				capacity = 0;
			}

			_availableItems = new Queue<T>(capacity);
			_activeItems = new HashSet<T>();

			for (int i = 0; i < capacity; ++i)
			{
				Add();
			}
		}

		public void Dispose()
		{
			if (_availableItems != null)
			{
				foreach (T item in _availableItems)
				{
					item.QueueFree();
				}
			}

			if (_activeItems != null)
			{
				foreach (T item in _activeItems)
				{
					item.QueueFree();
				}
			}

			_activeItems.Clear();
			_availableItems.Clear();
		}

		public T Get()
		{
			T item;

			if (_availableItems.Count > 0)
			{
				item = _availableItems.Dequeue();
			}
			else if (_canGrow)
			{
				item = Add(activateOnAdd: true);
			}
			else
			{
				// Pool exhausted and it cannot grow.
				return null;
			}

			_activeItems.Add(item);
			OnGet(item);

			return item;
		}

		public bool Return(T item)
		{
			if (item == null || !_activeItems.Contains(item))
			{
				// The item is either empty or it is not created by this pool.
				// In these cases prevent returning the item back to this pool.
				return false;
			}

			_activeItems.Remove(item);
			_availableItems.Enqueue(item);
			OnReturn(item);
			return true;
		}

		/// <summary>
		/// Custom activation functionality.
		/// </summary>
		protected virtual void OnGet(T item)
		{
		}

		/// <summary>
		/// Custom deactivation functionality.
		/// </summary>
		/// <param name="item"></param>
		protected virtual void OnReturn(T item)
		{
		}

		private T Add(bool activateOnAdd = false)
		{
			T item = _sourceScene.Instantiate<T>();
			if (!activateOnAdd)
			{
				_availableItems.Enqueue(item);
			}

			return item;
		}
	}
}