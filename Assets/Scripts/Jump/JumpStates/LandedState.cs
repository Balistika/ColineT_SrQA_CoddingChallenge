using Platformer.Mechanics;

namespace Platformer.Core
{
    public class LandedState : BaseJumpState
    {
        public LandedState(PlayerController playerController, JumpStateMachine jumpStateMachine) : base(playerController, jumpStateMachine)
        {
        }

        public override void Enter()
        {
            jumpStateMachine.SetState(new GroundedState(playerController, jumpStateMachine));
        }

        public override void Update() { }
        public override void Exit() { }
    }
}