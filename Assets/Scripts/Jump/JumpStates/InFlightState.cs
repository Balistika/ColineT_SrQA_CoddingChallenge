using Platformer.Gameplay;
using Platformer.Services;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class InFlightState : BaseJumpState
    {
        private bool jumpStopped = false;
        
        public InFlightState(IPlayerController playerController) : base(playerController) { }

        public override void Enter() { }

        public override void Update()
        {
            if (playerController.IsGrounded)
            {
                Schedule<PlayerLanded>().player = playerController;
                playerController.JumpStateMachine.SetState(new LandedState(playerController));
            }
            else if (playerController.InputController.IsJumpReleased() && !jumpStopped)
            {
                playerController.TriggerStopJump();
                jumpStopped = true;
            }
        }

        public override void Exit() { }
    }
}