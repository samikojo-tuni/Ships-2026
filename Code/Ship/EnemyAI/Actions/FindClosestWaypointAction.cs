using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class FindClosestWaypointAction : AIActionBase
	{
		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			if (ship == null || ship.Brain == null || !ship.IsPathValid)
			{
				return;
			}

			int waypointIndex = ship.PatrolPath.GetClosestWaypointIndex(ship.GlobalPosition);
			if (waypointIndex >= 0)
			{
				ship.CurrentWaypointIndex = waypointIndex;
			}
		}
	}
}