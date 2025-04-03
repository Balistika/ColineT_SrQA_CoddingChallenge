using Platformer.Mechanics;

namespace Platformer.Core
{
    public class PrepareToJumpState : BaseJumpState
    {
        public PrepareToJumpState(PlayerController playerController, JumpStateMachine jumpStateMachine) : base(playerController, jumpStateMachine)
        {
        }

        public override void Enter()
        {
            playerController.ApplyJumpImpulse();
            jumpStateMachine.SetState(new JumpingState(playerController, jumpStateMachine));
        }

        public override void Update() { }
        public override void Exit() { }
    }
}