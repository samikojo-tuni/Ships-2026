using System.Collections.Generic;
using Godot;

namespace GA.Ships
{
	public partial class EnemyBrain : Node
	{
		[Export] private AIStateSet _states = null;
		[Export] private AIStateID _initialStateID = null;

		private EnemyShip _owner = null;
		private Dictionary<AIStateID, AIState> _stateLookup = new Dictionary<AIStateID, AIState>();

		public AIState CurrentState { get; private set; } = null;

		public void Initialize(EnemyShip owner)
		{
			_owner = owner;
			if (_states != null)
			{
				_stateLookup = _states.BuildLookup();
				if (!ChangeState(_initialStateID))
				{
					GD.PushError("Can't activate initial state");
				}
			}
			else
			{
				GD.PushError("EnemyBrain: State lookup table is null.");
			}
		}

		public override void _Process(double delta)
		{
			CurrentState.UpdateState(_owner, GA.Ships.ProcessMode.Frame, (float)delta);
			foreach (var targetStateID in CurrentState.TryTransitioning(_owner))
			{
				if (targetStateID != null)
				{
					if (ChangeState(targetStateID))
					{
						break;
					}
					else
					{
						GD.PushWarning($"Can't transition from {CurrentState.ID} to {targetStateID}");
					}
				}
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			CurrentState.UpdateState(_owner, GA.Ships.ProcessMode.PhysicsFrame, (float)delta);
		}

		public bool ChangeState(AIStateID targetStateID)
		{
			if (targetStateID == null
				|| !_stateLookup.TryGetValue(targetStateID, out AIState targetState))
			{
				// State is not found.
				return false;
			}

			if (CurrentState == targetState)
			{
				// Already in target state.
				return false;
			}

			if (CurrentState != null)
			{
				// Exit Current State
				CurrentState.ExitState(_owner);
			}

			CurrentState = targetState;

			if (CurrentState != null)
			{
				CurrentState.EnterState(_owner);
			}

			return true;
		}
	}
}