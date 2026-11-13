using GA.Ships.Pathfinding;
using Godot;
using GA.Common;
using GA.Ships.Navigation;
using System.Collections.Generic;

namespace GA.Ships
{
	public partial class Level : Node3D
	{
		#region Statics
		private static Level _current = null;

		public static Level Current { get { return _current; } }
		#endregion Statics

		[Export] private NavigationGrid _grid = null;

		private PathDebugDrawer _pathVisualizer = null;

		private PlayerShip _player = null;

		public PlayerShip Player => _player;

		public Pathfinder Pathfinder
		{
			get;
			private set;
		}

		public NavigationGrid Grid
		{
			get { return _grid; }
		}

		public Level()
		{
			_current = this;
		}

		public override void _Ready()
		{
			if (_grid == null)
			{
				_grid = this.GetNode<NavigationGrid>(recursive: false);
			}

			Pathfinder = new Pathfinder(_grid);

			_pathVisualizer = this.GetNode<PathDebugDrawer>();
		}

		public void DrawPath(IList<Vector3> path)
		{
			if (_pathVisualizer != null)
			{
				_pathVisualizer.DrawPath(path);
			}
		}

		public void RegisterPlayer(PlayerShip player)
		{
			_player = player;
		}
	}
}