using Godot;
using System;

namespace GA.Ships
{
	[GlobalClass]
	public partial class ClearNavigationDataAction : AIActionBase
	{
		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			ship.ClearNavigationData();
		}
	}
}