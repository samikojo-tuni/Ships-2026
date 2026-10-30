using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class PatrolPathAction : AIActionBase
	{
		// NOTE: Instead of distance, use SqrDistance or Dot product.
		[Export] public float ArrivalDistance { get; set; } = 1.0f;

		public float SqrArrivalDistance
		{
			get;
			private set;
		}

		public PatrolPathAction()
		{
			SqrArrivalDistance = ArrivalDistance * ArrivalDistance;
		}

		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			if (ship == null || ship.Brain == null || !ship.IsPathValid)
			{
				return;
			}

			Waypoint currentWaypoint = ship.PatrolPath.GetWaypoint(ship.CurrentWaypointIndex);

			bool arrived = ship.Target == currentWaypoint
				&& ship.PathfindingPath.Count == 0
				&& ship.GlobalPosition.DistanceSquaredTo(currentWaypoint.TargetPosition) <= SqrArrivalDistance;

			if (arrived)
			{
				ship.AdvancePatrolIndex();
				currentWaypoint = ship.PatrolPath.GetWaypoint(ship.CurrentWaypointIndex);
			}

			ship.Target = currentWaypoint;
		}
	}
}