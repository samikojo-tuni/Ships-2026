using Godot;
using System.Collections.Generic;
using GA.Common;
using System;

namespace GA.Ships
{
	public partial class PatrolPath : Node3D
	{
		private IList<Waypoint> _path = null;

		public int Length
		{
			get
			{
				if (_path == null) { return 0; }
				return _path.Count;
			}
		}

		public override void _Ready()
		{
			Initialize();
		}

		private void Initialize()
		{
			_path = this.GetChildren<Waypoint>(recursive: false);
		}

		public int GetNextIndex(int currentIndex)
		{
			if (_path == null || Length == 0)
			{
				throw new InvalidOperationException("Patrol route has no points.");
			}

			return (currentIndex + 1) % Length;
		}

		/// <summary>
		/// Returns the index of the waypoint closest to the position.
		/// </summary>
		/// <param name="position"></param>
		/// <returns>The index of the closest waypoint or -1, if there are no waypoints.</returns>
		public int GetClosestWaypointIndex(Vector3 position)
		{
			if (_path == null || Length == 0)
			{
				return -1;
			}

			Waypoint closest = null;
			float nearestSqrDistance = float.PositiveInfinity;

			foreach (Waypoint waypoint in _path)
			{
				Vector3 toTarget = waypoint.TargetPosition - position;
				float sqrDistance = toTarget.LengthSquared();
				if (sqrDistance < nearestSqrDistance)
				{
					nearestSqrDistance = sqrDistance;
					closest = waypoint;
				}
			}

			return _path.IndexOf(closest);
		}

		public Waypoint GetWaypoint(int index)
		{
			if (index < 0 || index >= Length)
			{
				throw new IndexOutOfRangeException($"Invalid waypoint index {index} for patrol route with length {Length}");
			}

			if (_path == null)
			{
				throw new InvalidOperationException($"Path is null! Unable to retrieve a waypoint.");
			}

			return _path[index];
		}
	}
}