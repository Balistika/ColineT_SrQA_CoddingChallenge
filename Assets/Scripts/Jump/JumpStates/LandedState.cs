using Platformer.Services;

namespace Platformer.Core
{
    public class LandedState : BaseJumpState
    {
        public LandedState(IPlayerController playerController) : base(playerController)
        {
        }

        public override void Enter() { }

        public override void Update()
        {
            playerController.JumpStateMachine.SetState(new GroundedState(playerController));
        }
        public override void Exit() { }
    }
}