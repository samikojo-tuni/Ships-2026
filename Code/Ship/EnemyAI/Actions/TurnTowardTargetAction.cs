using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class TurnTowardTargetAction : AIActionBase
	{
		private const float DistanceMarginSqr = 0.1f;

		[Export] private float _turnSpeedMultiplyer = 1;

		public override void Act(EnemyShip ship, float deltaTime)
		{
			if (ship == null || ship.ShipData == null)
			{
				return;
			}

			Vector3 forward = ship.GlobalBasis.Z;
			Vector3 targetPosition = ship.CurrentPathfindingTarget;

			Vector3 toTarget = targetPosition - ship.GlobalPosition;
			toTarget.Y = 0;

			if (toTarget.LengthSquared() < DistanceMarginSqr)
			{
				return;
			}

			float targetYaw = Mathf.Atan2(toTarget.X, toTarget.Z);
			float currentYaw = Mathf.Atan2(forward.X, forward.Z);
			float delta = Mathf.Wrap(targetYaw - currentYaw, -Mathf.Pi, Mathf.Pi);

			float maxTurn = Mathf.DegToRad(ship.ShipData.RotationSpeed * _turnSpeedMultiplyer * deltaTime);
			float turnAmount = Mathf.Clamp(delta, -maxTurn, maxTurn);

			ship.RotateY(turnAmount);
		}
	}
}