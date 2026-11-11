using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class MoveTowardTargetAction : AIActionBase
	{
		private const float _minDistanceToTarget = 0.5f;

		[Export] private Curve _angleSpeedCurve = null;

		[Export(PropertyHint.Range, "0, 90, 1, radians_as_degrees")]
		private float _maxAngle = 0;

		[Export] private float _breakingDistance = 5.0f;

		private float MinDistanceToTarget
		{
			get => _minDistanceToTarget * _minDistanceToTarget;
		}

		private float BreakingDistanceSqr
		{
			get => _breakingDistance * _breakingDistance;
		}

		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			if (ship == null || ship.ShipData == null)
			{
				return;
			}

			float angleToTargetPoint = 0;

			Vector3 currentVelocity = ship.Velocity;
			Vector3 xzVelocity = new Vector3(currentVelocity.X, 0, currentVelocity.Z);

			Vector3 forward = ship.GlobalBasis.Z;
			forward.Y = 0;
			forward.Normalized();

			Vector3 toTargetPoint = ship.CurrentPathfindingTarget - ship.GlobalPosition;
			toTargetPoint.Y = 0;
			float sqrDistanceToTarget = toTargetPoint.LengthSquared();

			if (sqrDistanceToTarget > MinDistanceToTarget)
			{
				angleToTargetPoint = forward.AngleTo(toTargetPoint);
			}

			float speedScale = CalculateSpeedScale(angleToTargetPoint, sqrDistanceToTarget);

			Vector3 targetVelocity = forward * (ship.ShipData.MovementSpeed * speedScale);
			float blendSpeed = targetVelocity.Length() < 0.01f ? ship.ShipData.Acceleration : ship.ShipData.Deceleration;

			xzVelocity = xzVelocity.Lerp(targetVelocity, blendSpeed * deltaTime);

			currentVelocity.X = xzVelocity.X;
			currentVelocity.Z = xzVelocity.Z;

			ship.Velocity = currentVelocity;
			ship.MoveAndSlide();
		}

		private float CalculateSpeedScale(float angleToTarget, float sqrDistanceToTarget)
		{
			float absAngle = Mathf.Abs(angleToTarget);
			if (absAngle >= _maxAngle)
			{
				return 0.0f;
			}

			float t = Mathf.Clamp(absAngle / Mathf.Max(_maxAngle, 0.01f), 0.0f, 1.0f);
			float speedMultiplyer = _angleSpeedCurve != null
				? _angleSpeedCurve.Sample(t)
				: 1.0f - t;

			float breakScale = 1.0f;
			if (BreakingDistanceSqr > 0)
			{
				breakScale = Mathf.Clamp(sqrDistanceToTarget / BreakingDistanceSqr, 0.0f, 1.0f);
			}

			return Mathf.Clamp(speedMultiplyer * breakScale, 0.0f, 1.0f);
		}
	}
}