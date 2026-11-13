using System.Collections.Generic;
using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class AIState : Resource
	{
		[Export] public AIStateID ID { get; private set; } = null;
		[Export] public AIActionBase[] EnterActions { get; private set; } = new AIActionBase[0];
		[Export] public AIActionBase[] RunActions { get; private set; } = new AIActionBase[0];
		[Export] public AIActionBase[] ExitActions { get; private set; } = new AIActionBase[0];
		[Export] public AITransition[] Transitions { get; private set; } = new AITransition[0];

		/// <summary>
		/// Run when the state is activated.
		/// </summary>
		/// <param name="ship">Owner ship</param>
		public void EnterState(EnemyShip ship)
		{
			foreach (var action in EnterActions)
			{
				action.Act(ship);
			}
		}

		/// <summary>
		/// Run when the state is deactivated.
		/// </summary>
		/// <param name="ship"></param>
		public void ExitState(EnemyShip ship)
		{
			foreach (var action in ExitActions)
			{
				action.Act(ship);
			}
		}

		/// <summary>
		/// Run every frame when the state is active.
		/// </summary>
		/// <param name="ship"></param>
		/// <param name="deltaTime"></param>
		public void UpdateState(EnemyShip ship, ProcessMode processMode, float deltaTime)
		{
			PerformActions(ship, processMode, deltaTime);
		}

		public IEnumerable<AIStateID> TryTransitioning(EnemyShip ship)
		{
			foreach (var transition in Transitions)
			{
				bool decision = transition.Condition.Decide(ship);
				if (decision)
				{
					// TODO: Try to perform transition. Continue if fails.
					yield return transition.TargetStateID;
				}
			}

			// yield break stops iteration.
			yield break;
		}

		private void PerformActions(EnemyShip ship, ProcessMode processMode, float deltaTime)
		{
			foreach (var action in RunActions)
			{
				if (action.ProcessMode == processMode)
				{
					action.Act(ship, deltaTime);
				}
			}
		}
	}
}