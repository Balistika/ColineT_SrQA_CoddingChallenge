using Platformer.Gameplay;
using Platformer.Services;
using static Platformer.Core.Simulation;

namespace Platformer.Core
{
    public class JumpingState : BaseJumpState
    {
        public JumpingState(IPlayerController playerController) : base(playerController)
        {
        }

        public override void Enter() { }

        public override void Update()
        {
            if (!playerController.IsGrounded)
            {
                Schedule<PlayerJumped>().player = playerController;
                playerController.JumpStateMachine.SetState(new InFlightState(playerController));
            }
        }

        public override void Exit() { }
    }
}