using Godot;
using System;

namespace GA.Ships
{
	[GlobalClass]
	public partial class SetPlayerTargetAction : AIActionBase
	{
		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			PlayerShip player = Level.Current.Player;
			if (ship == null || player == null)
			{
				return;
			}

			ship.Target = player;
		}
	}
}