using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class FollowPathfindingPathAction : AIActionBase
	{
		[Export] private float _arrivalDistance = 1.0f;

		private float _arrivalDistanceSqr = 0;

		public FollowPathfindingPathAction() : base()
		{
			_arrivalDistanceSqr = _arrivalDistance * _arrivalDistance;
		}

		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			if (ship == null || ship.Brain == null
				|| ship.PathfindingPath == null || ship.PathfindingPath.Count == 0)
			{
				return;
			}

			// Have we reached the next pathfinding target point?
			if (HasArrived(ship) || HasPassed(ship))
			{
				// TODO: Add also dot product check between initial direction to target and current direction to target.
				ship.AdvancePathfindingPathTarget();
			}
		}

		private bool HasPassed(EnemyShip ship)
		{
			Vector3 currentDirection = ship.CurrentPathfindingTarget - ship.GlobalPosition;
			return ship.CurrentPathfindingDirection.Dot(currentDirection) <= 0;
		}

		private bool HasArrived(EnemyShip ship)
		{
			return ship.GlobalPosition.DistanceSquaredTo(ship.CurrentPathfindingTarget)
							<= _arrivalDistanceSqr;
		}
	}
}