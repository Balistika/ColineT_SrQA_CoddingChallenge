using Platformer.Gameplay;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;

namespace Platformer.Core
{
    public class InFlightState : BaseJumpState
    {
        private bool jumpStopped = false;
        
        public InFlightState(PlayerController playerController, JumpStateMachine jumpStateMachine) : base(playerController, jumpStateMachine)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            if (playerController.IsGrounded)
            {
                Schedule<PlayerLanded>().player = playerController;
                jumpStateMachine.SetState(new LandedState(playerController, jumpStateMachine));
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