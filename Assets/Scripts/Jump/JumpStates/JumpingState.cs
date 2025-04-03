using Platformer.Gameplay;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;

namespace Platformer.Core
{
    public class JumpingState : BaseJumpState
    {
        public JumpingState(PlayerController playerController, JumpStateMachine jumpStateMachine) : base(playerController, jumpStateMachine)
        {
        }

        public override void Enter() { }

        public override void Update()
        {
            if (!playerController.IsGrounded)
            {
                Schedule<PlayerJumped>().player = playerController;
                jumpStateMachine.SetState(new InFlightState(playerController, jumpStateMachine));
            }
        }

        public override void Exit() { }
    }
}