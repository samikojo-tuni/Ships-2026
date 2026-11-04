using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class UpdatePathfindingPathAction : AIActionBase
	{
		// Negative value means, no refresh is done.
		[Export] private float _pathRefreshInterval = 1.0f;

		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			if (ship == null || ship.Brain == null || ship.Target == null)
			{
				return;
			}

			if (Level.Current == null || Level.Current.Pathfinder == null)
			{
				return;
			}

			ship.PathfindingTimer += deltaTime;

			bool isRefreshDue = _pathRefreshInterval >= 0
				&& ship.PathfindingTimer >= _pathRefreshInterval;

			if (ship.PathfindingPath.Count > 0 && !isRefreshDue)
			{
				return;
			}

			Vector3 targetPosition = ship.Target.TargetPosition;
			var pathfindingPath = Level.Current.Pathfinder.AStar(ship.GlobalPosition, targetPosition);
			ship.SetPathfindingPath(pathfindingPath);
			ship.PathfindingTimer = 0.0f;
		}
	}
}