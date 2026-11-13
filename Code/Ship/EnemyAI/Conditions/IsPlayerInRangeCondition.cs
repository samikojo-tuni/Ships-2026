using System;
using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class IsPlayerInRangeCondition : AIConditionBase
	{
		[Export] private float _detectionRange = 30.0f;

		public float DetectionRangeSqr => _detectionRange * _detectionRange;

		public override bool Decide(EnemyShip ship)
		{
			if (ship == null)
			{
				return false;
			}

			PlayerShip player = Level.Current.Player;
			if (player == null)
			{
				return false;
			}

			float distanceSqr = ship.GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
			bool decision = distanceSqr <= DetectionRangeSqr;

			if (InvertDecision)
			{
				return !decision;
			}

			return decision;
		}
	}
}