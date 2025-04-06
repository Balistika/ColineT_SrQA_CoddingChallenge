using Platformer.Services;

namespace Platformer.Mechanics
{
    public class GroundedState : BaseJumpState
    {
        public GroundedState(IPlayerController playerController) : base(playerController) { }

        public override void Enter() { }

        public override void Update()
        {
            if (playerController.IsGrounded && playerController.InputController.IsJumpPressed())
            {
                playerController.JumpStateMachine.SetState(new PrepareToJumpState(playerController));
            }
        }

        public override void Exit() { }
    }
}