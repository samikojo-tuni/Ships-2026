using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class Waypoint : Node3D, ITarget
	{
		public Vector3 TargetPosition => GlobalPosition;
	}
}