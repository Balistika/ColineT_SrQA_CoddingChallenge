using Platformer.Services;

namespace Platformer.Mechanics
{
    public class PrepareToJumpState : BaseJumpState
    {
        public PrepareToJumpState(IPlayerController playerController) : base(playerController) { }

        public override void Enter() { }

        public override void Update()
        {
            playerController.ApplyJumpImpulse();
            playerController.JumpStateMachine.SetState(new JumpingState(playerController));
        }
        public override void Exit() { }
    }
}