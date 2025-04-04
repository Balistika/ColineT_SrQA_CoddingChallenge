using Platformer.Services;
using UnityEngine;

namespace Platformer.Core
{
    public class GroundedState : BaseJumpState
    {
        public GroundedState(IPlayerController playerController) : base(playerController)
        {
        }

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