using System.Collections.Generic;
using GA.Common;
using Godot;

namespace GA.Ships
{
	public partial class EnemyShip : ShipBase
	{
		[Export] private PatrolPath _patrolPath = null;

		private List<Vector3> _pathfindingPath = null;

		private ITarget _target = null;

		// EnemyAI system should be stateless. This means, all enemy data
		// should be stored here. States and actions modify that data.

		public EnemyBrain Brain { get; private set; } = null;

		public PatrolPath PatrolPath => _patrolPath;

		public bool IsPathValid
		{
			get => _patrolPath != null && _patrolPath.Length > 0;
		}

		public int CurrentWaypointIndex { get; set; } = 0;

		public IReadOnlyList<Vector3> PathfindingPath => _pathfindingPath;

		public ITarget Target
		{
			get => _target;
			set
			{
				_target = value;
				// TODO: Add data validation.
			}
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

		public void SetPatrolPath(PatrolPath path)
		{
			if (path != null)
			{
				_patrolPath = path;
			}
		}

		public void AdvancePatrolIndex()
		{
			if (!IsPathValid)
			{
				return;
			}

			CurrentWaypointIndex = _patrolPath.GetNextIndex(CurrentWaypointIndex);
		}
	}
}