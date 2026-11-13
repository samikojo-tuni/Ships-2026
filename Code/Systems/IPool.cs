using Godot;

namespace GA.Ships
{
	/// <summary>
	/// An interface for pooling any Godot Nodes.
	/// </summary>
	/// <typeparam name="T">A Node</typeparam>
	public interface IPool<T>
		where T : Node
	{
		/// <summary>
		/// Gets an object from the pool.
		/// </summary>
		///	<returns>Object from the pool, if there are any available. Null otherwise.</returns>
		T Get();

		/// <summary>
		/// Returns an object back to the pool.
		/// </summary>
		/// <param name="item">Object to return to the pool.</param>
		/// <returns><c>True</c> if the object was successfully returned to the pool; <c>false</c> otherwise.</returns>
		bool Return(T item);
	}
}