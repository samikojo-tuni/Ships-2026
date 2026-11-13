using System;
using System.Collections.Generic;
using GA.Common;
using GA.Ships.Pathfinding;
using Godot;

namespace GA.Ships
{
	public partial class EnemyShip : ShipBase
	{
		[Export] private PatrolPath _patrolPath = null;

		#region Pathfinding
		private float _pathfindingTimer = 0;

		// Path returned by the pathfinder to the target.
		private List<Vector3> _pathfindingPath = new List<Vector3>();
		[Export] private PathPostprocessorResource[] _pathPostprocessors = null;

		#endregion Pathfinding
		private ITarget _target = null;

		// EnemyAI system should be stateless. This means, all enemy data
		// should be stored here. States and actions modify that data.

		public EnemyBrain Brain { get; private set; } = null;

		public PatrolPath PatrolPath => _patrolPath;

		public bool IsPatrolPathValid
		{
			get => _patrolPath != null && _patrolPath.Length > 0;
		}

		public bool IsPathfindingPathValid
		{
			get => _pathfindingPath != null && _pathfindingPath.Count > 0;
		}

		public int CurrentWaypointIndex { get; set; } = 0;

		public IReadOnlyList<Vector3> PathfindingPath => _pathfindingPath;
		public float PathfindingTimer
		{
			get => _pathfindingTimer;
			set => _pathfindingTimer = value;
		}

		public ITarget Target
		{
			get => _target;
			set
			{
				_target = value;
				// TODO: Add data validation.
			}
		}

		public Vector3 CurrentPathfindingTarget
		{
			get
			{
				return IsPathfindingPathValid
					? _pathfindingPath[0]
					: Target != null
						? Target.TargetPosition
						: GlobalPosition;
			}
		}

		public Vector3 CurrentPathfindingDirection
		{
			get;
			set;
		}

		public override void _Ready()
		{
			base._Ready();

			Brain = this.GetNode<EnemyBrain>();
			if (Brain != null)
			{
				Brain.Initialize(this);
			}
			else
			{
				GD.PushError("Brain for the enemy ship can't be found!");
			}
		}

		public void SetPatrolPath(PatrolPath patrolPath)
		{
			if (patrolPath != null)
			{
				_patrolPath = patrolPath;
			}
		}

		public void SetPathfindingPath(IList<Vector3> pathfindingPath)
		{
			_pathfindingPath.Clear();
			if (pathfindingPath == null || pathfindingPath.Count == 0)
			{
				GD.PushWarning("No path provided for the enemy ship!");
				return;
			}

			if (_pathPostprocessors != null)
			{
				foreach (var postProcessor in _pathPostprocessors)
				{
					if (postProcessor != null)
					{
						pathfindingPath = postProcessor.PostProcess(pathfindingPath);
					}
				}
			}

			foreach (Vector3 cellPosition in pathfindingPath)
			{
				_pathfindingPath.Add(cellPosition);
			}

			Level.Current.DrawPath(_pathfindingPath);
		}

		public void AdvancePatrolIndex()
		{
			if (!IsPatrolPathValid)
			{
				return;
			}

			CurrentWaypointIndex = _patrolPath.GetNextIndex(CurrentWaypointIndex);
		}

		public void AdvancePathfindingPathTarget()
		{
			if (_pathfindingPath != null && _pathfindingPath.Count > 0)
			{
				if (_pathfindingPath.Count > 1)
				{
					CurrentPathfindingDirection = (_pathfindingPath[1] - _pathfindingPath[0]).Normalized();
				}
				_pathfindingPath.RemoveAt(0);
			}
		}

		public void ClearNavigationData()
		{
			_pathfindingPath.Clear();
			_pathfindingTimer = 0;
			CurrentPathfindingDirection = Vector3.Zero;
			CurrentWaypointIndex = 0;
		}
	}
}