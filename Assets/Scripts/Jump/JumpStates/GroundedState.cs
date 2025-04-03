using Platformer.Mechanics;

namespace Platformer.Core
{
    public class GroundedState : BaseJumpState
    {
        public GroundedState(PlayerController playerController, JumpStateMachine jumpStateMachine) : base(playerController, jumpStateMachine)
        {
        }

        public override void Enter() { }

        public override void Update()
        {
            if (playerController.IsGrounded && playerController.InputController.IsJumpPressed())
            {
                jumpStateMachine.SetState(new PrepareToJumpState(playerController, jumpStateMachine));
            }
        }

        public override void Exit() { }
    }
}