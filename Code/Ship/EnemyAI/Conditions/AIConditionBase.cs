using Godot;

namespace GA.Ships
{
	public abstract partial class AIConditionBase : Resource
	{
		[Export] private bool _invertDecision = false;

		protected bool InvertDecision => _invertDecision;

		public abstract bool Decide(EnemyShip ship);
	}
}