using Godot;

namespace GA.Ships
{
	public enum ProcessMode
	{
		None = 0,
		Frame,
		PhysicsFrame,
	}

	public abstract partial class AIActionBase : Resource
	{
		[Export] public ProcessMode ProcessMode { get; private set; } = ProcessMode.Frame;
		public abstract void Act(EnemyShip ship, float deltaTime = 0);
	}
}